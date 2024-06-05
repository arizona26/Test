using Newtonsoft.Json;
using System.Net;
using Test.Model;

namespace Test.Clients
{
    public class FootballClients
    {
        private static string _address;
        private static string _apikey;
        private static string _apiHost;
        private HttpClient _httpClient;

        public FootballClients()
        {
            _httpClient = new HttpClient();
            _address = Constants.Address;
            _apikey = Constants.ApiKey;
            _apiHost = Constants.ApiHost;
        }
        public async Task<TeamSearch> GetTeamById(int Id)
        {
            try
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"{_address}/v3/teams?id={Id}"),
                    Headers =
            {
                { "X-RapidAPI-Key", _apikey },
                { "X-RapidAPI-Host", _apiHost },
            }
                };

                Console.WriteLine($"Sending request to: {request.RequestUri}");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response: {content}");

                if (string.IsNullOrEmpty(content))
                {
                    Console.WriteLine("Помилка: Відповідь API пуста.");
                    return null;
                }

                var result = JsonConvert.DeserializeObject<TeamSearch>(content);
                if (result == null)
                {
                    Console.WriteLine("Помилка: Неможливо десеріалізувати відповідь API.");
                }
                else
                {
                    Console.WriteLine($"Deserialized result: {JsonConvert.SerializeObject(result)}");
                }

                return result;
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP помилка: {httpEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Загальна помилка: {ex.Message}");
                return null;
            }
        }
        public async Task<MatchSearch> GetNextMatchById(int teamid, int number)
        {
            var client = new HttpClient();
            var requestUri = new Uri($"{_address}/v2/fixtures/team/{teamid}/next/{number}?timezone=Europe%2FLondon");
            Console.WriteLine($"Запит URL: {requestUri}");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = requestUri,
                Headers =
                {
                    { "X-RapidAPI-Key", _apikey },
                    { "X-RapidAPI-Host", _apiHost },
                },
            };

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Помилка: {response.StatusCode}, Деталі: {errorResponse}");
                return null;
            }

            var body = await response.Content.ReadAsStringAsync();
            var seasonInfo = JsonConvert.DeserializeObject<MatchSearch>(body);
            return seasonInfo;
        }
        public async Task<NameSearch> GetName(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_address);

                var requestUri = $"/v2/teams/search/{name}";
                Console.WriteLine($"Request URL: {client.BaseAddress}{requestUri}");

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(client.BaseAddress, requestUri),
                    Headers =
                    {
                        { "X-RapidAPI-Key", _apikey },
                        { "X-RapidAPI-Host", _apiHost },
                    },
                };

                try
                {
                    var response = await client.SendAsync(request);

                    if (!response.IsSuccessStatusCode)
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"Error: {response.StatusCode}, Details: {errorResponse}");

                        if (response.StatusCode == HttpStatusCode.NotFound)
                        {
                            Console.WriteLine("Team not found (404).");
                            return null;
                        }

                        return null;
                    }

                    var body = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Body: {body}");
                    var nameSearch = JsonConvert.DeserializeObject<NameSearch>(body);

                    if (nameSearch == null)
                    {
                        Console.WriteLine("Deserialization resulted in a null object.");
                    }

                    return nameSearch;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception occurred: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
