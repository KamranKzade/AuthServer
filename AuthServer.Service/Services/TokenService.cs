using AuthServer.Core.Dtos;
using System.Security.Claims;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using SharedLibrary.Configuration;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using AuthServer.Core.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

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

	private async Task<IEnumerable<Claim>> GetClaims(UserApp userApp, List<string> audiences)
	{
		// userin role-larini aliriq
		var userRoles = await _userManager.GetRolesAsync(userApp);

		var userList = new List<Claim>
		{
			new Claim(ClaimTypes.NameIdentifier, userApp.Id),
			new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
			new Claim(ClaimTypes.Name, userApp.UserName),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
		};
		userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
		userList.AddRange(userRoles.Select(x => new Claim(ClaimTypes.Role, x)));

		return userList;
	}

	private IEnumerable<Claim> GetClaimByClient(Client client)
	{
		var claims = new List<Claim>();
		claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));
		new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
		new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

		return claims;
	}

	public TokenDto CreateToken(UserApp userApp)
	{
		// Tokenle bagli olan melumatlar
		var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
		var refleshTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.RefleshTokenExpiration);
		var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

		// credentiallari yaradiriq
		SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

		// jwt token aliriq
		JwtSecurityToken jwttoken = new JwtSecurityToken(
			issuer: _tokenOption.Issuer,
			expires: accessTokenExpiration,
			notBefore: DateTime.MinValue,
			claims: GetClaims(userApp, _tokenOption.Audience).Result,
			signingCredentials: credentials);

		// token yaratmaq ucun lazim olan handler
		var handler = new JwtSecurityTokenHandler();

		// handler uzerinden token yazmaq
		var token = handler.WriteToken(jwttoken);

		var tokenDto = new TokenDto
		{
			AccessToken = token,
			RefleshToken = CreateRefleshToken(),
			AccessTokenExpiration = accessTokenExpiration,
			RefleshTokenExpiration = refleshTokenExpiration,
		};

		return tokenDto;
	}

	public ClientTokenDto CreateTokenByClient(Client client)
	{
		// Tokenle bagli olan melumatlar
		var accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
		var securityKey = SignService.GetSymmetricSecurityKey(_tokenOption.SecurityKey);

		// credentiallari yaradiriq
		SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

		// jwt token aliriq
		JwtSecurityToken jwttoken = new JwtSecurityToken(
			issuer: _tokenOption.Issuer,
			expires: accessTokenExpiration,
			notBefore: DateTime.MinValue,
			claims: GetClaimByClient(client),
			signingCredentials: credentials);

		// token yaratmaq ucun lazim olan handler
		var handler = new JwtSecurityTokenHandler();

		// handler uzerinden token yazmaq
		var token = handler.WriteToken(jwttoken);

		var clientTokenDto = new ClientTokenDto
		{
			AccessToken = token,
			AccessTokenExpiration = accessTokenExpiration,
		};

		return clientTokenDto;
	}
}
