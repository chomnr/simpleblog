using Application.Common.Interface;
using Microsoft.AspNetCore.Http;

namespace Application.Infrastructure.Services;

public class WebHelperService : IWebHelperService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public WebHelperService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string GetHost()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.Host.ToString();
        }
        return "INVALID_URL";
    }

    public string GetPathBase()
    {
        if (_httpContextAccessor.HttpContext != null && 
            !string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.PathBase))
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return request.PathBase.ToString() + "/";
        }
        return "/";
    }
    
    public string GetBaseUrl()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            return $"{request.Scheme}://{request.Host}";
        }
        return "INVALID_BASE_URL";
    }
    
}