using AuthServer.Core.Dtos;
using AuthServer.Core.Models;
using AuthServer.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ProductController : CustomBaseController
{
	private readonly IServiceGeneric<Product, ProductDto> _productService;

	public ProductController(IServiceGeneric<Product, ProductDto> serviceGeneric)
	{
		_productService = serviceGeneric;
	}


	[HttpGet]
	public async Task<IActionResult> GetProduct()
	{
		return ActionResultInstance(await _productService.GetAllAsync());
	}


	[HttpPost]
	public async Task<IActionResult> SaveProduct(ProductDto product)
	{
		return ActionResultInstance(await _productService.AddAsync(product));
	}


	[HttpPut]
	public async Task<IActionResult> UpdateProduct(ProductDto product)
	{
		return ActionResultInstance(await _productService.UpdateAsync(product, product.Id));
	}


	[HttpDelete]
	public async Task<IActionResult> DeleteProduct(int id)
	{
		return ActionResultInstance(await _productService.RemoveAsync(id));
	}
}
