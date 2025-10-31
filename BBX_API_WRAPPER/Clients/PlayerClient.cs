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
    public class PlayerClient : IPlayerClient
    {
        private HttpClient _client;
        private string _urlBase;
        private JsonTools _jsonTools;

        public PlayerClient(HttpClient client)
        {
            _client = client;
            _urlBase = "http://localhost:3000/players";
            _jsonTools = new JsonTools(_client, _urlBase);
        }

        public Task AddNewPlayer(Player player)
        {
            try
            {
                return _jsonTools.PostItem<Player>("/add", player);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error adding new player: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error adding player to '{_urlBase}/add'. Inner: {ex.Message}", ex);
            }
        }

        public async Task RemovePlayer(int playerId)
        {
            try
            {
               await _jsonTools.DeleteItem($"/delete/{playerId}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error removing player with ID {playerId}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error deleting player from '{_urlBase}/delete/{playerId}'. Inner: {ex.Message}", ex);
            }
        }

        public async Task UpdatePlayer(Player player)
        {
            try
            {
                await _jsonTools.PutItem($"/update/{player.Id}", player);
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error updating player with ID {player.Id}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error updating player at '{_urlBase}/update/{player.Id}'. Inner: {ex.Message}", ex);
            }
        }

        public Task<IEnumerable<Player>> GetAllPlayers()
        {
            try
            {
                return _jsonTools.GetList<Player>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching all players: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving players from '{_urlBase}'. Inner: {ex.Message}", ex);
            }
        }

        public Task<Player> GetPlayerById(int id)
        {
            try
            {
                return _jsonTools.GetItem<Player>($"/{id}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching player by ID {id}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving player from '{_urlBase}/{id}'. Inner: {ex.Message}", ex);
            }
        }

        public Task<Player> GetPlayerByUsername(string username)
        {
            try
            {
                return _jsonTools.GetItem<Player>($"/u/{username}");
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Error fetching player by username '{username}': {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error retrieving player from '{_urlBase}/u/{username}'. Inner: {ex.Message}", ex);
            }
        }
    }
}
