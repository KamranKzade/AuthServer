using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core.Models;

public class UserApp : IdentityUser
{
	public string? City { get; set; }
	public DateTime BirthDate { get; set; }
}
