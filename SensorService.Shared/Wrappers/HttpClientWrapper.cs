using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SensorService.Shared.Wrappers
{
    public interface IHttpClientFactory
    {
        IHttpClientWrapper Create();
        IHttpClientWrapper Create(Uri baseAddress);
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly IHttpClientWrapper _clientWrapper;

        public HttpClientFactory(IHttpClientWrapper clientWrapper)
        {
            _clientWrapper = clientWrapper;
        }

        public IHttpClientWrapper Create()
        {
            return _clientWrapper;
        }

        public IHttpClientWrapper Create(Uri baseAddress)
        {
            _clientWrapper.BaseAddress = baseAddress;
            return _clientWrapper;
        }

    }
    public class HttpClientWrapper : IHttpClientWrapper, IDisposable
    {
        private readonly HttpClient _httpClient;
        public HttpClientWrapper()
        {
            _httpClient = new HttpClient();
        }

        public Uri BaseAddress
        {
            get => _httpClient.BaseAddress;
            set => _httpClient.BaseAddress = value;
        }

        public TimeSpan TimeOut
        {
            get => _httpClient.Timeout;
            set => _httpClient.Timeout = value;
        }

        public HttpRequestHeaders DefaultRequestHeaders => _httpClient.DefaultRequestHeaders;

        public Task<HttpResponseMessage> GetAsync(Uri requestUri)
        {
            return _httpClient.GetAsync(requestUri);
        }

        public Task<HttpResponseMessage> GetAsync(string requestString)
        {
            return _httpClient.GetAsync(requestString);
        }

        public Task<HttpResponseMessage> PostAsync(Uri requestUri, HttpContent content)
        {
            return _httpClient.PostAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> PostAsync(string requestString, HttpContent content)
        {
            return _httpClient.PostAsync(requestString, content);
        }

        public Task<HttpResponseMessage> PutAsync(Uri requestUri, HttpContent content)
        {
            return _httpClient.PutAsync(requestUri, content);
        }

        public Task<HttpResponseMessage> PutAsync(string requestString, HttpContent content)
        {
            return _httpClient.PutAsync(requestString, content);
        }

        public Task<HttpResponseMessage> DeleteAsync(Uri requestUri)
        {
            return _httpClient.DeleteAsync(requestUri);
        }

        public Task<HttpResponseMessage> DeleteAsync(string requestString)
        {
            return _httpClient.DeleteAsync(requestString);
        }


        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}
