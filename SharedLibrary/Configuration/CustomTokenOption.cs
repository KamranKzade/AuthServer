namespace SharedLibrary.Configuration;

public class CustomTokenOption
{
	public List<string> Audience { get; set; }
	public string Issuer { get; set; }
	public int AccessTokenExpiration { get; set; }
	public int RefleshTokenExpiration { get; set; }
	public string SecurityKey { get; set; }
}
