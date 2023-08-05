using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IDataAccessRepository
{
    public void InitUserData(List<Customer> data);
    public Customer GetUserByCustomerId(int customerID);

}



