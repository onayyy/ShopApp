using Application.Behaviors;
using Application.Common.Interfaces;
using Application.Common.Interfaces.RedisCache;
using Application.Common.Interfaces.Tokens;
using Application.Services;
using Infrastructure;
using Infrastructure.RedisCache;
using Infrastructure.Tokens;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using System.Text.Json.Serialization;

/*var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddHttpClient<CurrencyService>();
builder.Services.AddTransient<CurrencyService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IPizzaAppDbContext, PizzaAppDbContext>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(IPizzaAppDbContext).Assembly));

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddTransient<ITokenService, TokenService>();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCacheBehaviour<,>));
builder.Services.AddTransient<IRedisCacheService, RedisCacheService>();

builder.Services.Configure<RedisCacheSettings>(builder.Configuration.GetSection("RedisCacheSettings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Unauthorized");
        },
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Unauthorized");
        }
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheSettings:ConnectionString"];
    options.InstanceName = builder.Configuration["RedisCacheSettings:InstanceName"];
});

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddControllersWithViews()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);


builder.Services.AddHttpClient<CurrencyService>();
builder.Services.AddTransient<CurrencyService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<IPizzaAppDbContext, PizzaAppDbContext>();
builder.Services.AddMediatR(c => c.RegisterServicesFromAssembly(typeof(IPizzaAppDbContext).Assembly));

builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RedisCacheBehaviour<,>));
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

builder.Services.Configure<RedisCacheSettings>(builder.Configuration.GetSection("RedisCacheSettings"));
builder.Services.Configure<TokenSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.NoResult();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Unauthorized");
            }
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            if (!context.Response.HasStarted)
            {
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "text/plain";
                return context.Response.WriteAsync("Unauthorized");
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["RedisCacheSettings:ConnectionString"];
    options.InstanceName = builder.Configuration["RedisCacheSettings:InstanceName"];
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
