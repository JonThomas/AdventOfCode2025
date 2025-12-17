public class Day03Part2
{
    public long Solve()
    {
        var powerBanks = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day03Part2Input.txt"));
 
        long ret = 0;
        foreach(var powerBank in powerBanks)
        {
            string joltage = string.Empty;
            int previousDigitIndex = 0;
            for(int n = 11; n >= 0; n--)
            {
                (char digit, previousDigitIndex) = FindLargestCharBeforeIndex(powerBank, previousDigitIndex, n);
                joltage += digit;
            }
            ret += long.Parse(joltage);
            Console.WriteLine($"Found digits {joltage}. Partial solution = {ret}. Powerbank: {powerBank}");
        }
        return ret;
    }

    private (char, int) FindLargestCharBeforeIndex(string powerBank, int startIndex, int lastIndex)
    {
        int largestDigit = 0;
        int largestDigitIndex = 0;
        int powerBankLength = powerBank.Length;
        for(var i = startIndex; i < powerBankLength - lastIndex; i++)
        {
            if(powerBank[i] > largestDigit)
            {
                largestDigit = powerBank[i];
                largestDigitIndex = i;
            }
        }

        return ((char)largestDigit, largestDigitIndex + 1);
    }
}
