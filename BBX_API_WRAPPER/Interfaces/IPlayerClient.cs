using BBX_API_WRAPPER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBX_API_WRAPPER.Interfaces
{
    internal interface IPlayerClient
    {
        /// <summary>
        /// GET players/
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Player>> GetAllPlayers();

        /// <summary>
        /// GET players/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Player> GetPlayerById(int id);

        /// <summary>
        /// GET players/u/{username}
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<Player> GetPlayerByUsername(string username);

        /// <summary>
        /// POST players/add
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task AddNewPlayer(Player player);

        /// <summary>
        /// DELETE players/delete/{playerId}
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task RemovePlayer(int playerId);

        /// <summary>
        /// PUT players/update/{playerId}
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        Task UpdatePlayer(Player player);
    }
}
