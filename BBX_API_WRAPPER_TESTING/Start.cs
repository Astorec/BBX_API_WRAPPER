using BBX_API_WRAPPER.Clients;
using BBX_API_WRAPPER.Models;
using System.Text.Json;

namespace BBX_API_WRAPPER
{
    public class Start
    {
        HttpClientHandler handler = new();
        TournamentClient tournamentClient;
        PlayerClient playerClient;
        LeaderboardClient leaderboardClient;
        Tournament testDataTournament;
        public async Task StartTask()
        {
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
            (httpRequestMessage, cert, cetChain, policyErrors) =>
            {
                return true;
            };

            var client = new HttpClient(handler);
            tournamentClient = new TournamentClient(client);
            playerClient = new PlayerClient(client);
            leaderboardClient = new LeaderboardClient(client);


            var response = await client.GetAsync("http://localhost:3000/tournaments");

            Console.WriteLine(response.StatusCode);

            //await GetAllTournaments();

            //await GetTournamentById(11);

            //await GetTournamentByUrl("https://worldbeyblade.challonge.com/73kq22og");

            //await GetTournamentByUrl("https://challonge.com/frsasyrq");

            //await GetParticipants();
            //await GetMatches();
            //await GetAlllPlayer();
            //await GetPlayerByUsername();

            //await GetMatchDataWithNames();

            //await GetLeaderboard();

            await AddGetUpdateDeleteNewTournament();

        }


        public async Task GetAllTournaments()
        {
            var tournaments = await tournamentClient.GetAllTournaments();

            Console.WriteLine("\n--------------------Get All---------------------\n");

            foreach (var t in tournaments)
            {
                Console.WriteLine(t.Name);
            }

            // Get random tournament ID for testing
            var random = new Random();
            var tournamentList = new List<Tournament>(tournaments);
            if (tournamentList.Count > 0)
            {
                var randomIndex = random.Next(tournamentList.Count);
                testDataTournament = tournamentList[randomIndex];
            }
        }

        public async Task GetTournamentById(int id)
        {
            var tournament = await tournamentClient.GetTournamentById(id);
            bool isStoreChamp = tournament.IsStoreChampionship == 1 ? true : false;
            Console.WriteLine("\n--------------------Get By ID---------------------\n" +
                $"Name: {tournament.Name} Participants: {tournament.Participants} Is Store Champ: {isStoreChamp}");
        }

        public async Task GetTournamentByUrl(string url)
        {
            var tournament = await tournamentClient.GetTournamentByUrl(url);

            bool isStoreChamp = tournament.IsStoreChampionship == 1 ? true : false;

            Console.WriteLine($"\n--------------------Get By Url: {url} ---------------------\n" +
                $"Name: {tournament.Name} Participants: {tournament.Participants} Is Store Champ: {isStoreChamp}");

        }

        public async Task GetParticipants()
        {
            var participants = await tournamentClient.GetAllParticipants(testDataTournament.Id);
            Console.WriteLine($"\n--------------------Get Participants for tournament - {testDataTournament.Name} ---------------------\n");
            if (participants == null || participants.Count() == 0)
            {
                Console.WriteLine("No participants found.");
                return;
            }
            foreach (var p in participants)
            {
                Console.WriteLine($"Player DB ID: {p.PlayerDBId} Player ID: {p.PlayerId} Group ID: {p.GroupId}");
            }
        }

        public async Task GetMatches()
        {
            var matches = await tournamentClient.GetAllMatches(testDataTournament.Id);
            Console.WriteLine($"\n--------------------Get Matches for tournament - {testDataTournament.Name} ---------------------\n");
            if (matches == null || matches.Count() == 0)
            {
                Console.WriteLine("No matches found.");
                return;
            }

            foreach (var m in matches)
            {
                Console.WriteLine($"Match ID: {m.Id} Player 1 ID: {m.Player1Id} Player 2 ID: {m.Player2Id} Winner ID: {m.WinnerId}");
            }
        }

        public async Task GetAlllPlayer()
        {
            var players = await playerClient.GetAllPlayers();

            Console.WriteLine($"\n--------------------Get All Players---------------------\n");
            foreach (var p in players)
            {
                Console.WriteLine($"Player ID: {p.Id} Name: {p.Name} Username: {p.Username}");
            }
        }

        public async Task GetPlayerByUsername()
        {
            Console.WriteLine($"\n--------------------Get Player Astorec---------------------\n");
            var player = await playerClient.GetPlayerByUsername("Astorec");
            Console.WriteLine($"Player ID: {player.Id} Name: {player.Name} Username: {player.Username}");
        }

