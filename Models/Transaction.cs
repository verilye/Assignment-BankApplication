namespace WebDevAss2.Models;

public class Transaction
{

    [Key, Required]
    public int transactionID;

    [Required, Range(0, int.MaxValue, ErrorMessage ="Must be a positive number")]
    public float amount;

    [Required]
    public TransactionType TransactionType;

    [Required, DataType(DataType.Date)]
    public string transactionTimeUtc;

    [MaxLength(30)]
    public string? comment;

    // Foreign Key
    public int accountNumber;
    //Foreign Key
    public int? destinationAccountNumber;

    public Transaction(float amount, char transactionType, string transactionTimeUtc, string comment, int accountNumber, int? destinationAccountNumber)
    {
        this.amount = amount;
        this.TransactionType = transactionType;
        this.transactionTimeUtc = transactionTimeUtc;
        this.comment = comment;
        this.accountNumber = accountNumber;
        this.destinationAccountNumber = destinationAccountNumber;

    }

    public void SetTransactionID(string transactionID){
        this.transactionID = transactionID;
    }

}