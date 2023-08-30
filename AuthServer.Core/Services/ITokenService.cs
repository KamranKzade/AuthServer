using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Configuration;

namespace AuthServer.Core.Services;

public interface ITokenService
{
	TokenDto CreateToken(UserApp userApp);
	ClientTokenDto CreateTokenByClient(Client client);
}