        public async Task GetMatchDataWithNames()
        {
            Console.WriteLine($"\n--------------------Get Match Data for {testDataTournament.Name} URL: {testDataTournament.Url}---------------------\n");

            var matches = await tournamentClient.GetAllMatches(testDataTournament.Id);
            var participants = await tournamentClient.GetAllParticipants(testDataTournament.Id);

            // Cache all player data in one call to avoid repeated GetPlayerById requests
            var allPlayers = await playerClient.GetAllPlayers();
            var playerLookup = allPlayers.ToDictionary(p => p.Id);

            Dictionary<int, Match> groupStageMatches = new();
            Dictionary<int, Match> finalStageMatches = new();

            foreach (var m in matches)
            {
                if (participants.Any(p => p.GroupId == m.Player1Id))
                    groupStageMatches[m.Id] = m;
                else if (participants.Any(p => p.PlayerId == m.Player1Id))
                    finalStageMatches[m.Id] = m;
            }

            Console.WriteLine($"\n--------------------Group Stage---------------------\n");
            foreach (var gMatch in groupStageMatches)
            {
                var p1 = participants.FirstOrDefault(p => p.GroupId == gMatch.Value.Player1Id);
                var p2 = participants.FirstOrDefault(p => p.GroupId == gMatch.Value.Player2Id);

                var player1Name = p1 != null && playerLookup.TryGetValue(p1.PlayerDBId, out var player1)
                    ? !string.IsNullOrWhiteSpace(player1.Username) ? player1.Username : player1.Name
                    : "Unknown";
                var player2Name = p2 != null && playerLookup.TryGetValue(p2.PlayerDBId, out var player2)
                    ? !string.IsNullOrWhiteSpace(player2.Username) ? player2.Username : player2.Name
                    : "Unknown";

                Console.WriteLine($"[Group Stage] Match ID: {gMatch.Value.Id} Player 1: {player1Name} vs Player 2: {player2Name} Winner ID: {gMatch.Value.WinnerId}");
            }

            Console.WriteLine($"\n--------------------Finals Stage---------------------\n");
            foreach (var fMatch in finalStageMatches)
            {
                var p1 = participants.FirstOrDefault(p => p.PlayerId == fMatch.Value.Player1Id);
                var p2 = participants.FirstOrDefault(p => p.PlayerId == fMatch.Value.Player2Id);

                var player1Name = p1 != null && playerLookup.TryGetValue(p1.PlayerDBId, out var player1)
                    ? !string.IsNullOrWhiteSpace(player1.Username) ? player1.Username : player1.Name
                    : "Unknown";
                var player2Name = p2 != null && playerLookup.TryGetValue(p2.PlayerDBId, out var player2)
                    ? !string.IsNullOrWhiteSpace(player2.Username) ? player2.Username : player2.Name
                    : "Unknown";

                Console.WriteLine($"[Final Stage] Match ID: {fMatch.Value.Id} Player 1: {player1Name} vs Player 2: {player2Name} Winner ID: {fMatch.Value.WinnerId}");
            }
        }

        public async Task GetLeaderboard()
        {
            Console.WriteLine($"\n--------------------Main Board---------------------\n");
            var leaderboard = await leaderboardClient.GetMainBoard();

            // Print table header
            Console.WriteLine(
                $"{"Rank",-6} {"Name",-20} {"Score",-8} {"Win %",-8} {"Region",-10}");

            Console.WriteLine(new string('-', 60));

            // Print each row
            foreach (var lb in leaderboard)
            {
                Console.WriteLine(
                    $"{lb.PlayerRank,-6} {lb.DisplayName,-20} {lb.TotalScore,-8} {lb.TotalWinPercentage,-8} {lb.Region,-10}");
            }
        }

        public async Task AddGetUpdateDeleteNewTournament()
        {
            Console.WriteLine($"\n--------------------Adding new Tournament---------------------\n");

            var newTournament = new Tournament
            {
                Name = "Test Tournament",
                Url = "test_tournament",
                Participants = 16,
                IsSideEvent = 0,
                Region = 1,
                IsStoreChampionship = 1,
                AttendanceId = 3,
                Finalized = 0,
                State = "pending"
            };

            await tournamentClient.CreateNewTournament(newTournament);
            Console.WriteLine("Added new tournament.");

            Console.WriteLine($"\n--------------------Get new Tournament Details---------------------\n");

            var tournamnet = await tournamentClient.GetTournamentByUrl("test_tournament");

            Console.WriteLine($"Name: {tournamnet.Name} Participants: {tournamnet.Participants} Is Store Champ: {tournamnet.IsStoreChampionship}");

            Console.WriteLine($"\n--------------------Check DB that it was added correctly then Press Enter to continue---------------------\n");
            Console.ReadLine();


            Console.WriteLine($"\n--------------------Updating Tournament---------------------\n");
            tournamnet.Participants = 32;
            await tournamentClient.UpdateTournament(tournamnet.Id, tournamnet);

            Console.WriteLine($"\n--------------------Get updated Tournament Details---------------------\n");
            var updatedTournamnet = await tournamentClient.GetTournamentByUrl("test_tournament");
            Console.WriteLine($"Name: {updatedTournamnet.Name} Participants: {updatedTournamnet.Participants} Is Store Champ: {updatedTournamnet.IsStoreChampionship}");
            Console.WriteLine($"\n--------------------Check DB that it was updated correctly then Press Enter to continue---------------------\n");
            Console.ReadLine();

            Console.WriteLine($"\n--------------------Deleting new Tournament---------------------\n");
            await tournamentClient.DeleteTournament(tournamnet.Id);


            Console.WriteLine($"\n--------------------Check DB that it was removed then Press Enter to continue---------------------\n");
            Console.ReadLine();
        }
    }
}
