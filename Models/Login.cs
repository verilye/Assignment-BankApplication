namespace WebDevAss2.Models;
public class Login
{
    public int loginId;
    public string passwordHash;

    public Login(int loginId, string passwordHash)
    {   
        this.loginId = loginId;
        this.passwordHash = passwordHash;

    }

}