using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services;

public interface IAuthenticationService
{
	Task<Response<TokenDto>> CreateTokenAsync(LogInDto logIn);
	Task<Response<TokenDto>> CreateTokenByRefleshTokenAsync(string refleshToken);
	Task<Response<NoDataDto>> RevokeRefleshTokenAsync(string refleshToken);
	Task<Response<ClientTokenDto>> CreateTokenByClientAsync(ClientLogInDto clientLogInDto);
}