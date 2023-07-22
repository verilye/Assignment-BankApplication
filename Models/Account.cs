namespace WebDevAss2.Models;
public class Account
{

    [Key]
    [Required]
    [MaxLength(4), MinLength(4)]
    required public int accountNumber;

    [Required]
    public AccountType accountType;

    // Foreign Key
    public int customerId;
    public ICollection<Transaction> transactions;

    // Login is like an object or a struct potentially, store it differently

    public Account(int accountNumber, char accountType, int customerId, Transaction[] transactions)
    {
        this.accountNumber = accountNumber;
        this.accountType = accountType;
        this.customerId = customerId;
        this.transactions = transactions;
    }

}