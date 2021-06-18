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
        // This function will only work on .txt/.json/.ini/.cfg files.
        // If the URL contains some sort of Content-Disposition header and is not directly to the file, this fuction will not work.
        public string[] ReadWebTextFile(string url)
        {
            return webTextFileReader(url).FileLines;
        }

        public void ReadWebTextFileAsync(string url)
        {
            if (_IsBusy)
            {
                return;
            }

            TaskScheduler currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
            source = new CancellationTokenSource();
            token = source.Token;
            HTTPReadComplete httpReadComplete = new HTTPReadComplete();
            _IsBusy = true;
            try
            {
                httpTask = Task.Factory.
                    StartNew(() => httpReadComplete = webTextFileReader(url), token, TaskCreationOptions.None, TaskScheduler.Default).
                    ContinueWith((t) => downloadAsyncComplete(HttpType.ReadWebTextFile, httpReadComplete), currentSynchronizationContext);

            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error running download worker task.", Ex);
            }
        }

        private HTTPReadComplete webTextFileReader(string url)
        {
            HTTPReadComplete httpReadComplete = new HTTPReadComplete();
            try
            {
                if (Path.GetExtension(url).ToLower() != ".txt" &&
                    Path.GetExtension(url).ToLower() != ".json" &&
                    Path.GetExtension(url).ToLower() != ".ini" &&
                    Path.GetExtension(url).ToLower() != ".cfg")
                {
                    httpReadComplete.Error = true;
                    return httpReadComplete;
                }
            }
            catch (Exception Ex)
            {
                httpReadComplete.Error = true;
                LogWriter.Exception("Failed to get url extension.", Ex);
                return httpReadComplete;
            }

            string[] fileLines = null;
            HttpConnection httpConnectionData = null;
            try
            {
                DateTime startTime = DateTime.UtcNow;
                httpConnectionData = processHTTPRequest(new HTTPRequest() { URL = url, });

                //httpConnectionData.ResponseStream.ReadTimeout = 1500;
                if (httpConnectionData.ResponseStream != null)
                {
                    fileLines = readStreamToArray(httpConnectionData.ResponseStream, null);
                }

                if (fileLines == null)
                {
                    httpReadComplete.Error = true;
                }
                else
                {
                    httpReadComplete.FileLines = fileLines.ToArray();
                }
                
                return httpReadComplete;
            }
            catch (Exception Ex)
            {
                httpReadComplete.Error = true;
                LogWriter.Exception("There was a problem reading HTTP file. Url: " + url, Ex);
                return httpReadComplete;
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
            }
        }
    }
}
