using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services;

public interface IUserService
{
	Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto);
	Task<Response<UserAppDto>> GetUserByNameAsync(string userName);
}