using LibraryAPI.Data;
using LibraryAPI.Middleware;
using LibraryAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Source: https://stackoverflow.com/questions/74764003/asp-net-core-6-swagger-does-not-send-recognize-the-bearer-token
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "JWTToken_Auth_API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    c.EnableAnnotations();
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<LibraryDbContext>(context =>
{
    context.UseSqlServer(builder.Configuration.GetSection("ConnectionStrings")["LibraryDbConnectionString"]);
});

builder.Services.AddScoped<LibraryService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<LibraryAnalyticsService>();
builder.Services.AddScoped<BorrowingService>();
builder.Services.AddSingleton<CacheService>();

var azureAdOptions = builder.Configuration.GetSection("AzureAd");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"{azureAdOptions["Instance"]}{azureAdOptions["TenantId"]}/v2.0";
        options.Audience = azureAdOptions["Audience"];

        options.TokenValidationParameters.ValidIssuers = new[]
        {
            $"https://sts.windows.net/{azureAdOptions["TenantId"]}/",
            $"{azureAdOptions["Instance"]}{azureAdOptions["TenantId"]}/"
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.UseMiddleware<ObjectIdMiddleware>();

app.MapControllers();

app.Run();

public partial class Program { }
