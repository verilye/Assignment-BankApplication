using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebDevAss2.Data.Repositories;

public class UserDataWebServiceRepository<T> : IUserDataWebServiceRepository<T>
{
    HttpClient client;
    public UserDataWebServiceRepository(HttpClient client)
    {
        this.client = client;
    }

    public async Task<T> FetchJsonData(string url)
    {
        Console.WriteLine("Loading user data from api...");
        // Pull user data from passed in url and return it as a string
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        // deserialise Json 
        T userData = JsonConvert.DeserializeObject<T>(responseBody);

        if (userData == null) { Console.WriteLine("Error: User data NOT loaded!"); }
        return userData!;
    }

    public void StoreJsonData(List<T> data){
        Console.WriteLine("Implement me");
        return;
    }
}