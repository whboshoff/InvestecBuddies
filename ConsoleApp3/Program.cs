using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ConsoleApp3
{
	class Program
	{
        //https://swapi.dev/api/people
        // HttpClient is intended to be instantiated once per application, rather than per-use. See Remarks.
        static readonly HttpClient client = new HttpClient();

        static async Task Main()
        {
            // Call asynchronous network methods in a try/catch block to handle exceptions.
            try
            {
                List<Character> characters = new List<Character>();
                HttpResponseMessage response = await client.GetAsync("https://swapi.dev/api/people");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                APIBaseResponseModel responseModel = JsonConvert.DeserializeObject<APIBaseResponseModel>(responseBody);
                characters.AddRange(responseModel.results);

                while (!String.IsNullOrEmpty(responseModel.next))
                {
                    try
                    {
                        response = await client.GetAsync(responseModel.next);
                    }
                    catch
                    {
                        break;
                    }
                    responseBody = await response.Content.ReadAsStringAsync();
                    responseModel = JsonConvert.DeserializeObject<APIBaseResponseModel>(responseBody);
                    characters.AddRange(responseModel.results);
                }

                foreach (Character character in characters)
                {
                    Console.WriteLine(character.name);
                    foreach (string film in character.films)
						Console.WriteLine($"     {film}");
                }

                //Console.WriteLine(responseBody);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private Dictionary<int, string[]> GetFilmGroups(Character[] characters)
		{
            Dictionary<int, string[]> groups = new Dictionary<int, string[]>();
            int groupNumber = 0;
            foreach (Character character in characters)
			{
                if (groupNumber == 0)
                    groups.Add(groupNumber, character.films);

                foreach()
			}
		}

    }
}
