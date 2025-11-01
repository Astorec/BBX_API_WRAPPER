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

        public async Task CreateNewTournament(Tournament tournament)
        {
            try
            {
                await _jsonTools.PostItem<Tournament>("/create", tournament);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/create'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error creating tournament at '{_urlBase}/create'. Inner: {ex.Message}", ex);
            }
        }

        public async Task DeleteTournament(int tournamentId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/{tournamentId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting tournament id '{tournamentId}' from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task UpdateTournament(int tournamentId, Tournament tournament)
        {
            try
            {
                await _jsonTools.PutItem<Tournament>($"/{tournamentId}", tournament);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error updating tournament id '{tournamentId}' from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
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
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving participants from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<Participant> GetParticipantByPlayerDbId(int playerDbId)
        {
            try
            {
                return await _jsonTools.GetItem<Participant>($"/participants/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/participants/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving participant with playerDbId '{playerDbId}' at '{_urlBase}/participants/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<Participant> GetParticipantInTournament(int  tournamentId, int playerId)
        {
            try
            {
                return await _jsonTools.GetItem<Participant>($"/{tournamentId}/participants/player/{playerId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants/id/{playerId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving participant with playerId '{playerId}' from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants/id/{playerId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task AddParticipant(int tournamentId, Participant participant)
        {
            try
            {
                await _jsonTools.PostItem<Participant>($"/{tournamentId}/participants/add", participant);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants/add'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adding participant to tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants/add'. Inner: {ex.Message}", ex);
            }
        }

        public async Task AddParticipants(int tournamentId, IEnumerable<Participant> participants)
        {
            try
            {
                await _jsonTools.PostList<Participant>($"/{tournamentId}/participants/add", participants);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants/add'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adding participants to tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants/add'. Inner: {ex.Message}", ex);
            }
        }

        public async Task UpdateParticipant(int tournamentId, Participant participant)
        {
            try
            {
                await _jsonTools.PutItem<Participant>($"/{tournamentId}/participants/{participant.Id}", participant);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants/{participant.Id}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error updating participant id '{participant.Id}' in tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants/{participant.Id}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemoveParticipantByTournamentAndPlayerId(int tournamentId, int playerDbId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/{tournamentId}/participants/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting participant with playerDbId '{playerDbId}' from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemoveParticipantAllEntries(int playerDbId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/participants/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/participants/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting participant with playerDbId '{playerDbId}' from all tournaments at '{_urlBase}/participants/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemoveParticipantsFromTournament(int tournamentId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/{tournamentId}/participants");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/participants'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting participants from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/participants'. Inner: {ex.Message}", ex);
            }
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
            try
            {
                return _jsonTools.PutList<Match>($"/{tournamentId}/matches", matches);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/matches'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error updating matches in tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/matches'. Inner: {ex.Message}", ex);
            }
        }



        public Task AddNewMatches(int tournamentId, IEnumerable<Match> matches)
        {
            try
            {
                return _jsonTools.PostList<Match>($"/{tournamentId}/matches", matches);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/matches'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adding matches to tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/matches'. Inner: {ex.Message}", ex);
            }
        }

        public Task DeleteMatches(int tournamentId)
        {
            try
            {
                return _jsonTools.DeleteItem($"/{tournamentId}/matches");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/matches'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting matches from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/matches'. Inner: {ex.Message}", ex);
            }
        }

        #endregion

        #region TournamentData Details


        public async Task<IEnumerable<TournamentData>> GetAllTournamentData(int tournamentId)
        {
            try
            {
                return await _jsonTools.GetList<TournamentData>($"/{tournamentId}/data");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving tournament data from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<TournamentData>> GetTournamentDataForPlayer(int playerDbId)
        {
            try
            {
                return await _jsonTools.GetList<TournamentData>($"/data/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/data/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving tournament data for playerDbId '{playerDbId}' at '{_urlBase}/data/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task<TournamentData> GetTournamentDataForPlayer(int tournamentId, int playerDbId)
        {
            try
            {
                return await _jsonTools.GetItem<TournamentData>($"/{tournamentId}/data/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving tournament data for playerDbId '{playerDbId}' from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task AddTournamentData(int tournamentId, IEnumerable<TournamentData> tournamentData)
        {
            try
            {
                await _jsonTools.PostList<TournamentData>($"/{tournamentId}/data/add", tournamentData);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data/add'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adding tournament data to tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data/add'. Inner: {ex.Message}", ex);
            }
        }

        public async Task UpdateTournamentData(int tournamentId, IEnumerable<TournamentData> tournamentData)
        {
            try
            {
                await _jsonTools.PutList<TournamentData>($"/{tournamentId}/data/update", tournamentData);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data/update'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error updating tournament data in tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data/update'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemoveAllTournamentData(int tournamentId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/{tournamentId}/data");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting tournament data from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemovePlayerTournamentData(int tournamentId, int playerDbId)
        {
            try
            {
                await _jsonTools.DeleteItem($"/{tournamentId}/data/player/{playerDbId}");
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException(
                    $"Failed connecting to '{_urlBase}/{tournamentId}/data/player/{playerDbId}'. If this is a local dev server try using 'http://' or pass a constructor with ignoreSsl=true for testing. Inner: {ex.Message}",
                    ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting tournament data for playerDbId '{playerDbId}' from tournament id '{tournamentId}' at '{_urlBase}/{tournamentId}/data/player/{playerDbId}'. Inner: {ex.Message}", ex);
            }

            #endregion
        }
    }
}