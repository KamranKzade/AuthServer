using AuthServer.Core.Dtos;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class AuthController : CustomBaseController
{
	private readonly IAuthenticationService _authService;

	public AuthController(IAuthenticationService authService)
	{
		_authService = authService;
	}


	[HttpPost]
	public async Task<IActionResult> CreateToken(LogInDto logInDto)
	{
		var result = await _authService.CreateTokenAsync(logInDto);

		return ActionResultInstance(result);
	}


	[HttpPost]
	public IActionResult CreateTokenByClient(ClientLogInDto clientLogInDto)
	{
		var result = _authService.CreateTokenByClientAsync(clientLogInDto);

		return ActionResultInstance(result);
	}


	[HttpPost]
	public async Task<IActionResult> RevokeRefleshToken(RefleshTokenDto refleshTokenDto)
	{
		var result = await _authService.RevokeRefleshTokenAsync(refleshTokenDto.Token);

		return ActionResultInstance(result);
	}


	[HttpPost]
	public async Task<IActionResult> CreateTokenByRefleshToken(RefleshTokenDto refleshTokenDto)
	{
		var result = await _authService.CreateTokenByRefleshTokenAsync(refleshTokenDto.Token);

		return ActionResultInstance(result);
	}
}
