public class Day02Part2
{
    public long Solve()
    {
        var input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day02Part2Input.txt"));
        var inputRanges = input.Split([','], StringSplitOptions.RemoveEmptyEntries);
        List<Range> ranges = new List<Range>();
        foreach (var inputRange in inputRanges)
        {
            var bounds = inputRange.Split(['-'], StringSplitOptions.RemoveEmptyEntries);
            var start = long.Parse(bounds[0]);
            var end = long.Parse(bounds[1]);
            ranges.Add(new Range(start, end));
        }

        var invalidProductIds = new List<long>();

        foreach (var range in ranges)
        {
            foreach(var potentialValidProductId in range)
            {
                string prodIdString = potentialValidProductId.ToString();
                if (ContainsRepeatedSubstrings(prodIdString))
                {
                    invalidProductIds.Add(potentialValidProductId);
                    continue;
                }
            }
        }

        return invalidProductIds.Sum();
    }

    internal static bool ContainsRepeatedSubstrings(string prodIdString)
    {
        if(prodIdString.AllCharsAreSame())
        {
            Console.WriteLine($"Found invalid product ID: {prodIdString} with pattern {prodIdString[..1]}");
            return true;
        }

        // Loop to check first the if two chars are repeated, then three, ... 
        for (int repeatedCharLength = 2; repeatedCharLength <= prodIdString.Length / 2; repeatedCharLength++)
        {
            // Loop to check if the substring repeats throughout the string
            if(CheckForRepeatedSubstringsWithThisLength(prodIdString, repeatedCharLength))
            {
                Console.WriteLine($"Found invalid product ID: {prodIdString} with pattern {prodIdString[..repeatedCharLength]}");
                return true;
            }
        }
        return false;
    }

    private static bool CheckForRepeatedSubstringsWithThisLength(string prodIdString, int repeatedCharLength)
    {
        if(prodIdString.Length % repeatedCharLength != 0)
        {
            return false;
        }
        var firstRepeat = prodIdString[..repeatedCharLength];
        for (int i = 0 + repeatedCharLength; i <= prodIdString.Length - repeatedCharLength; i += repeatedCharLength)
        {
            if (firstRepeat != prodIdString[i..(i + repeatedCharLength)])
            {
                return false;
            }
        }
        return true;
    }

    public class Range : IEnumerable<long>
    {
        private long _start;
        private long _end;
        private List<long> theRange = new List<long>();

        public Range(long start, long end)
        {
            _start = start;
            _end = end;
            theRange = RemoveAsManyAsPossible(start, end);
        }

        private List<long> RemoveAsManyAsPossible(long start, long end)
        {
            var ret = new List<long>();
            for (long i = start; i <= end; i++)
            {
                var iLength = i.ToString().Length;

                // Keep only numbers that can have a repeaded substring, or only contains the same chars
                if(iLength < 2)
                {
                    continue;
                }
                else if(iLength % 2 == 0 || iLength == 9 || iLength == 15)
                {
                    ret.Add(i);
                }
                else if(i.ToString().AllCharsAreSame())
                {
                    ret.Add(i);
                }
            }
            return ret;
        }

        public IEnumerator<long> GetEnumerator()
        {
            foreach(var item in theRange)
            {
                yield return item;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return $"{_start}-{_end}";
        }   
    }
}
