using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;
public class Customer{

    [Key]
    [Required, MaxLength(4), MinLength(4)]
    public int customerId{get;set;}

    [DataType(DataType.Text)]
    [Required, MaxLength(50), MinLength(1)]
    public string name{get;set;}

    [DataType(DataType.Text)]
    [MinLength(11), MaxLength(11)] 
    [RegularExpression(@"[0-9]+\s[0-9]+\s[0-9]+",
        ErrorMessage ="Format needs to be XXX XXX XXX")]
    public int tfn{get;set;}

    [DataType(DataType.Text)]
    [MaxLength(50), MinLength(1)]
    public string address{get;set;}

    [DataType(DataType.Text)]
    [MaxLength(40), MinLength(1)]
    public string city{get;set;}

    [DataType(DataType.Text)]
    [MaxLength(3), MinLength(2)]
    public string state{get;set;}

    [MaxLength(4), MinLength(4)]
    public int postCode{get;set;}

    [RegularExpression(@"04\d\d\s\d\d\d\s\d\d\d", 
        ErrorMessage = "Format needs to be 04XX XXX XXX")]
    [MaxLength(12)]
    public int mobile{get;set;}

    public ICollection<Account> accounts{get;set;}
    public Login login{get;set;}

    public Customer(int customerId, string name, int tfn, string address, string city, string state, int mobile, int postCode)
    {
        this.customerId = customerId;
        this.name = name;
        this.tfn = tfn;
        this.address = address;
        this.city = city;
        this.state = state;
        this.mobile = mobile;  
        this.postCode = postCode;
    }
}