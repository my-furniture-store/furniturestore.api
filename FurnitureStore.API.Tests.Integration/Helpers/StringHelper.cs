namespace FurnitureStore.API.Tests.Integration.Helpers;

public static class StringHelper
{
    public static string Clamplength(this string str, int minLength, int maxLength)
    {
        if(str.Length < minLength)
            return str.PadRight(minLength);

        if(str.Length > maxLength)
            return str.Substring(0,maxLength);


        return str;
    }
}
