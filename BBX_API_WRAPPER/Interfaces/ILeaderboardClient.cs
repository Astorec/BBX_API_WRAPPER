
using BBX_API_WRAPPER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BBX_API_WRAPPER.Interfaces
{
    public interface ILeaderboardClient
    {
        Task<IEnumerable<Leaderboard>> GetMainBoard();
    }
}
