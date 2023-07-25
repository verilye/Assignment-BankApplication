using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;
public class Login
{

    [Key]
    [Required]
    [MaxLength(8), MinLength(8)]
    public string loginId{get;set;}

    // Foreign Key
    public int customerId{get;set;}

    [Required]
    [MaxLength(94), MinLength(3)]
    public string passwordHash{get;set;}



    public Login(string loginId,int customerId, string passwordHash)
    {   
        this.loginId = loginId;
        this.customerId = customerId;
        this.passwordHash = passwordHash;

    }

}