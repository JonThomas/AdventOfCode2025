public static class Helpers
{
    public static int Mod(this int a, int b)
    {
        return (a % b + b) % b;
    }

    public static bool AllCharsAreSame(this string characters)
    {
        var firstChar = characters[0];
        foreach(var c in characters)
        {
            if(c != firstChar)
            {
                return false;
            }
        }
        return true;
    }
}