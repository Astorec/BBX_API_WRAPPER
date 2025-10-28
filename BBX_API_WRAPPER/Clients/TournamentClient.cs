using System.Text.Json;
using System.Net.Http;
using System;
using System.Linq;
using BBX_API_WRAPPER.Interfaces;
using BBX_API_WRAPPER.Models;

namespace BBX_API_WRAPPER.Clients
{
    public class TournamentClient : ITournamentClient
    {
        private HttpClient _client;

        // make this assignable from constructors
        private string _urlBase;

        // Add parameterless ctor for local dev that ignores SSL by default
        public TournamentClient() : this(ignoreSslForLocalhost: true)
        {
        }

        // existing ctor (preserves behavior when you pass an already-configured HttpClient)
        public TournamentClient(HttpClient client)
        {
            _client = client;
            // normalize: no trailing slash
            _urlBase = "http://localhost:3000/tournaments";


        }

        // new ctor: creates an HttpClient that (optionally) ignores SSL errors for local development
        // Usage: new TournamentClient(ignoreSslForLocalhost: true)
        public TournamentClient(bool ignoreSslForLocalhost)
        {
            var handler = new HttpClientHandler();

            if (ignoreSslForLocalhost)
            {
                // Accept any server certificate (insecure—use only for local dev)
                handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            }

            _client = new HttpClient(handler);
            _urlBase = "http://localhost:3000/tournaments";
        }

        // new ctor: allow explicit base URL and optionally ignore SSL (or use http)
        public TournamentClient(string baseUrl, bool ignoreSsl = false)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("baseUrl must be provided", nameof(baseUrl));


            // plain HTTP; handler not needed
            _client = new HttpClient();
        }

        #region Tournament Details
        public async Task<IEnumerable<Tournament>> GetAllTournaments()
        {
            try
            {
                var response = await _client.GetAsync(_urlBase);

                response.EnsureSuccessStatusCode();

                var jsonContent = await response.Content.ReadAsStringAsync();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tournaments = JsonSerializer.Deserialize<IEnumerable<Tournament>>(jsonContent, options);

                return tournaments ?? Enumerable.Empty<Tournament>();
            }
            catch (HttpRequestException ex)
            {
                // Common cause: using https when server speaks plain http (or bad cert). Give actionable hint.
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
        }

        public async Task<Tournament> GetTournamentById(int id)
        {
            try
            {
                var response = await _client.GetAsync($"{_urlBase}/{id}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var tournament = JsonSerializer.Deserialize<Tournament>(json, options);

                return tournament!;
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{id}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
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

            var response = await _client.GetAsync($"{_urlBase}/url/{lastSegment}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var tournament = JsonSerializer.Deserialize<Tournament>(json, options);
            return tournament!;
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
                var resposne = await _client.GetAsync($"{_urlBase}/{tournamentId}/participants");

                resposne.EnsureSuccessStatusCode();

                var json = await resposne.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var participants = JsonSerializer.Deserialize<IEnumerable<Participant>>(json, options);

                return participants!;
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
                var resposne = await _client.GetAsync($"{_urlBase}/{tournamentId}/matches");

                resposne.EnsureSuccessStatusCode();

                var json = await resposne.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var matches = JsonSerializer.Deserialize<IEnumerable<Match>>(json, options);

                return matches!;
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