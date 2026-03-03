using Microsoft.Extensions.Logging;

public class AppLoggingService
{
    private readonly ILogger<AppLoggingService> _logger;

    public AppLoggingService(ILogger<AppLoggingService> logger)
    {
        _logger = logger;
    }

    public void LogAuthenticationSuccess(string userId, string clientIp)
    {
        _logger.LogInformation("Authentication successful for User {UserId} from {ClientIP}", userId, clientIp);
    }

    public void LogAuthenticationFailure(string clientIp, string reason)
    {
        _logger.LogWarning("Authentication failed from {ClientIP}. Reason: {Reason}", clientIp, reason);
    }

    public void LogRateLimitExceeded(string clientIp, string endpoint)
    {
        _logger.LogWarning("Rate limit exceeded for {ClientIP} at {Endpoint}", clientIp, endpoint);
    }

    public void LogProxyRequest(string targetUrl, string clientIp)
    {
        _logger.LogInformation("Proxy request to {TargetUrl} from {ClientIP}", targetUrl, clientIp);
    }

    public void LogApplicationStartup()
    {
        _logger.LogInformation("Application startup completed successfully");
    }

    public void LogApplicationShutdown()
    {
        _logger.LogInformation("Application shutting down");
    }
}
