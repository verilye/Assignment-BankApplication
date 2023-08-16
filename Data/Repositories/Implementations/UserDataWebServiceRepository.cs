using System;
using System.Net.Http;
using Newtonsoft.Json;
using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public class UserDataWebServiceRepository<T> : IUserDataWebServiceRepository<T>
{
    private readonly HttpClient _client;
    private readonly IDataAccessRepository _dataAccess;
    public UserDataWebServiceRepository(IDataAccessRepository dataAccess, HttpClient client)
    {
        _client = client;
        _dataAccess = dataAccess;
    }

    public async Task<T> FetchJsonData(string url)
    {
        try
        {
            using (var _client = new HttpClient())
            {

                Console.WriteLine("Loading user data from api...");
                // Pull user data from passed in url and return it as a string
                HttpResponseMessage response = await _client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                // deserialise Json 
                T userData = JsonConvert.DeserializeObject<T>(responseBody)!;

                if (userData == null) { Console.WriteLine("Error: User data NOT loaded!"); }
                return userData!;
            }
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"HTTP request error: {ex.Message}");
            throw; // Rethrow the exception to be handled at a higher level
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"JSON deserialization error: {ex.Message}");
            throw; // Rethrow the exception to be handled at a higher level
        }
    }
}