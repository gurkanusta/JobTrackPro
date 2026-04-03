
using System.Reflection;
using System.Text;

using FluentValidation;

using JobTrackPro.Application.Common.Behaviors;
using JobTrackPro.Application.Common.Interfaces;
using JobTrackPro.Infrastructure.Identity;
using JobTrackPro.Infrastructure.Persistence;

using MediatR;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;


Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console(new CompactJsonFormatter())
    .WriteTo.File(
        path: "logs/jobtrackpro-.log",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequiredLength = 8;

    
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>() 
.AddDefaultTokenProviders();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
                                       Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero 
    };
});

builder.Services.AddAuthorization();


builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();


builder.Services.AddMediatR(typeof(JobTrackPro.Application.AssemblyReference).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(JobTrackPro.Application.AssemblyReference).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "JobTrackPro API", Version = "v1" });

    
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGci..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id   = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseMiddleware<JobTrackPro.Api.Middleware.ExceptionHandlingMiddleware>();

app.UseDefaultFiles();
app.UseStaticFiles();

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