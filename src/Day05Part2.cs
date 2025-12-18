public class Day05Part2
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day05Part2Input.txt"));

        var freshIngredientsRanges = GetRanges(input);

        var nonoverlappingRanges = FindAllUniqueRanges(freshIngredientsRanges);

        return nonoverlappingRanges.Count();
    }

    internal NonoverlappingFreshRanges FindAllUniqueRanges(List<FreshRange> freshIngredientsRanges)
    {
        // My idea was to keep a list of unique ranges, by removing overlapping ingredientsIds as we go.
        // But when a range completely overlaps a previous range, and extends it in both directions, the idea broke down.
        // I solved it by restarting the process, but moving the range that caused the problem to the top of the list.
        while (true)
        {
            bool done = TryFindCompleteFreshIngredientsRanges(freshIngredientsRanges, out var nonoverlappingRanges, out var rangeThatWouldSplit);
            if (done)
            {
                return nonoverlappingRanges;
            }
            // Adding the range that would split to the top of the list, and try again
            freshIngredientsRanges = AddLastRangeToTop(freshIngredientsRanges!, rangeThatWouldSplit!);
        }
    }

    private List<FreshRange> AddLastRangeToTop(List<FreshRange> input, FreshRange rangeThatWouldSplit)
    {
        var ret = new List<FreshRange>{rangeThatWouldSplit};
        foreach(var range in input)
        {
            if(range != rangeThatWouldSplit)
            {
                ret.Add(range);
            }
        }
        return ret;
    }

    private static bool TryFindCompleteFreshIngredientsRanges(List<FreshRange> freshIngredientsRanges, out NonoverlappingFreshRanges nonoverlappingRanges, out FreshRange? rangeThatWouldSplit)
    {
        rangeThatWouldSplit = null;
        nonoverlappingRanges = new NonoverlappingFreshRanges();
        foreach (var range in freshIngredientsRanges)
        {
            Console.Write($"Adding range {range}");
            if (!nonoverlappingRanges.AddRange(range))
            {
                rangeThatWouldSplit = range;
                return false;
            }
        }
        return true;
    }

    private List<FreshRange> GetRanges(string[] input)
    {
        var ranges = new List<FreshRange>();
        foreach(var line in input)
        {
            if(string.IsNullOrEmpty(line))
            {
                break;
            }

            var parts = line.Split('-');
            long start = long.Parse(parts[0]);
            long end = long.Parse(parts[1]);
            ranges.Add(new FreshRange(start, end));
        }
        return ranges;
    }

    /// <summary>
    /// Holds a list of non-overlapping ranges of fresh ingredient IDs, and makes sure that new ranges don't overlap with existing ranges by removing overlapping IDs from the new range.
    /// </summary>
    public class NonoverlappingFreshRanges
    {
        private List<FreshRange> Ranges = new List<FreshRange>();

        public bool AddRange(FreshRange range)
        {
            foreach(var nonoverlappingRange in Ranges)
            {
                if(nonoverlappingRange.OverlapsInBothDirections(range))
                {
                    return false;
                }
                else if(nonoverlappingRange.Overlaps(range))
                {
                    range.ReRange(nonoverlappingRange);
                }
            }
            Console.WriteLine();
            if(range.IsValid())
            {
                Ranges.Add(range);
            }
            return true;
        }

        public long Count()
        {
            long total = 0;
            foreach(var range in Ranges)
            {
                total += range.GetLength();
            }
            return total;
        }

        public List<long> GetFreshIngredientIds()
        {
            var ret = new List<long>();
            foreach(var range in Ranges)
            {
                ret.AddRange(range.GetFreshIngredientIds());
            }
            return ret;
        }
    }

    /// <summary>
    /// A range of fresh ingredient IDs.
    /// All IDs between Start and End, inclusive, are fresh, so if Start = 5 and End = 10, the fresh IDs are 5,6,7,8,9,10.
    /// A FreshRange can be re-ranged to remove overlapping IDs with another FreshRange. This is done by narrowing the Start or End values using the ReRange method.
    /// </summary>
    public class FreshRange
    {
        private long Start;
        private long End;

        public FreshRange(long start, long end)
        {
            Start = start;
            End = end;
        }

        public bool Overlaps(FreshRange other)
        {
            return other.Start <= End && other.End >= Start;
        }

        internal bool OverlapsInBothDirections(FreshRange other)
        {
            if(other.Start < Start && other.End > End)
            {
                return true;
            }
            return false;
        }

        public FreshRange ReRange(FreshRange nonOverlappingRange)
        {
            if(Start >= nonOverlappingRange.Start && End <= nonOverlappingRange.End)
            {
                Start = 0;
                End = 0;
            }
            if(Start < nonOverlappingRange.Start && End > nonOverlappingRange.End)
            {
                throw new InvalidOperationException("This range is split - cannot re-range");
            }
            if(Start >= nonOverlappingRange.Start && nonOverlappingRange.End >= Start)
            {
                Start = Math.Min(nonOverlappingRange.End + 1, End);
            }
            if(End <= nonOverlappingRange.End && nonOverlappingRange.Start <= End)
            {
                End = Math.Max(nonOverlappingRange.Start - 1, Start);
            }
            Console.Write($" Re-ranged to {this}");
            return this;
        }

        public bool IsValid()
        {
            return End != 0 && Start != 0;
        }

        public long GetLength()
        {
            return End - Start + 1;
        }

        public List<long> GetFreshIngredientIds()
        {
            var ret = new List<long>();
            for(long i = Start; i <= End; i++)
            {
                ret.Add(i);
            }
            return ret;
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }
}