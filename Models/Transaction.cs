using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;

public class Transaction
{

    [Key, Required]
    public int transactionID{get;set;}

    [Required, Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float amount{get;set;}

    [Required]
    public char TransactionType{get;set;}

    [Required, DataType(DataType.Date)]
    public string transactionTimeUtc{get;set;}

    [MaxLength(30)]
    public string? comment{get;set;}

    // Foreign Key
    public int accountNumber{get;set;}
    //Foreign Key
    public int? destinationAccountNumber{get;set;}

    public Transaction(int transactionID, float amount, char transactionType, string transactionTimeUtc, string comment, int accountNumber, int? destinationAccountNumber)
    {
        this.transactionID = transactionID;
        this.amount = amount;
        this.TransactionType = transactionType;
        this.transactionTimeUtc = transactionTimeUtc;
        this.comment = comment;
        this.accountNumber = accountNumber;
        this.destinationAccountNumber = destinationAccountNumber;

    }

}