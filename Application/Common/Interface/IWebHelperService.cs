namespace Application.Common.Interface;

public interface IWebHelperService
{
    string GetHost();
    string GetPathBase();
    string GetBaseUrl();
}