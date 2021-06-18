using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamlineMVVM;

namespace HTTPHandler
{
    public partial class HTTP : IDisposable
    {
        public bool Download(string url, string downloadPath, bool overwrite)
        {
            return downloadFile(url, downloadPath, overwrite, false, null).Success;
        }

        public void DownloadAsync(string url, string downloadPath, bool overwrite)
        {
            if (_IsBusy)
            {
                return;
            }

            TaskScheduler currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
            source = new CancellationTokenSource();
            token = source.Token;
            HTTPComplete httpComplete = new HTTPComplete();
            _IsBusy = true;
            try
            {
                httpTask = Task.Factory.
                    StartNew(() => httpComplete = downloadFile(url, downloadPath, overwrite, true, currentSynchronizationContext), token, TaskCreationOptions.None, TaskScheduler.Default).
                    ContinueWith((t) => downloadAsyncComplete(HttpType.Download, httpComplete), currentSynchronizationContext);

            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error running download worker task.", Ex);
            }
        }

        // At some point, I will need to make this report progress better. The way it does it here is a bit blocky.
        private HTTPComplete downloadFile(string url, string filePath, bool overwrite, bool isAsync, TaskScheduler taskScheduler)
        {
            HTTPComplete httpComplete = new HTTPComplete();

            if (string.IsNullOrEmpty(filePath))
            {
                httpComplete.Error = true;
                LogWriter.LogEntry("Unable to download file. Destination path cannot be null or empty.");
                return httpComplete;
            }

            if (File.Exists(filePath) && overwrite == false)
            {
                httpComplete.Error = true;
                LogWriter.LogEntry("Unable to download file. Destination path exists and overwrite is set to false.");
                return httpComplete;
            }

            string path = "";
            try
            {
                path = Path.GetDirectoryName(filePath);
            }
            catch (Exception Ex)
            {
                httpComplete.Error = true;
                LogWriter.Exception("Failed to get path from destination file. Path: " + filePath, Ex);
                return httpComplete;
            }

            if (Directory.Exists(path) == false)
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception Ex)
                {
                    httpComplete.Error = true;
                    LogWriter.Exception("Error creating directory to save file. Path: " + filePath, Ex);
                    return httpComplete;
                }
            }

            HttpConnection httpConnectionData = new HttpConnection();
            httpConnectionData = processHTTPRequest(new HTTPRequest() { URL = url, });
            if (httpConnectionData.ResponseStream == null)
            {
                httpComplete.Error = true;
                LogWriter.LogEntry("Failed to download file. Response stream is null.");
                return httpComplete;
            }

            Stream bufferStream = null;
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                bufferStream = File.OpenWrite(filePath);
            }
            catch (Exception Ex)
            {
                httpComplete.Error = true;
                LogWriter.Exception("Error opening write stream to file. Path: " + filePath, Ex);
                bufferStream = null;
            }

            if (bufferStream == null)
            {
                httpComplete.Error = true;
                LogWriter.LogEntry("Something went wrong. Open write steam is null.");
                return httpComplete;
            }

            httpComplete.PayLoadSize = httpConnectionData.PayloadSize;
            int reportedPercentage = 0;

            if (isAsync)
            {
                downloadReportAsync(new HTTPProgress()
                {
                    BytesReceived = 0,
                    PayLoadSize = httpConnectionData.PayloadSize,
                    PercentageComplete = 0,
                }, taskScheduler);
            }

            try // Long try block. Not avoidable.
            {
                byte[] buffer = new byte[BufferSize];
                //httpConnectionData.ResponseStream.ReadTimeout = Timeout * 1000;
                int bytesRead = httpConnectionData.ResponseStream.Read(buffer, 0, BufferSize);

                while (bytesRead > 0 && CancellationRequested == false)
                {
                    httpComplete.BytesReceived += bytesRead;

                    HTTPProgress httpProgress = new HTTPProgress()
                    {
                        BytesReceived = httpComplete.BytesReceived,
                        PayLoadSize = httpConnectionData.PayloadSize,
                        PercentageComplete = calculatePercentage(httpConnectionData.PayloadSize, httpComplete.BytesReceived),
                    };
                    httpComplete.PercentageComplete = httpProgress.PercentageComplete;

                    if (isAsync && CancellationRequested)
                    {
                        break;
                    }

                    if (isAsync && httpConnectionData.PayloadSize > 100) // No point in really doing all this for very very small files.
                    {
                        switch (ReportType)
                        {
                            case ReportProgressType.Percentage:
                                if (httpProgress.PercentageComplete >= reportedPercentage + ProgessPercentage)
                                {
                                    reportedPercentage += ProgessPercentage;
                                    downloadReportAsync(httpProgress, taskScheduler);
                                }
                                break;

                            case ReportProgressType.Bytes:
                                downloadReportAsync(httpProgress, taskScheduler);
                                break;

                            default:
                                break;
                        }
                    }

                    bufferStream.Write(buffer, 0, bytesRead);
                    bytesRead = httpConnectionData.ResponseStream.Read(buffer, 0, BufferSize);
                }
            }
            catch (Exception Ex)
            {
                httpComplete.Error = true;
                LogWriter.Exception("There was a problem reading HTTP file. Url: " + url, Ex);
                return httpComplete;
            }
            finally
            {
                if (httpConnectionData.ResponseStream != null)
                {
                    httpConnectionData.ResponseStream.Dispose();
                }

                if (httpConnectionData.WebResponse != null)
                {
                    httpConnectionData.WebResponse.Close();
                }

                if (bufferStream != null)
                {
                    bufferStream.Dispose();
                }
            }

            if (isAsync && CancellationRequested)
            {
                return httpComplete;
            }

            if (File.Exists(filePath) == false)
            {
                httpComplete.Error = true;
                LogWriter.LogEntry("Something went wrong. File failed to download.");
                return httpComplete;
            }

            httpComplete.BytesReceived = httpConnectionData.PayloadSize;
            httpComplete.PercentageComplete = 100;
            httpComplete.Success = true;

            if (isAsync)
            {
                downloadReportAsync(new HTTPProgress()
                {
                    BytesReceived = httpConnectionData.PayloadSize,
                    PayLoadSize = httpConnectionData.PayloadSize,
                    PercentageComplete = 100,
                }, taskScheduler);
            }

            return httpComplete;
        }

        private int calculatePercentage(long totalBytes, long currentBytes)
        {
            int percentage = 0;

            try
            {
                percentage = Convert.ToInt32((Convert.ToDecimal(currentBytes) / Convert.ToDecimal(totalBytes)) * 100);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error calculating percentage. Total Bytes: " + Convert.ToString(totalBytes) + " | Current Bytes:" + Convert.ToString(currentBytes), Ex);
            }

            return percentage;
        }

        private void downloadReportAsync(HTTPProgress httpProgress, TaskScheduler taskScheduler)
        {
            if (taskScheduler == null)
            {
                return;
            }

            if (DownloadProgress == null)
            {
                return;
            }

            try
            {
                Task.Factory.StartNew(() => DownloadProgress(this, httpProgress), CancellationToken.None, TaskCreationOptions.None, taskScheduler);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("TaskWorker Error: Failed to run timeout task.", Ex);
            }
        }

        public void CancelDownloadAsync()
        {
            if (source != null)
            {
                try
                {
                    source.Cancel();
                }
                finally
                {
                    // Nothing really needs to be here.
                }
            }

            _CancellationRequested = true;
            downloadAsyncComplete(HttpType.Download, new HTTPComplete() { Cancelled = true });
        }
    }
}
