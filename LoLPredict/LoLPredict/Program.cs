using DataScraper;
namespace LoLPredict
{
    public class Program
    {

        static void Main(string[] args)
        {
            string apiKey = "LoL Fake API key";
            RiotAPIService riotApi = new RiotAPIService(apiKey);
           
            /*
             * 1. Get all Challenger players
             * 2. Get all recent games to build list of matches
             * 3. For each player in those matches, acquire all ranked 5x5 games from season 8 for them 
             * 4. From each of those games (written to database after each summoner so they can be read from database rather than gotten), build player season stats
             * 5. Get any other relevant summoner info (rank, LP, hot streak, etc.)
             * 6. Create AI project to preprocess data into usable features
             * 7. Create neural net/other classification models within AI project to utilize acquired data.
             * 
             */
        }
    }
}
