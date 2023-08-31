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
			userRefleshToken.Expiration = token.RefleshTokenExpiration;
		}
		await _unitOfWork.CommitAsync();

		return Response<TokenDto>.Success(token, 200);
	}

	public Response<ClientTokenDto> CreateTokenByClientAsync(ClientLogInDto clientLogInDto)
	{
		var client = _client.SingleOrDefault(x => x.Id == clientLogInDto.ClientId && x.Secret == clientLogInDto.ClientSecret);

		if (client == null)
			return Response<ClientTokenDto>.Fail("ClientId or ClientSecret not found", 404, true);

		var token = _tokenService.CreateTokenByClient(client);

		return Response<ClientTokenDto>.Success(token, 200);


	}

	public async Task<Response<TokenDto>> CreateTokenByRefleshTokenAsync(string refleshToken)
	{
		var existRefleshToken = await _userRefleshTokenService.Where(x => x.Code == refleshToken).SingleOrDefaultAsync();

		if (existRefleshToken == null) return Response<TokenDto>.Fail("Reflesh token not found ", 404, true);

		var user = await _userManager.FindByIdAsync(existRefleshToken.UserId);

		if (user == null) return Response<TokenDto>.Fail("User Id not found", 404, true);

		var tokenDto = _tokenService.CreateToken(user);

		existRefleshToken.Code = tokenDto.RefleshToken;
		existRefleshToken.Expiration = tokenDto.RefleshTokenExpiration;

		await _unitOfWork.CommitAsync();

		return Response<TokenDto>.Success(tokenDto, 200);
	}

	public async Task<Response<NoDataDto>> RevokeRefleshTokenAsync(string refleshToken)
	{
		var existRefleshToken = await _userRefleshTokenService.Where(x => x.Code == refleshToken).SingleOrDefaultAsync();

		if (existRefleshToken == null) return Response<NoDataDto>.Fail("Reflesh token not found ", 404, true);


		_userRefleshTokenService.Remove(existRefleshToken);

		await _unitOfWork.CommitAsync();

		return Response<NoDataDto>.Success(200);
	}
}
