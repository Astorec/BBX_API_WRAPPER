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

        /// <summary>
        /// DELETE tournaments/{tournamentId}
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task DeleteTournament(int tournamentId);

        Task UpdateTournament(int tournamentId, Tournament tournament);

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

        /// <summary>
        /// POST tournaments/{tournamentId}/participants/add
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="participants"></param>
        /// <returns></returns>
        Task AddParticipants(int tournamentId, IEnumerable<Participant> participants);

        /// <summary>
        /// PUT tournaments/{tournamentId}/participants/update
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        Task UpdateParticipant(int tournamentId, Participant participant);


        /// <summary>
        /// DELETE tournaments/{tournamentId}/participants/{playerDbId}
        /// Remove a participant from a specific tournament
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task RemoveParticipantByTournamentAndPlayerId(int tournamentId, int playerDbId);

        /// <summary>
        /// DELETE tournaments/participants/{playerDbId}
        /// Removes all entries of a participant across all tournaments
        /// </summary>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task RemoveParticipantAllEntries(int playerDbId);

        /// <summary>
        /// DELETE tournaments/{tournamentId}/participants
        /// Removes all participants from a tournament
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task RemoveParticipantsFromTournament(int tournamentId);
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

        /// <summary>
        /// POST tournaments/{tournamentId}/matches
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="matches"></param>
        /// <returns></returns>
        Task AddNewMatches(int tournamentId, IEnumerable<Match> matches);

        /// <summary>
        /// DELETE tournaments/{tournamentId}/matches
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task DeleteMatches(int tournamentId);

        #endregion

        /// <summary>
        /// GET tournaments/{tournamentId}/data
        /// We send in the tournament ID so that we don't get everything in the system
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task<IEnumerable<TournamentData>> GetAllTournamentData(int tournamentId);

        /// <summary>
        /// GET tournaments/data/player/{playerDbId}
        /// </summary>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task<IEnumerable<TournamentData>> GetTournamentDataForPlayer(int playerDbId);

        /// <summary>
        /// GET tournaments/{tournamentId}/data/player/{playerDbId}
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task<TournamentData> GetTournamentDataForPlayer(int tournamentId, int playerDbId);

        /// <summary>
        /// POST tournaments/{tournamentId}/data/add
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="tournamentData"></param>
        /// <returns></returns>
        Task AddTournamentData(int tournamentId, IEnumerable<TournamentData> tournamentData);

        /// <summary>
        /// PUT tournaments/{tournamentId}/data/update
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="tournamentData"></param>
        /// <returns></returns>
        Task UpdateTournamentData(int tournamentId, IEnumerable<TournamentData> tournamentData);

        /// <summary>
        /// DELETE tournaments/{tournamentId}/data
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        Task RemoveAllTournamentData(int tournamentId);

        /// <summary>
        /// DELETE tournaments/{tournamentId}/data/player/{playerDbId}
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="playerDbId"></param>
        /// <returns></returns>
        Task RemovePlayerTournamentData(int tournamentId, int playerDbId);

        #region TournamentData Details

        #endregion
    }
}