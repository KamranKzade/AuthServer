using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Services;

public class TokenService : ITokenService
{
	private readonly UserManager<UserApp> _userManager;
	private readonly CustomTokenOption _tokenOption;

	public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOption> options)
	{
		_userManager = userManager;
		_tokenOption = options.Value;
	}

	// Reflesh Token yaradiriq
	private string CreateRefleshToken()
	{
		var numberByte = new Byte[32];

		// Random token aliriq, guidden daha etibarlidir
		using var rnd = RandomNumberGenerator.Create();

		rnd.GetBytes(numberByte);

		return Convert.ToBase64String(numberByte);
	}

	private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
	{
		var userList = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userApp.Id),
			new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
			new Claim(ClaimTypes.Name, userApp.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};
		userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

		return userList;
	}


	public TokenDto CreateToken(UserApp userApp)
	{
		throw new NotImplementedException();
	}

	public ClientTokenDto CreateTokenByClient(Client client)
	{
		throw new NotImplementedException();
	}
}
