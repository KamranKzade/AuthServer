using AuthServer.Data;
using AuthServer.Core.Models;
using SharedLibrary.Exceptions;
using SharedLibrary.Extentions;
using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Service.Services;
using SharedLibrary.Configuration;
using FluentValidation.AspNetCore;
using AuthServer.Core.Repositories;
using AuthServer.Data.Repositories;
using AuthServer.Core.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers()
				.AddFluentValidation(opts => // FluentValidationlari sisteme tanidiriq
{
	opts!.RegisterValidatorsFromAssemblyContaining<Program>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// DI lari sisteme tanitmaq
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IServiceGeneric<,>), typeof(ServiceGeneric<,>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Connectioni veririk
builder.Services.AddDbContext<AppDbContext>(options =>
{
	options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"), sqlOptions =>
	{
		// Migration-i harada etmek isteyirikse onun assemblesini yaziriq 
		sqlOptions.MigrationsAssembly("AuthServer.Data");
	});
});


// Identity-ni sisteme tanidiriq
builder.Services.AddIdentity<UserApp, IdentityRole>(opt =>
{
	// Userin, Password uzerinde deyisiklikler edirik
	opt.User.RequireUniqueEmail = true;
	opt.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<AppDbContext>()
  .AddDefaultTokenProviders();


// TokenOption elave edirik Configure-a 
// Option pattern --> DI uzerinden appsetting-deki datalari elde etmeye deyilir.
builder.Services.Configure<CustomTokenOption>(builder.Configuration.GetSection("TokenOptions"));
var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<CustomTokenOption>();
builder.Services.AddCustomTokenAuth(tokenOptions);


builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));

// Validationlari 1 yere yigib qaytaririq
builder.Services.UseCustomValidationResponse();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}
// else
// {
// 	// Prod-da gorsenmeyi lazimdi
// 	// app.UseCustomException();
// }

app.UseCustomException();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
