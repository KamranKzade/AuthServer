using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MiniApp1.API.Controllers;

[Authorize(Roles = "admin")]
[Route("api/[controller]")]
[ApiController]
public class StockController : ControllerBase
{
	[HttpGet]
	public IActionResult GetStock()
	{
		var userName = HttpContext.User.Identity.Name;
		var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


		// db-dan userId veya username uzerinden lazimli datalari cek
		return Ok($"Stock islemleri ==> Username: {userName} -- UserId:{userId.Value}");
	}

}
