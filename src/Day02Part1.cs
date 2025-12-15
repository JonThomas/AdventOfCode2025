public class Day02Part1
{
    public long Solve()
    {
        var input = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day02Part1Input.txt"));
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
                var firstHalf = prodIdString[..(prodIdString.Length / 2)];
                var secondHalf = prodIdString[(prodIdString.Length / 2)..];
                if(firstHalf == secondHalf)
                {
                    invalidProductIds.Add(potentialValidProductId);
                    Console.WriteLine($"Found invalid product ID: {potentialValidProductId} in range {range}");
                }
            }
        }

        return invalidProductIds.Sum();
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
                if(iLength % 2 == 0)
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
