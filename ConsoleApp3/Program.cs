using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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

                Dictionary<string, string[]> buddies = GetFilmGroups(characters.ToArray());

				Console.WriteLine("Buddies:");
                foreach (KeyValuePair<string, string[]> group in buddies)
                {
					Console.WriteLine(group.Key);
					//foreach (string film in group.Value)
					//	Console.WriteLine($"     {film}");
				}
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }

        private static Dictionary<string, string[]> GetFilmGroups(Character[] characters)
		{
            Dictionary<string, string[]> groups = new Dictionary<string, string[]>();
            for(int i = 0; i < characters.Length; i++)
			{
                bool filmsMatch = false;
                bool characterAdded = false;
                string matchingKey = String.Empty;
                foreach(KeyValuePair<string, string[]> group in groups)
				{
                    //check if they have the same number of films
                    if (characters[i].films.Length != group.Value.Length)
                        continue;

                    for(int j = 0; j < group.Value.Length; j++)
					{
                        if (characters[i].films[j] != group.Value[j])
                        {
                            filmsMatch = false;
                            break;
                        }
                        filmsMatch = true;
					}

                    if(filmsMatch)
					{
                        matchingKey = group.Key;
                        characterAdded = true;
                        break;
					}
                    
				}

                if (!characterAdded)
                    groups.Add(characters[i].name, characters[i].films);
                else
                {
                    groups.Add($"{matchingKey}, {characters[i].name}", characters[i].films);
                    groups.Remove(matchingKey);
                }

            }

            return groups;
		}

    }
}
