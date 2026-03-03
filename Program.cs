using System.Threading.RateLimiting;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options=>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
})
.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("Jwt");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        RequireExpirationTime = true,
        
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(
            System.Text.Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("key"))
        ),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddHealthChecks();
builder.Services.AddSingleton<AppLoggingService>();

builder.Services.AddRateLimiter(options =>
{
    var rateLimiterConfig = builder.Configuration.GetSection("RateLimiter:DefaultPolicy");
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();
    
    options.AddPolicy("default", context =>
    {
        var permitLimit = rateLimiterConfig.GetValue<int>("PermitLimit");
        var window = TimeSpan.Parse(rateLimiterConfig.GetValue<string>("Window"));
        var queueLimit = rateLimiterConfig.GetValue<int>("QueueLimit");
        var clientIp = context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
        
        logger.LogDebug("Rate limit check for {ClientIP}: {PermitsUsed}/{PermitLimit}", 
            clientIp, permitLimit);
        
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: clientIp,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = permitLimit,
                Window = window,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = queueLimit
            });
    });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins(builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>())
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Get logger instance
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application starting up");

app.UseAuthentication();
app.UseAuthorization();
app.UseCors();

// Add logging middleware
app.Use(async (context, next) =>
{
    logger.LogInformation("Incoming request: {Method} {Path} from {RemoteIP}", 
        context.Request.Method, 
        context.Request.Path, 
        context.Connection.RemoteIpAddress);
    
    await next();
    
    logger.LogInformation("Request completed: {StatusCode}", context.Response.StatusCode);
});

app.MapReverseProxy();
app.UseSecurityHeaders();
app.MapHealthChecks("/health");
app.UseRateLimiter();

app.Run();
