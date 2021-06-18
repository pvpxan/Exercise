using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using StreamlineMVVM;

namespace HTTPHandler
{
    public partial class HTTP : IDisposable
    {
        public HTTPResponse DataRequest(HTTPRequest httpRequest)
        {
            HttpConnection httpConnectionData = new HttpConnection();
            httpConnectionData = processHTTPRequest(httpRequest);
            HTTPResponse httpResponse = new HTTPResponse() { StatusCode = httpConnectionData.StatusCode, };

            if (httpConnectionData.ResponseStream != null)
            {
                string characterSet = httpConnectionData.WebResponse.CharacterSet;
                Encoding encoding = Encoding.GetEncoding(characterSet);
                string[] responseData = readStreamToArray(httpConnectionData.ResponseStream, encoding);
                if (responseData.Length > 0)
                {
                    httpResponse.Response = responseData[0];
                }

                httpConnectionData.ResponseStream.Dispose();
            }

            cleanup(httpConnectionData);

            return httpResponse;
        }

        public void DataRequestAsync(HTTPRequest httpRequest)
        {
            if (_IsBusy)
            {
                return;
            }

            TaskScheduler currentSynchronizationContext = TaskScheduler.FromCurrentSynchronizationContext();
            source = new CancellationTokenSource();
            token = source.Token;
            HTTPResponse httpResponse = new HTTPResponse();
            _IsBusy = true;
            try
            {
                httpTask = Task.Factory.
                    StartNew(() => httpResponse = DataRequest(httpRequest), token, TaskCreationOptions.None, TaskScheduler.Default).
                    ContinueWith((t) => downloadAsyncComplete(HttpType.DataRequest, httpResponse), currentSynchronizationContext);

            }
            catch (Exception Ex)
            {
                LogWriter.Exception("Error running download worker task.", Ex);
            }
        }
    }
}
