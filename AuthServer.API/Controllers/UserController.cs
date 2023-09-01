using AuthServer.Core.Dtos;
using Microsoft.AspNetCore.Mvc;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Authorization;
using SharedLibrary.Exceptions;

namespace AuthServer.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : CustomBaseController
{
	private readonly IUserService _userService;

	public UserController(IUserService userService)
	{
		_userService = userService;
	}


	[HttpPost]
	public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
	{
		// throw new CustomException("Dbda bir xeta meydana geldi");
		return ActionResultInstance(await _userService.CreateUserAsync(createUserDto));
	}

	[Authorize]
	[HttpGet]
	public async Task<IActionResult> GetUser()
	{
		return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity!.Name!));
	}

}
