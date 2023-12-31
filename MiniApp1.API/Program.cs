using Microsoft.AspNetCore.Authorization;
using MiniApp1.API.Requirements;
using SharedLibrary.Configuration;
using SharedLibrary.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IAuthorizationHandler,BirthDayRequirementHandler>();

builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
builder.Services.AddCustomTokenAuth(tokenOptions);

builder.Services.AddAuthorization(opts =>
{
	opts.AddPolicy("BakuPolicy", policy =>
	{
		policy.RequireClaim("city", "baku");
	});

	opts.AddPolicy("AgePolicy", policy =>
	{
		policy.Requirements.Add(new BirthDayRequirement(18));
	});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
