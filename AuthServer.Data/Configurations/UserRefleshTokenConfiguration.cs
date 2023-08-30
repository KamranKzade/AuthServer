using AuthServer.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthServer.Data.Configurations
{
	public class UserRefleshTokenConfiguration : IEntityTypeConfiguration<UserRefleshToken>
	{
		public void Configure(EntityTypeBuilder<UserRefleshToken> builder)
		{
			builder.HasKey(x => x.UserId);
			builder.Property(x=>x.Code).IsRequired().HasMaxLength(200);
		}
	}
}
