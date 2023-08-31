using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.Extensions.Options;
using AuthServer.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using AuthServer.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Service.Services;

public class AuthenticationService : IAuthenticationService
{
	private readonly List<Client> _client;
	private readonly ITokenService _tokenService;
	private readonly UserManager<UserApp> _userManager;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IGenericRepository<UserRefleshToken> _userRefleshTokenService;

	public AuthenticationService(IOptions<List<Client>> client, ITokenService tokenService, UserManager<UserApp> userManager,
		IUnitOfWork unitOfWork, IGenericRepository<UserRefleshToken> userRefleshTokenService)
	{
		_client = client.Value;
		_tokenService = tokenService;
		_userManager = userManager;
		_unitOfWork = unitOfWork;
		_userRefleshTokenService = userRefleshTokenService;
	}

	public async Task<Response<TokenDto>> CreateTokenAsync(LogInDto logIn)
	{
		if (logIn == null) throw new ArgumentNullException(nameof(logIn));

		var user = await _userManager.FindByEmailAsync(logIn.Email);

		if (user == null) return Response<TokenDto>.Fail("Email or Password is wrong", 400, isShow: true);

		if (!await _userManager.CheckPasswordAsync(user, logIn.Password))
			return Response<TokenDto>.Fail("Email or Password is wrong", 400, isShow: true);

		var token = _tokenService.CreateToken(user);

		var userRefleshToken = await _userRefleshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

		if (userRefleshToken == null)
			await _userRefleshTokenService.AddAsync(new UserRefleshToken { UserId = user.Id, Code = token.RefleshToken, Expiration = token.RefleshTokenExpiration });
		else
		{
			userRefleshToken.Code = token.RefleshToken;
			userRefleshToken.Expiration= token.RefleshTokenExpiration;
		}
		await _unitOfWork.CommitAsync();

		return Response<TokenDto>.Success(token, 200);
	}

	public Task<Response<ClientTokenDto>> CreateTokenByClientAsync(ClientLogInDto clientLogInDto)
	{
		throw new NotImplementedException();
	}

	public Task<Response<TokenDto>> CreateTokenByRefleshTokenAsync(string refleshToken)
	{
		throw new NotImplementedException();
	}

	public Task<Response<NoDataDto>> RevokeRefleshTokenAsync(string refleshToken)
	{
		throw new NotImplementedException();
	}
}
