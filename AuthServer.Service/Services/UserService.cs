﻿using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace AuthServer.Service.Services;

public class UserService : IUserService
{
	private readonly UserManager<UserApp> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserService(UserManager<UserApp> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}


	public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
	{
		var user = new UserApp
		{
			Email = createUserDto.Email,
			UserName = createUserDto.UserName
		};

		var result = await _userManager.CreateAsync(user, createUserDto.Password);

		if (!result.Succeeded)
		{
			var errors = result.Errors.Select(x => x.Description).ToList();

			return Response<UserAppDto>.Fail(new ErrorDto(errors, true), 400);
		}

		return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);
	}

	public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
	{
		var user = await _userManager.FindByNameAsync(userName);

		if (user == null) return Response<UserAppDto>.Fail("UserName not found", 404, true);

		return Response<UserAppDto>.Success(ObjectMapper.Mapper.Map<UserAppDto>(user), 200);

	}

	public async Task<Response<NoDataDto>> CreateUserRoles(string username)
	{
		if (! await	_roleManager.RoleExistsAsync("admin"))
		{
			await _roleManager.CreateAsync(new() { Name = "admin" });
			await _roleManager.CreateAsync(new() { Name = "manager" });
		}

		var user = await _userManager.FindByNameAsync(username);
		await _userManager.AddToRoleAsync(user, "admin");
		await _userManager.AddToRoleAsync(user, "manager");

		return Response<NoDataDto>.Success(StatusCodes.Status201Created);
	}

}
