using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebDevAss2.Models;

public class Transaction
{

    [Key, Required]
    public int transactionID { get; set; }

    [Required]
    [Column(TypeName = "char")]
    public char TransactionType { get; set; }

    // Foreign Key
    public int accountNumber { get; set; }
    //Foreign Key
    public int? destinationAccountNumber { get; set; }

    [Required, Range(0, int.MaxValue, ErrorMessage = "Must be a positive number")]
    [Column(TypeName = "money")]
    public float amount { get; set; }

    [MaxLength(30)]
    public string? comment { get; set; }

    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime transactionTimeUtc { get; set; }


    public Transaction(int transactionID, float amount, char transactionType, DateTime transactionTimeUtc, string comment, int accountNumber, int? destinationAccountNumber)
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