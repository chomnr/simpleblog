namespace Application.Common;

public static class PostHelper
{
    public static string GetDate(long milliseconds)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);

        return dateTimeOffset.ToString("dd MMM yyyy");
    }
}