using System.Text.Json;
using System.Net.Http;
using System;
using System.Linq;
using BBX_API_WRAPPER.Interfaces;
using BBX_API_WRAPPER.Models;
using BBX_API_WRAPPER.Utils;

namespace BBX_API_WRAPPER.Clients
{
    public class TournamentClient : ITournamentClient
    {
        private HttpClient _client;

        // make this assignable from constructors
        private string _urlBase;
        private JsonTools _jsonTools;

        // existing ctor (preserves behavior when you pass an already-configured HttpClient)
        public TournamentClient(HttpClient client)
        {
            _client = client;
            // normalize: no trailing slash
            _urlBase = "http://localhost:3000/tournaments";

            _jsonTools = new JsonTools(_client, _urlBase);
        }

        #region Tournament Details
        public async Task<IEnumerable<Tournament>> GetAllTournaments()
        {
            try
            {
               return await _jsonTools.GetList<Tournament>();
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
                throw new Exception($"Unexpected error retrieving tournaments from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<Tournament> GetTournamentById(int id)
        {
            try
            {
              return await _jsonTools.GetItem<Tournament>($"/{id}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{id}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving tournament id '{id}' from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<Tournament> GetTournamentByUrl(string url)
        {
            // URL example: https://challonge.com/rjzy5wdh or https://worldbeyblade.challonge.com/fx3zbztv
            // Extract the last segment after the last slash
            // If we have worldbeyblade.challonge.com, we need to extract worldbeyblade as the subdomain
            // then the string we would have is worldbeyblade-fx3zbztv

            var urlSplit = url.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var lastSegment = urlSplit.Last();

            // Check for subdomain
            if (urlSplit.Length >= 3 && urlSplit[urlSplit.Length - 2].Contains('.'))
            {
                var domainParts = urlSplit[urlSplit.Length - 2].Split('.', StringSplitOptions.RemoveEmptyEntries);
                if (domainParts.Length > 2)
                {
                    var subdomain = domainParts[0];
                    lastSegment = $"{subdomain}-{lastSegment}";
                }
            }
            
            return await _jsonTools.GetItem<Tournament>($"/url/{lastSegment}");
        }

        public Task CreateNewTournament(Tournament tournament)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Participant Details
        public async Task<IEnumerable<Participant>> GetAllParticipants(int tournamentId)
        {
            try
            {
                return await _jsonTools.GetList<Participant>($"/{tournamentId}/participants");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
        }

        public  Task<Participant> GetParticipantByPlayerDbId(int tournamentId, int playerDbId)
        {
            throw new NotImplementedException();
        }

        public Task AddParticipant(int tournamentId, Participant participant)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Match Details
        public async Task<IEnumerable<Match>> GetAllMatches(int tournamentId)
        {
            try
            {
                return await _jsonTools.GetList<Match>($"/{tournamentId}/matches");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
        }

        public Task UpdateMatches(int tournamentId, IEnumerable<Match> matches)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}