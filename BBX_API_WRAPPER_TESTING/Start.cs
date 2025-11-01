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

            //await AddGetUpdateDeleteNewTournament();
            await RunAllEndpointTests();

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
            Console.WriteLine($"\n--------------------Get Player Ross---------------------\n");
            var player = await playerClient.GetPlayerByUsername("Ross");
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

        #region Player Client Tests

        public async Task TestPlayerCRUD()
        {
            Console.WriteLine("\n==================== PLAYER CRUD TESTS ====================\n");

            // Create new player
            Console.WriteLine("--------------------Adding new Player---------------------");
            var newPlayer = new Player
            {
                Name = "Test Player",
                Username = "testplayer123",
                Region = 1
            };

            await playerClient.AddNewPlayer(newPlayer);
            Console.WriteLine($"Added new player: {newPlayer.Name} ({newPlayer.Username})");

            // Get player by username
            Console.WriteLine("\n--------------------Get Player by Username---------------------");
            var retrievedPlayer = await playerClient.GetPlayerByUsername("testplayer123");
            Console.WriteLine($"Retrieved Player - ID: {retrievedPlayer.Id}, Name: {retrievedPlayer.Name}, Username: {retrievedPlayer.Username}");

            // Update player
            Console.WriteLine("\n--------------------Updating Player---------------------");
            retrievedPlayer.Name = "Updated Test Player";
            retrievedPlayer.Region = 2;
            await playerClient.UpdatePlayer(retrievedPlayer);
            Console.WriteLine($"Updated player name to: {retrievedPlayer.Name}");

            // Verify update
            var updatedPlayer = await playerClient.GetPlayerById(retrievedPlayer.Id);
            Console.WriteLine($"Verified Update - Name: {updatedPlayer.Name}, Region: {updatedPlayer.Region}");

            Console.WriteLine("\nPress Enter to delete the test player...");
            Console.ReadLine();

            // Delete player
            Console.WriteLine("--------------------Deleting Player---------------------");
            await playerClient.RemovePlayer(retrievedPlayer.Id);
            Console.WriteLine("Test player deleted.");
        }

        #endregion

        #region Tournament Participant Tests

        public async Task TestTournamentParticipants()
        {
            Console.WriteLine("\n==================== TOURNAMENT PARTICIPANT TESTS ====================\n");

            if (testDataTournament == null)
            {
                await GetAllTournaments();
            }

            Console.WriteLine($"Testing participants for tournament: {testDataTournament.Name} (ID: {testDataTournament.Id})");

            // Get all participants
            var participants = await tournamentClient.GetAllParticipants(testDataTournament.Id);
            Console.WriteLine($"\n--------------------Current Participants ({participants.Count()})---------------------");
            foreach (var p in participants.Take(5)) // Show first 5
            {
                Console.WriteLine($"ID: {p.Id}, Player DB ID: {p.PlayerDBId}, Player ID: {p.PlayerId}, Group ID: {p.GroupId}");
            }

            if (participants.Any())
            {
                // Get specific participant
                var firstParticipant = participants.First();
                Console.WriteLine($"\n--------------------Get Specific Participant by Player DB ID---------------------");
                var specificParticipant = await tournamentClient.GetParticipantInTournament(testDataTournament.Id, firstParticipant.PlayerDBId);
                Console.WriteLine($"Found participant - ID: {specificParticipant.Id}, Player DB ID: {specificParticipant.PlayerDBId}");

                // Update participant
                Console.WriteLine($"\n--------------------Update Participant---------------------");
                var originalGroupId = specificParticipant.GroupId;
                specificParticipant.GroupId = originalGroupId + 1000; // Change to unlikely group ID
                await tournamentClient.UpdateParticipant(testDataTournament.Id, specificParticipant);
                Console.WriteLine($"Updated participant group ID from {originalGroupId} to {specificParticipant.GroupId}");

                // Verify update
                var updatedParticipant = await tournamentClient.GetParticipantInTournament(testDataTournament.Id, specificParticipant.PlayerDBId);
                Console.WriteLine($"Verified update - Group ID: {updatedParticipant.GroupId}");

                Console.WriteLine("\nPress Enter to revert the participant update...");
                Console.ReadLine();

                // Revert update
                updatedParticipant.GroupId = originalGroupId;
                await tournamentClient.UpdateParticipant(testDataTournament.Id, updatedParticipant);
                Console.WriteLine($"Reverted participant group ID back to {originalGroupId}");
            }
        }

        public async Task TestAddRemoveParticipants()
        {
            Console.WriteLine("\n==================== ADD/REMOVE PARTICIPANT TESTS ====================\n");

            if (testDataTournament == null)
            {
                await GetAllTournaments();
            }

            // Get a random player to add as participant
            var allPlayers = await playerClient.GetAllPlayers();
            var randomPlayer = allPlayers.Skip(new Random().Next(allPlayers.Count())).First();

            Console.WriteLine($"--------------------Adding Participant to Tournament---------------------");
            var newParticipant = new Participant
            {
                PlayerDBId = randomPlayer.Id,
                TournamentId = testDataTournament.Id,
                PlayerId = 999999, // Test player ID
                GroupId = 888888 // Test group ID
            };

            await tournamentClient.AddParticipant(testDataTournament.Id, newParticipant);
            Console.WriteLine($"Added participant - Player: {randomPlayer.Name} to tournament: {testDataTournament.Name}");

            Console.WriteLine("\nPress Enter to remove the test participant...");
            Console.ReadLine();

            // Remove the participant
            Console.WriteLine($"--------------------Removing Participant---------------------");
            await tournamentClient.RemoveParticipantByTournamentAndPlayerId(testDataTournament.Id, randomPlayer.Id);
            Console.WriteLine("Test participant removed.");
        }

        #endregion

        #region Tournament Match Tests

        public async Task TestTournamentMatches()
        {
            Console.WriteLine("\n==================== TOURNAMENT MATCH TESTS ====================\n");

            if (testDataTournament == null)
            {
                await GetAllTournaments();
            }

            Console.WriteLine($"Testing matches for tournament: {testDataTournament.Name}");

            // Get all matches
            var matches = await tournamentClient.GetAllMatches(testDataTournament.Id);
            Console.WriteLine($"\n--------------------Current Matches ({matches.Count()})---------------------");
            foreach (var m in matches.Take(5)) // Show first 5
            {
                Console.WriteLine($"Match ID: {m.Id}, P1: {m.Player1Id}, P2: {m.Player2Id}, Winner: {m.WinnerId}");
            }

            if (matches.Any())
            {
                Console.WriteLine($"\n--------------------Update Match Results---------------------");
                var testMatches = matches.Take(2).ToList();

                // Store original values for restoration
                var originalData = testMatches.Select(m => new
                {
                    Id = m.Id,
                    OriginalWinner = m.WinnerId,
                    OriginalP1Score = m.Player1Score,
                    OriginalP2Score = m.Player2Score
                }).ToList();

                // Update match results
                foreach (var match in testMatches)
                {
                    match.WinnerId = match.Player1Id; // Set player 1 as winner
                    match.Player1Score = 3;
                    match.Player2Score = 1;
                }

                await tournamentClient.UpdateMatches(testDataTournament.Id, testMatches);
                Console.WriteLine($"Updated {testMatches.Count} match results");

                Console.WriteLine("\nPress Enter to revert match updates...");
                Console.ReadLine();

                // Revert changes
                for (int i = 0; i < testMatches.Count; i++)
                {
                    testMatches[i].WinnerId = originalData[i].OriginalWinner;
                    testMatches[i].Player1Score = originalData[i].OriginalP1Score;
                    testMatches[i].Player2Score = originalData[i].OriginalP2Score;
                }

                await tournamentClient.UpdateMatches(testDataTournament.Id, testMatches);
                Console.WriteLine("Match results reverted to original state");
            }
        }

        #endregion

        #region Tournament Data Tests

        public async Task TestTournamentData()
        {
            Console.WriteLine("\n==================== TOURNAMENT DATA TESTS ====================\n");

            if (testDataTournament == null)
            {
                await GetAllTournaments();
            }

            Console.WriteLine($"Testing tournament data for: {testDataTournament.Name}");

            // Get all tournament data
            var tournamentData = await tournamentClient.GetAllTournamentData(testDataTournament.Id);
            Console.WriteLine($"\n--------------------Current Tournament Data ({tournamentData.Count()})---------------------");

            Console.WriteLine($"{"Player DB ID",-12} {"Wins",-6} {"Losses",-8} {"Rank",-6} {"Win %",-8} {"Score",-8}");
            Console.WriteLine(new string('-', 50));

            foreach (var td in tournamentData.Take(10)) // Show first 10
            {
                Console.WriteLine($"{td.PlayerDBId,-12} {td.Wins,-6} {td.Losses,-8} {td.Rank,-6} {td.WinPercentage,-8:F1} {td.Score,-8}");
            }

            if (tournamentData.Any())
            {
                // Get tournament data for specific player
                var firstPlayerData = tournamentData.First();
                Console.WriteLine($"\n--------------------Get Data for Specific Player (ID: {firstPlayerData.PlayerDBId})---------------------");
                var playerTournamentData = await tournamentClient.GetTournamentDataForPlayer(testDataTournament.Id, firstPlayerData.PlayerDBId);
                Console.WriteLine($"Player Data - Wins: {playerTournamentData.Wins}, Losses: {playerTournamentData.Losses}, Rank: {playerTournamentData.Rank}");

                // Get all tournament data for this player across all tournaments
                var allPlayerData = await tournamentClient.GetTournamentDataForPlayer(firstPlayerData.PlayerDBId);
                Console.WriteLine($"\n--------------------All Tournament Data for Player {firstPlayerData.PlayerDBId}---------------------");
                foreach (var data in allPlayerData.Take(5))
                {
                    Console.WriteLine($"Tournament {data.TournamentId}: Wins: {data.Wins}, Losses: {data.Losses}, Rank: {data.Rank}");
                }
            }

            // Test adding new tournament data
            Console.WriteLine($"\n--------------------Add Test Tournament Data---------------------");
            // Get random player to associate with test data
            var participants = await tournamentClient.GetAllParticipants(testDataTournament.Id);
            var randomPlayerDbId = participants.ElementAt(new Random().Next(participants.Count())).PlayerDBId;

            var testData = new List<TournamentData>
            {
                new TournamentData
                {
                    TournamentId = testDataTournament.Id,
                    PlayerDBId = randomPlayerDbId, // Test player ID
                    Wins = 5,
                    Losses = 2,
                    Rank = 3,
                    WinPercentage = 71.4,
                    Score = 150
                }
            };

            await tournamentClient.AddTournamentData(testDataTournament.Id, testData);
            Console.WriteLine("Added test tournament data");

            Console.WriteLine("\nPress Enter to remove test tournament data...");
            Console.ReadLine();

            // Remove test data
            await tournamentClient.RemovePlayerTournamentData(testDataTournament.Id, randomPlayerDbId);
            Console.WriteLine("Test tournament data removed");
        }

        #endregion

        #region Comprehensive Test Runner

        public async Task RunAllEndpointTests()
        {
            Console.WriteLine("\n🚀 STARTING COMPREHENSIVE API ENDPOINT TESTS 🚀\n");

            try
            {
                // Tournament tests (already existing)
                await GetAllTournaments();
                await GetTournamentById(11);
                await GetTournamentByUrl("https://worldbeyblade.challonge.com/ij1pgt0i");

                // Player tests
                await GetAlllPlayer();
                await GetPlayerByUsername();
                await TestPlayerCRUD();

                // Leaderboard tests
                await GetLeaderboard();

                // Participant tests
                await GetParticipants();
                await TestTournamentParticipants();
                await TestAddRemoveParticipants();

                // Match tests
                await GetMatches();
                await TestTournamentMatches();

                // Tournament data tests
                await TestTournamentData();

                // Complex data analysis
                await GetMatchDataWithNames();

                // CRUD operations
                await AddGetUpdateDeleteNewTournament();

                Console.WriteLine("\n✅ ALL ENDPOINT TESTS COMPLETED SUCCESSFULLY! ✅");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ TEST FAILED: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        #endregion

    }
}
