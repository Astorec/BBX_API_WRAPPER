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


            var response = await client.GetAsync("http://localhost:3000/tournaments");

            Console.WriteLine(response.StatusCode);
         
            await GetAllTournaments();

            await GetTournamentById(11);

            await GetTournamentByUrl("https://worldbeyblade.challonge.com/73kq22og");

            await GetTournamentByUrl("https://challonge.com/frsasyrq");

            await GetParticipants();
            await GetMatches();
            await GetAlllPlayer();
            await GetPlayerByUsername();
        }


        public async Task GetAllTournaments()
        {
            var tournaments = await tournamentClient.GetAllTournaments();

            Console.WriteLine("\n--------------------Get All---------------------\n");

            foreach (var t in tournaments)
            {
                Console.WriteLine(t.name);
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
            bool isStoreChamp = tournament.is_store_championship == 1 ? true : false;
            Console.WriteLine("\n--------------------Get By ID---------------------\n" +
                $"Name: {tournament.name} Participants: {tournament.participants} Is Store Champ: {isStoreChamp}");
        }

        public async Task GetTournamentByUrl(string url)
        {
            var tournament = await tournamentClient.GetTournamentByUrl(url);

            bool isStoreChamp = tournament.is_store_championship == 1 ? true : false;

            Console.WriteLine($"\n--------------------Get By Url: {url} ---------------------\n" +
                $"Name: {tournament.name} Participants: {tournament.participants} Is Store Champ: {isStoreChamp}");

        }

        public async Task GetParticipants()
        {
            var participants = await tournamentClient.GetAllParticipants(testDataTournament.id);
            Console.WriteLine($"\n--------------------Get Participants for tournament - {testDataTournament.name} ---------------------\n");
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
            var matches = await tournamentClient.GetAllMatches(testDataTournament.id);
            Console.WriteLine($"\n--------------------Get Matches for tournament - {testDataTournament.name} ---------------------\n");
            if(matches == null || matches.Count() == 0)
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
    }
}