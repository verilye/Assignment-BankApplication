namespace WebDevAss2.Models;

public class Customer{

    required public string customerId;
    public string name;
    public string address;
    public string city;
    public string postCode;
    public Account[] accounts;
    public Login login;

    public Customer(string customerId, string name, string address, string city, string postCode, Account[] accounts, Login login)
    {
        this.customerId = customerId;
        this.name = name;
        this.address = address;
        this.city = city;
        this.postCode = postCode;
        this.accounts = accounts;
        this.login = login;
    }
}