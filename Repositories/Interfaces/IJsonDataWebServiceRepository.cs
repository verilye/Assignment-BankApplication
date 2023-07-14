namespace WebDevAss2.Repositories;

public interface IJsonDataWebServiceRepository()
{
    public async Task<List<T>> FetchJsonData(string url);
    public void StoreJsonData(List<T> data);

}



