using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IUserDataWebServiceRepository<T>
{
    public Task<T> FetchJsonData(string url);

}



