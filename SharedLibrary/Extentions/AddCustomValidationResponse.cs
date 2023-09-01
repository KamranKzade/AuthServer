using SharedLibrary.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace SharedLibrary.Extentions;

public static class AddCustomValidationResponse
{
	// Fluent validation-dan gelen xetalari 1 yere yigib, response olaraq geri qaytaririq
	public static void UseCustomValidationResponse(this IServiceCollection services)
	{
		services.Configure<ApiBehaviorOptions>(opts =>
		{
			opts.InvalidModelStateResponseFactory = context =>
			{
				var errors = context.ModelState.Values.Where(x => x.Errors.Count > 0)
													  .SelectMany(x => x.Errors)
													  .Select(x => x.ErrorMessage)
													  .ToList();


				ErrorDto errorDto = new ErrorDto(errors, true);

				var response = Response<NoContentResult>.Fail(errorDto, 400);

				return new BadRequestObjectResult(response);
			};
		});
	}


}
