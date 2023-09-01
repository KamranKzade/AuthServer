using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace MiniApp2.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
	[HttpGet]
	public IActionResult GetInvoice()
	{
		var userName = HttpContext.User.Identity.Name;
		var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);


		// db-dan userId veya username uzerinden lazimli datalari cek
		return Ok($"Invoice islemleri ==> Username: {userName}-- UserId:{userId.Value}");
	}

}