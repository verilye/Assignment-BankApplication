using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebDevAss2.Models;
public class Login
{

    [Key]
    [Required]
    [MaxLength(8), MinLength(8)]
    public string loginId;

    // Foreign Key
    public int customerId;

    [Required]
    [MaxLength(94), MinLength(3)]
    public string passwordHash;



    public Login(string loginId, string passwordHash)
    {   
        this.loginId = loginId;
        this.passwordHash = passwordHash;

    }

}