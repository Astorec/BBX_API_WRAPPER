using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BBX_API_WRAPPER.Utils
{
    public class JsonTools
    {
        private HttpClient _client;
        private string _urlBase;
        public JsonTools(HttpClient client, string urlBase)
        {
            _client = client;
            _urlBase = urlBase;
        }

        /// <summary>
        /// GET response from endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        private async Task<HttpResponseMessage> GetResponse(string endpoint)
        {
            var fullUrl = _urlBase;

            if (!string.IsNullOrEmpty(endpoint))
            {
                fullUrl = $"{_urlBase}{endpoint}";
            }

            try
            {
                var response = await _client.GetAsync(fullUrl);

                response.EnsureSuccessStatusCode();
                return response;

            }
            catch (HttpRequestException ex)
            {
                // Common cause: using https when server speaks plain http (or bad cert). Give actionable hint.
                throw new HttpRequestException(
                    $"Failed connecting to '{fullUrl}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// GET list from endpoint
        /// </summary>
        /// <param name="endpoint"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task<IEnumerable<T>> GetList<T>(string enpoint = "")
        {
            try
            {
                var jsonContent = await GetResponse(enpoint).Result.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var items = JsonSerializer.Deserialize<IEnumerable<T>>(jsonContent, options);

                return items ?? Enumerable.Empty<T>();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed retrieving list from '{_urlBase}'. Inner: {ex.Message}",
                    ex);
            }
        }


        /// <summary>
        /// GET item from endpoint
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"> Example /{id}/participants</param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException"></exception>
        public async Task <T>GetItem<T>(string endpoint)
        {
            try
            {
                var jsonContent = await GetResponse(endpoint).Result.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var item = JsonSerializer.Deserialize<T>(jsonContent, options);
                return item!;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed retrieving item from '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }
   
        public async Task <T>PostItem<T>(string endpoint, T item)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_urlBase}{endpoint}", jsonContent);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var createdItem = JsonSerializer.Deserialize<T>(responseContent, options);
                return createdItem!;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed posting item to '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }

        public async Task PostList<T>(string endpoint, IEnumerable<T> items)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(items), Encoding.UTF8, "application/json");
                var response = await _client.PostAsync($"{_urlBase}{endpoint}", jsonContent);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed posting list to '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }

        public async Task DeleteItem(string endpoint)
        {
            try
            {
                var response = await _client.DeleteAsync($"{_urlBase}{endpoint}");
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed deleting item at '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }

        public async Task PutItem<T>(string endpoint, T item)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{_urlBase}{endpoint}", jsonContent);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed putting item to '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }

        public async Task PutList<T>(string endpoint, IEnumerable<T> items)
        {
            try
            {
                var jsonContent = new StringContent(JsonSerializer.Serialize(items), Encoding.UTF8, "application/json");
                var response = await _client.PutAsync($"{_urlBase}{endpoint}", jsonContent);
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed putting list to '{_urlBase}/{endpoint}'. Inner: {ex.Message}",
                    ex);
            }
        }
    }
}
