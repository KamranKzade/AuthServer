using SharedLibrary.Dtos;
using AuthServer.Core.Dtos;

namespace AuthServer.Core.Services;

public interface IAuthenticationService
{
	Task<Response<TokenDto>> CreateToken(LogInDto logIn);
	Task<Response<TokenDto>> CreateTokenByRefleshToken(string refleshToken);
	Task<Response<NoDataDto>> RevokeRefleshToken(string refleshToken);
	Task<Response<ClientTokenDto>> CreateTokenByClient(ClientLogInDto clientLogInDto);
}