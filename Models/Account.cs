using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebDevAss2.Models;

namespace WebDevAss2.Models;
public class Account
{

    [Key]
    [Required]
    [MaxLength(4), MinLength(4)]
    required public int AccountNumber{get;set;}

    [Required]
    [Column(TypeName = "char(1)")]
    public char AccountType{get;set;}

    // Foreign Key
    public int CustomerId{get;set;}
    public ICollection<Transaction>? Transactions{get;set;}

    public ICollection<BillPay>? BillPays{get;set;}

    public Customer? Customer{get;set;}

    // Login is like an object or a struct potentially, store it differently

    public Account(int accountNumber, char accountType, int customerId)
    {
        this.AccountNumber = accountNumber;
        this.AccountType = accountType;
        this.CustomerId = customerId;
    }

}