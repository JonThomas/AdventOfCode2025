public class Day03Part1
{
    public int Solve()
    {
        var powerBanks = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day03Part1Input.txt"));
 
        var ret = 0;
        foreach(var powerBank in powerBanks)
        {
            (char firstDigit, int firstDigitIndex) = FindLargestCharUnlessItIsTheLast(powerBank);
            char secondDigit = FindLargestCharAfterIndex(powerBank, firstDigitIndex);
            ret += int.Parse(new string([firstDigit, secondDigit]));

            Console.WriteLine($"Power bank {powerBank} - largest digits are {firstDigit} and {secondDigit}. Partial solution = {ret}");
        }
        return ret;
    }

    private char FindLargestCharAfterIndex(string powerBank, int firstDigitIndex)
    {
        int largestDigit = 0;
        foreach(var c in powerBank[(firstDigitIndex + 1)..])
        {
            if(c > largestDigit)
            {
                largestDigit = c;
            }
        }
        return (char)largestDigit;
    }

    private (char firstDigit, int firstDigitIndex) FindLargestCharUnlessItIsTheLast(string powerBank)
    {
        int largestDigit = 0;
        int largestDigitIndex = 0;
        for(var i = 0; i < powerBank.Length - 1; i++)
        {
            if(powerBank[i] > largestDigit)
            {
                largestDigit = powerBank[i];
                largestDigitIndex = i;
            }
        }

        return ((char)largestDigit, largestDigitIndex);
    }
}
