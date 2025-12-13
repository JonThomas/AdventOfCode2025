using System.Runtime.InteropServices.Marshalling;
using System.Security.Principal;

public class Day01Part1
{
    public int Solve()
    {
        var input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day01Part1Input.txt"));
        var inputLines = input.Split(['\n','\r'], StringSplitOptions.RemoveEmptyEntries);

        var combination = new Combination();
        var timesPointingAt0 = 0;
        foreach (var line in inputLines)
        {
            var direction = line[0..1];
            var ticks = int.Parse(line[1..]);
            if (direction == "L")
            {
                timesPointingAt0 += combination.TurnLeft(ticks) ? 1 : 0;
            }
            else if (direction == "R")
            {
                timesPointingAt0 += combination.TurnRight(ticks) ? 1 : 0;
            }
            else
            {
                throw new Exception("Invalid direction " + direction);
            }
            Console.WriteLine($"Dial at {combination.GetDial()}, after {line}. Times at 0: {timesPointingAt0}"); 
        }
        return timesPointingAt0;
    }

    private class Combination
    {
        private int dialAt = 50;

        public int GetDial()
        {
            return dialAt;
        }

        public bool TurnLeft(int ticks)
        {
            dialAt -= ticks;
            return NormalizeDialAndCheckForZero(ref dialAt);
        }

        public bool TurnRight(int ticks)
        {
            dialAt += ticks;
            return NormalizeDialAndCheckForZero(ref dialAt);
        }
        
        private bool NormalizeDialAndCheckForZero(ref int dialAt)
        {
            dialAt %= 100;
            return dialAt == 0;
        }
    }
}