using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace USDAFoods
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();

            Console.Write("Enter a food item to search: ");
            string query = Console.ReadLine();

            UsdaFoodApiClient client = new UsdaFoodApiClient(new HttpClient(), config);
            string searchResultJson = await client.SearchFood(query);

            // Parse the JSON response
            var searchResult = JsonSerializer.Deserialize<JsonDocument>(searchResultJson);
            var foods = searchResult.RootElement.GetProperty("list").GetProperty("item").EnumerateArray();

            if (!foods.Any())
            {
                Console.WriteLine("No results found.");
            }
            else
            {
                Console.WriteLine("Search Results:");
                foreach (var food in foods)
                {
                    string name = food.GetProperty("name").GetString();
                    string ndbno = food.GetProperty("ndbno").GetString();
                    Console.WriteLine($"- {name} (NDB Number: {ndbno})");
                }
            }
        }
    }
}