namespace WebDevAss2.Repositories;

public interface IJsonDataWebServiceRepository<T>
{
    public Task<List<T>> FetchJsonData(string url);
    public void StoreJsonData(List<T> data);

}



