namespace AuthServer.Core.Dtos;

public class TokenDto
{
	public string AccessToken { get; set; }
	public DateTime AccessTokenExpiration { get; set; }
	public string RefleshToken { get; set; }
	public DateTime RefleshTokenExpiration { get; set; }
}
