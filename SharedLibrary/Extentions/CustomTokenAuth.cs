using SharedLibrary.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace SharedLibrary.Extentions;

public static class CustomTokenAuth
{
	public static void AddCustomTokenAuth(this IServiceCollection services, CustomTokenOption tokenOptions)
	{
		services.AddAuthentication(option =>
		{
			// Sechema ni secirikki ne olacaq, eger 1 dene authserverimiz varsa bele olur, coxdursa ayri ayri qeyd etmeliyik
			option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

			// JwtDen gelen schemani, default scema ile elaqelendirdik
			option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme /* Jwtonden gelen schema */ , opts =>
		{
			// Token geldikde burdaki emrlere gore dogrulamani yerine yetirecek

			opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
			{
				ValidIssuer = tokenOptions.Issuer,
				ValidAudience = tokenOptions.Audience[0],
				IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

				ValidateIssuerSigningKey = true, // imzani kontrol edirik
				ValidateAudience = true, // bizim audience di, yeni oz name-i var audience-ler icerisinde
				ValidateIssuer = true, // bizim gonderdiyimiz issuerdi,bunu yoxlayiriq
				ValidateLifetime = true, // omrunu kontrol edirik, yeni kecerli 1 tokendi ya yox	


				ClockSkew = TimeSpan.Zero // Tokene bir omur verdikde, elave olaraq 5 deyqe vaxt verir, biz burda elave vaxt vermirik ( 0 edirik )
			};


		});

	}

}
