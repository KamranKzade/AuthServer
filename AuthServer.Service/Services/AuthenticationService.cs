using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using Microsoft.Extensions.Options;
using AuthServer.Core.Repositories;
using Microsoft.AspNetCore.Identity;
using AuthServer.Core.Configuration;

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

	public Task<Response<TokenDto>> CreateTokenAsync(LogInDto logIn)
	{
		throw new NotImplementedException();
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
