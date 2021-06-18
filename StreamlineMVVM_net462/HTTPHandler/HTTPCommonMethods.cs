using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using StreamlineMVVM;

namespace HTTPHandler
{
    public partial class HTTP : IDisposable
    {
        private readonly Uri baseURI = new Uri("http://www.BaseURI.com");
        public string ParseURLFileName(string url)
        {
            string fileName = "";
            try
            {
                Uri uri;
                if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                {
                    uri = new Uri(baseURI, url);
                }

                fileName = Path.GetFileName(uri.LocalPath);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error parsing file name from URL: " + url, Ex);
            }

            return fileName;
        }

        // NOTE: Request string can be null or empty since it is technically an optional field. HTTP requests may not always need to have data written to the request stream.
        private HttpConnection processHTTPRequest(HTTPRequest httpRequest)
        {
            HttpConnection httpConnectionData = new HttpConnection();

            try
            {
                httpConnectionData.WebRequest = (HttpWebRequest)WebRequest.Create(httpRequest.URL);
                httpConnectionData.WebRequest.ReadWriteTimeout = Timeout * 1000;

                httpConnectionData.WebRequest.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");
                httpConnectionData.WebRequest.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                foreach (HTTPHeader httpHeader in httpRequest.Headers)
                {
                    httpConnectionData.WebRequest.Headers.Add(httpHeader.Key, httpHeader.Value);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP connection error. Exception thrown trying to get http web response. Url: " + httpRequest.URL, Ex);
                return cleanup(httpConnectionData);
            }

            if (httpRequest.Authenticate)
            {
                httpConnectionData.WebRequest.PreAuthenticate = httpRequest.Authenticate;
            }

            if (string.IsNullOrEmpty(httpRequest.ContentType) == false)
            {
                httpConnectionData.WebRequest.ContentType = httpRequest.ContentType;
            }

            switch (httpRequest.Method)
            {
                case RequestMethod.GET:
                    httpConnectionData.WebRequest.Method = WebRequestMethods.Http.Get;
                    break;

                case RequestMethod.POST:
                    httpConnectionData.WebRequest.Method = WebRequestMethods.Http.Post;
                    break;

                case RequestMethod.PUT:
                    httpConnectionData.WebRequest.Method = WebRequestMethods.Http.Put;
                    break;

                case RequestMethod.HEAD:
                    httpConnectionData.WebRequest.Method = WebRequestMethods.Http.Head;
                    break;

                case RequestMethod.CONNECT:
                    httpConnectionData.WebRequest.Method = WebRequestMethods.Http.Connect;
                    break;

                case RequestMethod.DELETE:
                    httpConnectionData.WebRequest.Method = "DELETE";
                    break;

                case RequestMethod.PATCH:
                    httpConnectionData.WebRequest.Method = "PATCH";
                    break;

                case RequestMethod.OPTIONS:
                    httpConnectionData.WebRequest.Method = "OPTIONS";
                    break;

                case RequestMethod.TRACE:
                    httpConnectionData.WebRequest.Method = "TRACE";
                    break;
            }

            if (string.IsNullOrEmpty(httpRequest.RequestString) == false)
            {
                bool sent = true;

                Stream requestStream = null;
                try
                {
                    byte[] requestData = Encoding.UTF8.GetBytes(httpRequest.RequestString);
                    httpConnectionData.WebRequest.ContentLength = requestData.Length;
                    httpConnectionData.WebRequest.AllowWriteStreamBuffering = true;

                    requestStream = httpConnectionData.WebRequest.GetRequestStream();
                    requestStream.Write(requestData, 0, requestData.Length);
                }
                catch (Exception Ex)
                {
                    sent = false;
                    LogWriter.Exception("HTTP connection error. Exception thrown when attempting to write to request stream. Url: " + httpRequest.URL, Ex);
                }
                finally
                {
                    if (requestStream != null)
                    {
                        requestStream.Dispose();
                    }
                }

                if (sent == false)
                {
                    return cleanup(httpConnectionData);
                }
            }

            try
            {
                httpConnectionData.WebResponse = (HttpWebResponse)httpConnectionData.WebRequest.GetResponse();
                if (httpConnectionData.WebResponse == null)
                {
                    LogWriter.LogEntry("HTTP connection failure. Response is null. Url: " + httpRequest.URL);
                    return cleanup(httpConnectionData);
                }
            }
            catch (WebException Ex)
            {
                string responseBody = readStreamToText(Ex.Response.GetResponseStream(), null);

                HTTPHeader[] responseHeaders = httpHeaderCollection(Ex.Response.Headers);
                string responseHeadersText = "";
                for (int i = 0; i < responseHeaders.Length; i++)
                {
                    responseHeadersText = responseHeadersText + "   " + responseHeaders[i].Key + ": " + responseHeaders[i].Value + Environment.NewLine;
                }

                HTTPHeader[] requestHeaders = httpHeaderCollection(httpConnectionData.WebRequest.Headers);
                string requestHeadersText = "";
                for (int i = 0; i < requestHeaders.Length; i++)
                {
                    if (requestHeaders[i].Key.ToLower() == "authorization" && httpRequest.AuthMask)
                    {
                        string maskedToken = "*****";

                        for (int c = ((requestHeaders[i].Value.Length / 4) * 3); c < requestHeaders[i].Value.Length; c++)
                        {
                            maskedToken = maskedToken + requestHeaders[i].Value[c];
                        }

                        requestHeadersText = requestHeadersText + "   " + requestHeaders[i].Key + ": " + maskedToken + Environment.NewLine;
                    }
                    else
                    {
                        requestHeadersText = requestHeadersText + "   " + requestHeaders[i].Key + ": " + requestHeaders[i].Value + Environment.NewLine;
                    }
                }

                requestHeadersText = requestHeadersText.TrimEnd();

                string logError =
                    "HTTP connection error. Error code: " + Ex.Message + Environment.NewLine +
                    "Response Headers:" + Environment.NewLine + responseHeadersText +
                    "Response Message:" + Environment.NewLine + "   " + responseBody +
                    "Request Url: " + Environment.NewLine + "   " + httpRequest.URL + Environment.NewLine +
                    "Request Headers:" + Environment.NewLine + requestHeadersText;

                if (httpRequest.RequestMask == false)
                {
                    logError = logError + Environment.NewLine + "Request String:" + Environment.NewLine + "   " + httpRequest.RequestString;
                }

                LogWriter.Exception(logError, Ex);

                return cleanup(httpConnectionData);
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP connection error. General Exception thrown when attempting to get response. Url: " + httpRequest.URL, Ex);
                return cleanup(httpConnectionData);
            }

            try
            {
                httpConnectionData.StatusCode = (int)httpConnectionData.WebResponse.StatusCode;
                if (httpConnectionData.StatusCode != 200)
                {
                    LogWriter.LogEntry("HTTP Error. Bad response: " + Convert.ToString(httpConnectionData.StatusCode) + ". URL: " + httpRequest.URL);
                    return cleanup(httpConnectionData);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP connection error. Exception thrown trying to read response status code. Url: " + httpRequest.URL, Ex);
                return cleanup(httpConnectionData);
            }

            try
            {
                httpConnectionData.ResponseStream = httpConnectionData.WebResponse.GetResponseStream();
                if (httpConnectionData.ResponseStream == null)
                {
                    LogWriter.LogEntry("HTTP connection failure. Null response stream. URL: " + httpRequest.URL);
                    return cleanup(httpConnectionData);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP response read error. Exception thrown trying to read response status code. Url: " + httpRequest.URL, Ex);
                return cleanup(httpConnectionData);
            }

            try
            {
                if (httpConnectionData.WebResponse != null)
                {
                    httpConnectionData.PayloadSize = Convert.ToInt64(httpConnectionData.WebResponse.Headers.Get("Content-Length"));
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP connection error. Exception thrown when trying to get Content-Length from header data. Url: " + httpRequest.URL, Ex);
            }

            return httpConnectionData;
        }

        private string[] readStreamToArray(Stream stream, Encoding encoding)
        {
            List<string> streamLines = new List<string>();
            StreamReader streamReader = null;
            try
            {
                if (encoding == null)
                {
                    streamReader = new StreamReader(stream);
                    string line = "";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        streamLines.Add(line);
                    }
                }
                else
                {
                    streamReader = new StreamReader(stream, encoding);
                    streamLines.Add(streamReader.ReadToEnd());
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("There was a problem reading the memory stream.", Ex);
            }
            finally
            {
                if (streamReader != null)
                {
                    streamReader.Dispose();
                }
            }

            return streamLines.ToArray();
        }

        private string readStreamToText(Stream stream, Encoding encoding)
        {
            string[] streamLines = readStreamToArray(stream, encoding);

            if (streamLines.Length <= 0)
            {
                return "";
            }

            string streamText = "";
            try
            {
                for (int i = 0; i < streamLines.Length; i++)
                {
                    if (string.IsNullOrEmpty(streamLines[i]))
                    {
                        continue;
                    }
                    
                    streamText = streamText + streamLines[i] + Environment.NewLine;
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error reading stream text.", Ex);
            }

            return streamText;
        }

        private HTTPHeader[] httpHeaderCollection(WebHeaderCollection webHeaderCollection)
        {
            List<HTTPHeader> httpHeaderList = new List<HTTPHeader>();
            try
            {
                for (int i = 0; i < webHeaderCollection.Count; i++)
                {
                    httpHeaderList.Add(new HTTPHeader() { Key = webHeaderCollection.Keys[i], Value = webHeaderCollection[i] });
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error parsing header collection.", Ex);
            }

            return httpHeaderList.ToArray();
        }

        // This was initially created to just determine the size of a file. Might not be needed but leaving it here since I already wrote it.
        private long getHTTPRequestSize(string url)
        {
            HttpWebResponse httpWebSizeResponse = null;
            long payloadSize = -1;
            try
            {
                HttpWebRequest httpWebSizeRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebSizeRequest.ReadWriteTimeout = Timeout * 1000;
                httpWebSizeRequest.Method = WebRequestMethods.Http.Head;
                httpWebSizeResponse = (HttpWebResponse)httpWebSizeRequest.GetResponse();

                if (httpWebSizeResponse != null)
                {
                    payloadSize = Convert.ToInt64(httpWebSizeResponse.Headers.Get("Content-Length"));
                }
                else
                {
                    LogWriter.LogEntry("HTTP size request failure. Web request response is null. Url: " + url);
                }
            }
            catch (Exception Ex)
            {
                LogWriter.Exception("HTTP size request error. Url: " + url, Ex);
            }
            finally
            {
                if (httpWebSizeResponse != null)
                {
                    httpWebSizeResponse.Close();
                }
            }

            return payloadSize;
        }

        private void downloadAsyncComplete(HttpType httpType, object httpCompleteData)
        {
            // If the IsBusy is false, we just want to fizzle out. This means the Task was cancelled already.
            if (IsBusy == false)
            {
                return;
            }
            _IsBusy = false;

            if (httpCompleteData == null)
            {
                return;
            }

            switch (httpType)
            {
                case HttpType.DataRequest:
                    if (DataRequestComplete == null)
                    {
                        break;
                    }

                    HTTPResponse httpResponse = new HTTPResponse();
                    if (httpCompleteData is HTTPResponse)
                    {
                        httpResponse = httpCompleteData as HTTPResponse;
                    }

                    DataRequestComplete(this, new HTTPResponse()
                    {
                        Response = httpResponse.Response,
                        StatusCode = httpResponse.StatusCode,
                    });

                    break;

                case HttpType.ReadWebTextFile:
                    if (FileReadComplete == null)
                    {
                        break;
                    }

                    HTTPReadComplete httpReadComplete = new HTTPReadComplete() { Error = true, };
                    if (httpCompleteData is HTTPReadComplete)
                    {
                        httpReadComplete = httpCompleteData as HTTPReadComplete;
                    }

                    FileReadComplete(this, new HTTPReadComplete()
                    {
                        FileLines = httpReadComplete.FileLines.ToArray(),
                        Error = httpReadComplete.Error,
                    });

                    break;

                case HttpType.Download:
                    if (DownloadComplete == null)
                    {
                        break;
                    }

                    HTTPComplete httpComplete = new HTTPComplete() { Error = true, };
                    if (httpCompleteData is HTTPComplete)
                    {
                        httpComplete = httpCompleteData as HTTPComplete;
                    }

                    DownloadComplete(this, new HTTPComplete()
                    {
                        PayLoadSize = httpComplete.PayLoadSize,
                        PercentageComplete = httpComplete.PercentageComplete,
                        BytesReceived = httpComplete.BytesReceived,
                        Cancelled = httpComplete.Cancelled,
                        Error = httpComplete.Error,
                    });

                    break;
            }

            Dispose();
        }

        private HttpConnection cleanup(HttpConnection httpConnectionData)
        {
            if (httpConnectionData == null)
            {
                return new HttpConnection();
            }

            if (httpConnectionData.WebResponse != null)
            {
                httpConnectionData.WebResponse.Close();
                httpConnectionData.WebResponse = null;
            }

            if (httpConnectionData.ResponseStream != null)
            {
                httpConnectionData.ResponseStream.Dispose();
            }

            return httpConnectionData;
        }

        private void disposeTask(Task task)
        {
            if (task != null && task.IsCompleted)
            {
                task.Dispose();
            }
        }

        // Although this class method internally cleans up it is still good to expose this method just in case.
        public void Dispose()
        {
            try
            {
                if (httpTask != null && httpTask.IsCompleted)
                {
                    httpTask.Dispose();
                    httpTask = null;
                }

                if (source != null)
                {
                    source.Dispose();
                    source = null;
                }
            }
            catch (Exception Ex)
            {
                // The above may fail is the task is somehow dead locked. Will look into this more.
                LogWriter.Exception("Error with disposing resources.", Ex);
            }
        }
    }
}
