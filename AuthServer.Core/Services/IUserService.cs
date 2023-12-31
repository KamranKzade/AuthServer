﻿using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Core.Services;

public interface IUserService
{
	Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
	Task<Response<UserAppDto>> GetUserByNameAsync(string userName);

	Task<Response<NoDataDto>> CreateUserRoles(string username);

}