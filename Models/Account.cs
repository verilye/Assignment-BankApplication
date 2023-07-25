using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebDevAss2.Models;

namespace WebDevAss2.Models;
public class Account
{

    [Key]
    [Required]
    [MaxLength(4), MinLength(4)]
    required public int accountNumber{get;set;}

    [Required]
    public char accountType{get;set;}

    // Foreign Key
    public int customerId{get;set;}
    public ICollection<Transaction> transactions{get;set;}

    // Login is like an object or a struct potentially, store it differently

    public Account(int accountNumber, char accountType, int customerId)
    {
        this.accountNumber = accountNumber;
        this.accountType = accountType;
        this.customerId = customerId;
    }

}