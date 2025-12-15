public static class Helpers
{
    public static int Mod(this int a, int b)
    {
        return (a % b + b) % b;
    }
}