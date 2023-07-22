namespace WebDevAss2.Models;

public class Customer{

    [Key]
    [Required, MaxLength(4), MinLength(4)]
    public int customerId;

    [DataType(DataType.Text)]
    [Required, MaxLength(50), MinLength(1)]
    public string name;

    [DataType(DataType.Text)]
    [MinLength(11), MaxLength(11)] 
    [RegularExpression(@"[0-9]+\s[0-9]+\s[0-9]+",
        ErrorMessage ="Format needs to be XXX XXX XXX")]
    public int TFN;

    [DataType(DataType.Text)]
    [MaxLength(50), MinLength(1)]
    public string address;

    [DataType(DataType.Text)]
    [MaxLength(40), MinLength(1)]
    public string city;

    [DataType(DataType.Text)]
    [MaxLength(3), MinLength(2)]
    public State state;

    [MaxLength(4), MinLength(4)]
    public int postCode;

    [RegularExpression(@"04\d\d\s\d\d\d\s\d\d\d", 
        ErrorMessage = "Format needs to be 04XX XXX XXX")]
    [MaxLength(12)]
    public int mobile;

    public ICollection<Account> accounts;
    public Login login;

    public Customer(int customerId, string name, string address, string city, string postCode, Account[] accounts, Login login)
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