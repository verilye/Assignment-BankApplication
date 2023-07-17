namespace WebDevAss2.Models;

public class Transaction
{
    public float amount;
    public char TransactionType;
    public string transactionTimeUtc;
    public string? comment;
    public int accountNumber;
    public int? destinationAccountNumber;
    public string transactionID;

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