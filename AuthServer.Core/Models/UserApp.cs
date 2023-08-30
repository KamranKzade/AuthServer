using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models;

public class UserApp:IdentityUser<string>
{
    public string City { get; set; }
}
