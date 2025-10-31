using BBX_API_WRAPPER.Interfaces;
using BBX_API_WRAPPER.Models;
using BBX_API_WRAPPER.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBX_API_WRAPPER.Clients
{
    public class LeaderboardClient : ILeaderboardClient
    {
        private HttpClient _client;

        // make this assignable from constructors
        private string _urlBase;
        private JsonTools _jsonTools;

        // existing ctor (preserves behavior when you pass an already-configured HttpClient)
        public LeaderboardClient(HttpClient client)
        {
            _client = client;
            // normalize: no trailing slash
            _urlBase = "http://localhost:3000/leaderboard";

            _jsonTools = new JsonTools(_client, _urlBase);
        }


        public async Task<IEnumerable<Leaderboard>> GetMainBoard()
        {
            try
            {
                return await _jsonTools.GetList<Leaderboard>();
            }
            catch (HttpRequestException ex)
            {
                // Common cause: using https when server speaks plain http (or bad cert). Give actionable hint.
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving leaderboard from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
        }
    }
}
