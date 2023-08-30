using AuthServer.Core.Configuration;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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


	public TokenDto CreateToken(UserApp userApp)
	{
		throw new NotImplementedException();
	}

	public ClientTokenDto CreateTokenByClient(Client client)
	{
		throw new NotImplementedException();
	}
}
