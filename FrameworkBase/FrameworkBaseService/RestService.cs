using FrameworkBaseService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FrameworkBaseService
{
    public class RestService<T> : IRestService<T>
    {
        private readonly HttpClient _httpClient;
        private Uri BaseEndpoint { get; set; }

        public JsonSerializerSettings MicrosoftDateFormatSettings {
            get {
                return new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
                };
            }
        }

        public RestService()
        {
            //if (baseEndpoint == null)
            //{
            //    throw new ArgumentNullException("baseEndpoint");
            //}
            //BaseEndpoint = baseEndpoint;
            _httpClient = new HttpClient();
        }

        public void addCustomHeaders()
        {
            //Inject custom headers AdHoc
            _httpClient.DefaultRequestHeaders.Remove("userIP");
            _httpClient.DefaultRequestHeaders.Add("userIP", "1.1.1.1");
        }

        public HttpContent CreateHttpContent(T content)
        {
            var json = JsonConvert.SerializeObject(content, MicrosoftDateFormatSettings);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        public Uri CreateRequestUri(string relativePath, string queryString)
        {
           // var endpoint = new Uri(BaseEndpoint, relativePath);

            var endpoint = new Uri(relativePath);
            var uriBuilder = new UriBuilder(endpoint);
            uriBuilder.Query = queryString;
            return uriBuilder.Uri;
        }

        public async Task<T> DeleteAsync(Uri url, int id)
        {
            addCustomHeaders();
            Uri uri = new Uri(url + "/" + id);
            var response = await _httpClient.DeleteAsync(uri);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<IEnumerable<T>> GetAsync(Uri url)
        {
            addCustomHeaders();
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(data);
        }

        public async Task<T> GetAsync(Uri url, int id)
        {
            Uri uri = new Uri(url + "/" + id);
            addCustomHeaders();
            var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> PostAsync(Uri url, T model)
        {
            addCustomHeaders();
            var response = await _httpClient.PostAsync(url.ToString(), CreateHttpContent(model));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }

        public async Task<T> PutAsync(Uri url, T model, int id)
        {
            addCustomHeaders();
            Uri uri = new Uri(url + "/" + id);
            var response = await _httpClient.PutAsync(uri, CreateHttpContent(model));
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(data);
        }
    }
}
