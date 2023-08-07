using WebDevAss2.Models;

namespace WebDevAss2.Data.Repositories;

public interface IDataAccessRepository
{
    public bool CheckForPopulatedDb();
    public void InitUserData(List<Customer> data);
    public Customer GetCustomerByCustomerId(int customerID);
    public Login GetLoginByCustomerId(int customerID);


}



