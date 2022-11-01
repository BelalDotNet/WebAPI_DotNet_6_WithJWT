using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI_DotNet_6_WithJWT.DBContext;
using WebAPI_DotNet_6_WithJWT.Models;

var builder = WebApplication.CreateBuilder(args);

//Added Database Connection
        var connectionString = builder.Configuration.GetConnectionString("DBCon");
        builder.Services.AddDbContextPool<JWT_DBContext>(option =>
        option.UseSqlServer(connectionString)
        );

//For Getting JWT SecretKey From AppSetting.Json For Accessing KEY Globally AND Generating Token
var _JWTSetting = builder.Configuration.GetSection("JWTSetting");
                  builder.Services.Configure<JWTSetting>(_JWTSetting);


var authkey = builder.Configuration.GetValue<string>("JWTSetting:SecretKey");

builder.Services.AddAuthentication(item =>
{
    item.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    item.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(item =>
{

    item.RequireHttpsMetadata = true;
    item.SaveToken = true;
    item.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authkey)),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();  

// Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //Added Authentication
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
