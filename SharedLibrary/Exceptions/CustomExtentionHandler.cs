using System.Text.Json;
using SharedLibrary.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;

namespace SharedLibrary.Exceptions
{
	public static class CustomExtentionHandler
	{
		public static void UseCustomException(this IApplicationBuilder app)
		{
			app.UseExceptionHandler(config =>
			{
				config.Run(async context =>
				{
					context.Response.StatusCode = 500;
					context.Response.ContentType = "application/json";

					var errorFeature = context.Features.Get<IExceptionHandlerFeature>();

					if (errorFeature != null)
					{
						var ex = errorFeature.Error;

						ErrorDto errorDto = null;

						if (ex is CustomException)
							errorDto = new(ex.Message, true);
						else
							errorDto = new(ex.Message, false);

						var response = Response<NoDataDto>.Fail(errorDto, 500);

						await context.Response.WriteAsync(JsonSerializer.Serialize(response));
					}

				});
			});
		}
	}
}
