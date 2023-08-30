using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace AuthServer.Data;

public class AppDbContext : IdentityDbContext<UserApp, IdentityRole, string>
{
	public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


	public DbSet<Product> Products { get; set; }
	public DbSet<UserRefleshToken> UserRefleshTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
	}
}
