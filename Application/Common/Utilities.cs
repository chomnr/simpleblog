namespace Application.Common;

public static class Utilities
{
    public static string GetDate(long milliseconds)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(milliseconds);

        return dateTimeOffset.ToString("dd MMM yyyy");
    }
}