using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using IBM.Webclient;

namespace IBM.SocketIO.Tests.Mocks
{
    public class MockHttpResponse
    {
        public string DataToReturn { get; set; }
        public HttpStatusCode Code { get; set; }
        public Dictionary<string, string> Headers { get; set; }
    }

    public class MockHttpClient : IHttpClient
    {
        #region Private Members

        private Dictionary<string, string> headers = new Dictionary<string, string>();
        private ConcurrentQueue<MockHttpResponse> mockedResponses = new ConcurrentQueue<MockHttpResponse>();

        #endregion

        public MockHttpClient(string dataToReturn, HttpStatusCode code)
            : this(new MockHttpResponse[] { new MockHttpResponse { DataToReturn = dataToReturn, Code = code } })
        { }

        public MockHttpClient(IEnumerable<MockHttpResponse> sequencedResponses)
        {
            mockedResponses = new ConcurrentQueue<MockHttpResponse>(sequencedResponses);
        }

        public void Dispose()
        {
        }

        public void AddHeader(string name, string value)
        {
            this.headers.Add(name, value);
        }

        public virtual Task<HttpResponseMessage> SendAsync(HttpRequestMessage message, CancellationToken token)
        {
            if (!mockedResponses.TryDequeue(out MockHttpResponse mockResponse))
            {
                throw new Exception("Mock Exception - too many calls");
            }

            var responseMessage = new HttpResponseMessage(mockResponse.Code);
            foreach(var header in this.headers)
            {
                responseMessage.Headers.Add(header.Key, header.Value);
            }
            if (mockResponse.Headers != null)
            {
                foreach (var header in mockResponse.Headers)
                {
                    responseMessage.Headers.Add(header.Key, header.Value);
                }
            }

            responseMessage.Content = new StringContent(mockResponse.DataToReturn);

            return Task.FromResult(responseMessage);
        }
    }
}
