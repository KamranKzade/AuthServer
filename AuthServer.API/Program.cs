using SharedLibrary.Configuration;
using AuthServer.Core.Configuration;
using AuthServer.Core.Services;
using AuthServer.Service.Services;
using AuthServer.Core.Repositories;
using AuthServer.Data.Repositories;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data;
using Microsoft.EntityFrameworkCore;
using AuthServer.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
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

builder.Services.Configure<List<Client>>(builder.Configuration.GetSection("Clients"));




builder.Services.AddAuthentication(option =>
{
	// Sechema ni secirikki ne olacaq, eger 1 dene authserverimiz varsa bele olur, coxdursa ayri ayri qeyd etmeliyik
	option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

	// JwtDen gelen schemani, default scema ile elaqelendirdik
	option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme /* Jwtonden gelen schema */ , opts =>
{
	// Token geldikde burdaki emrlere gore dogrulamani yerine yetirecek

	opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
	{
		ValidIssuer = tokenOptions.Issuer,
		ValidAudience = tokenOptions.Audience[0],
		IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

		ValidateIssuerSigningKey = true, // imzani kontrol edirik
		ValidateAudience = true, // bizim audience di, yeni oz name-i var audience-ler icerisinde
		ValidateIssuer = true, // bizim gonderdiyimiz issuerdi,bunu yoxlayiriq
		ValidateLifetime = true, // omrunu kontrol edirik, yeni kecerli 1 tokendi ya yox	


		ClockSkew = TimeSpan.Zero // Tokene bir omur verdikde, elave olaraq 5 deyqe vaxt verir, biz burda elave vaxt vermirik ( 0 edirik )
	};


});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
