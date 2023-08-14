namespace WebDevAss2.Models;
public class AccountViewModel{
    public Account Account {get;set;} = default!;
    public float Balance {get;set;}
    public List<Transaction> Transactions {get;set;} = default!;
} 