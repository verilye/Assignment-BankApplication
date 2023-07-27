using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IDataAccessRepository
{
    public void StoreJsonData(List<Customer> data);

}



