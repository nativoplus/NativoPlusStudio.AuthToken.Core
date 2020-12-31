using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NativoPlusStudio.AuthToken.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PostFormAsync(this HttpClient client, IList<KeyValuePair<string, string>> request, string endpoint = "")
        {
            var content = new FormUrlEncodedContent(request);
            var response = await client
                .PostAsync(endpoint, content);
            return response;
        }

        public static async Task<HttpResponseMessage> GetAsync(this HttpClient client, string endpoint = "", string query = "")
        {
            var response = await client
                .GetAsync($"{endpoint}{query}");

            return response;
        }

        public static async Task<HttpResponseMessage> PostAsync<TRequest>(this HttpClient client, TRequest request, string endpoint = "", string mediaType = "application/json") where TRequest : new()
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, mediaType);

            var response = await client
                .PostAsync(endpoint, content);

            return response;
        }

        public static async Task<HttpResponseMessage> PutAsync<TRequest>(this HttpClient client, TRequest request, string endpoint = "", string mediaType = "application/json") where TRequest : new()
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, mediaType);
            var response = await client
                .PutAsync(endpoint, content);
            return response;
        }
        public static async Task<HttpResponseMessage> DeleteAsync(this HttpClient client, string endpoint = "", string query = "")
        {
            var response = await client
                .DeleteAsync($"{endpoint}{query}");
            return response;
        }

        public static async Task<(TResponse Response, bool Status, string Code, string Message)> TransformHttpResponseToType<TResponse>(this HttpResponseMessage response) where TResponse: new()
        {
            var genericResponse = new TResponse();
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                genericResponse = JsonConvert.DeserializeObject<TResponse>(data);
            }
            return (genericResponse, response.IsSuccessStatusCode, response.StatusCode.ToString(), response.ReasonPhrase);
        }
    }
}
