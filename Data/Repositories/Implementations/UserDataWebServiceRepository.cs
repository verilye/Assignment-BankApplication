using System;
using System.Net.Http;
using Newtonsoft.Json;
using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public class UserDataWebServiceRepository<T> : IUserDataWebServiceRepository<T>
{
    private readonly HttpClient _client;
    public UserDataWebServiceRepository(HttpClient client)
    {
        _client = client;
    }

      public async Task<T> FetchJsonData(string url)
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