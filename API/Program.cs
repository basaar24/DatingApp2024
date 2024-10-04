using System.Text;
using API.Data;
using API.Extensions;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
// Configure the HTTP request pipeline.
app.UseCors((cors) => cors.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200","https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
