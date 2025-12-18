public class Day05Part1
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day05Part1Input.txt"));

        var freshIngredientsRanges = GetRanges(input);
        var ingredients = GetIngredients(input);

        int totalFreshIngredients = 0;

        foreach(var ingredient in ingredients)
        {
            if (CheckForFreshness(freshIngredientsRanges, ingredient))
            {
                totalFreshIngredients++;
                Console.WriteLine($"Ingredient {ingredient} is fresh");
            }
        }

        return totalFreshIngredients;
    }

    private static bool CheckForFreshness(List<FreshRange> freshIngredientsRanges, long ingredient)
    {
        foreach (var range in freshIngredientsRanges)
        {
            if (range.Contains(ingredient))
            {
                return true;
            }
        }

        return false;
    }

    private List<long> GetIngredients(string[] input)
    {
        List<long> ingredients = new List<long>();
        bool read = false;
        foreach(var line in input)
        {
            if(string.IsNullOrEmpty(line))
            {
                read = true;
                continue;
            }
            if(read)
            {
                ingredients.Add(long.Parse(line));
            }
        }
        return ingredients;
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

    public class FreshRange
    {
        private long Start { get; }
        private long End { get; }

        public FreshRange(long start, long end)
        {
            Start = start;
            End = end;
        }

        public bool Contains(long ingredient)
        {
            return ingredient >= Start && ingredient <= End;
        }
    }
}