namespace FoodOrderApis.Common.Helpers;

public static class RandomGenerator
{
    public static string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var value = new string(Enumerable.Range(1, length)
            .Select(_ => chars[Random.Shared.Next(chars.Length)])
            .ToArray());
        return value;
    }
}
