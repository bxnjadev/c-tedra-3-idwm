using System.Text;
using Catedra3.Data;
using Catedra3.Model;
using Catedra3.Service;
using Catedra3.Token;
using Catedra3.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using IAuthenticationService = Catedra3.Service.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);

var dataSource = "Data Source = app.db";

builder.Services.AddDbContext<DbContextProvider>(options =>
{
    options.UseSqlite(dataSource);
    options.EnableSensitiveDataLogging();
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IEncryptStrategy, BcryptEncryptStrategy>();
builder.Services.AddScoped<IUserTokenProvider, JwtUserTokenProvider>();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
builder.Services.AddScoped<CloudinaryImageService>();

builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IAuthenticationService, JwtAuthenticationService>();


var jwtSecret = builder.Configuration["JWT:Secret"];
if (jwtSecret == null)
{
    return;
}

Console.WriteLine(jwtSecret);

builder.Services.AddAuthentication()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();

