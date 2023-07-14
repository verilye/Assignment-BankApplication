using System;
using System.Net.Http;
using Newtonsoft.Json;

namespace WebDevAss2.Repositories;

class JsonDataWebServiceRepository<T> : IJsonDataWebServiceRepository
{
    HttpClient client;
    public JsonDataWebServiceRepository(HttpClient client)
    {
        this.client = client;
    }


    public async Task<List<T>> FetchJsonData(string url)
    {
        Console.WriteLine("Loading user data from api...");
        // Pull user data from passed in url and return it as a string
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        // deserialise Json 
        List<T>? userData = JsonConvert.DeserializeObject<List<T>>(responseBody);

        if (userData == null) { Console.WriteLine("Error: User data NOT loaded!"); }
        return userData!;
    }

    public void StoreJsonData(){
        Console.WriteLine("Implement me");
        return;
    }
}