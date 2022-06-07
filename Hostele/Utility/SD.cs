using Hostele.Models;

namespace Hostele.Utility;

public class SD
{
    public const string Role_User = "User";
    public const string Role_Admin = "Admin";
    
   

    public bool f(AppUser user)
    {
        if (user.isAdmin)
        {
            return true;
        }

        return false;
    }
}
