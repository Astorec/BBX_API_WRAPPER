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
    }
}
