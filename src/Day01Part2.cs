public class Day01Part2
{
    public int Solve()
    {
        var input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day01Part2Input.txt"));
        var inputLines = input.Split(['\n','\r'], StringSplitOptions.RemoveEmptyEntries);

        var combination = new Combination();
        var timesPointingAt0 = 0;
        foreach (var line in inputLines)
        {
            var direction = line[0..1];
            var ticks = int.Parse(line[1..]);
            Console.Write($"Dial at: {combination.GetDial()}. Next turn: {line}. ");
            if (direction == "L")
            {
                timesPointingAt0 += combination.TurnLeft(ticks);
            }
            else if (direction == "R")
            {
                timesPointingAt0 += combination.TurnRight(ticks);
            }
            else
            {
                throw new Exception("Invalid direction " + direction);
            }
            Console.WriteLine($"Dial at {combination.GetDial()}. Partial solution: {timesPointingAt0}");
        }
        return timesPointingAt0;
    }

    public class Combination
    {
        private int dialAt = 50;

        public int GetDial()
        {
            return dialAt;
        }

        public int TurnLeft(int ticks)
        {
            var ret = CheckForZero(dialAt, ticks, 'L');
            dialAt = SetAndNormalizeDial(dialAt, ticks, 'L');
            return ret;
        }

        public int TurnRight(int ticks)
        {
            var ret = CheckForZero(dialAt, ticks, 'R');
            dialAt = SetAndNormalizeDial(dialAt, ticks, 'R');
            return ret;
        }

        internal int CheckForZero(int dialAt, int ticks, char direction)
        {
            if(dialAt < 0 || dialAt > 99)
                throw new ArgumentException("DialAt should be between 0 and 100 " + dialAt);
            if(ticks < 1)
                throw new ArgumentException("Ticks can't be negative or 0: " + ticks);

            var dialMovedTo = direction == 'R' ? dialAt + ticks : dialAt-ticks;

            if (direction == 'R')
            {
                if(dialMovedTo < 100)
                    return 0;
                return (int)Math.Floor((double)dialMovedTo / 100);
            }
            else if(direction == 'L')
            {
                if(dialMovedTo > 0)
                    return 0;
                var turns = Math.Abs((int)Math.Ceiling((double)dialMovedTo / 100)) + 1;
                if(dialAt == 0)
                    return turns - 1;
                return turns;
            }
            throw new ArgumentException("Direction is invalid " + direction);
        }

        internal int SetAndNormalizeDial(int dialAt, int ticks, char direction)
        {
            if(direction == 'L')
            {
                dialAt -= ticks;
            }
            else
            {
                dialAt += ticks;
            }
            return Helpers.Mod(dialAt, 100);
        }
    }
} 