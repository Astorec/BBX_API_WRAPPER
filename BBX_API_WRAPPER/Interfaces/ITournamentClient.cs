using BBX_API_WRAPPER.Models;

namespace BBX_API_WRAPPER.Interfaces
{
    public interface ITournamentClient
    {
        #region Tournament Details
        /// <summary>
        /// GET tournaments/
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Tournament>> GetAllTournaments();

        /// <summary>
        /// GET tournaments/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Tournament> GetTournamentById(int id);

        /// <summary>
        /// GET tournaments/url/{url}
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Tournament> GetTournamentByUrl(string url);

        /// <summary>
        /// POST tournaments/create
        /// </summary>
        /// <param name="tournament"></param>
        /// <returns></returns>
        Task CreateNewTournament(Tournament tournament);
        #endregion

        #region Participant Details

        /// <summary>
        /// GET tournaments/{tournamentId}/participants
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task<IEnumerable<Participant>> GetAllParticipants(int tournamentId);

        /// <summary>
        /// GET tournaments/{tournamentId}/participants/{playerDbId}
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task<Participant> GetParticipantByPlayerDbId(int tournamentId, int playerDbId);

        /// <summary>
        /// POST tournaments/{tournamentId}/participants/add
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        Task AddParticipant (int tournamentId, Participant participant);
        #endregion

        #region Match Details 

        /// <summary>
        /// GET tournaments/{tournamentId}/matches
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task<IEnumerable<Match>> GetAllMatches(int tournamentId);

        /// <summary>
        /// POST tournaments/{tournamentId}/matches/update
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        Task UpdateMatches(int tournamentId, IEnumerable<Match> matches);

        #endregion
    }
}