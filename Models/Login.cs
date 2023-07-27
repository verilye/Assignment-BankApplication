using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebDevAss2.Models;
public class Login
{

    [Key]
    [Required]
    [Column(TypeName = "char")]
    [MaxLength(8), MinLength(8)]
    public string loginId{get;set;}

    // Foreign Key
    public int customerId{get;set;}

    [Required]
    [Column(TypeName = "char")]
    [MaxLength(94), MinLength(3)]
    public string passwordHash{get;set;}



    public Login(string loginId,int customerId, string passwordHash)
    {   
        this.loginId = loginId;
        this.customerId = customerId;
        this.passwordHash = passwordHash;

    }

}