namespace WebDevAss2.Models;
public class Account
{
    required public int accountNumber;
    public char accountType;
    public int customerId;
    public Transaction[] transactions;

    // Login is like an object or a struct potentially, store it differently

    public Account(int accountNumber, char accountType, int customerId, Transaction[] transactions)
    {
        this.accountNumber = accountNumber;
        this.accountType = accountType;
        this.customerId = customerId;
        this.transactions = transactions;
    }





}