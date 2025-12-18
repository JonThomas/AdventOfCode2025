using Xunit;

namespace AdventOfCode2025.Tests;

public class Day05Part2Tests
{
    [Fact]
    public void Day5_TwoRangesThatDontOverlap_AllNumbersAreKept()
    {
        var nonoverlappingRanges = new Day05Part2.NonoverlappingFreshRanges();
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 3));
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(10, 11));

        Assert.Equal(5, nonoverlappingRanges.Count());
        Assert.Equal(new List<long> {1,2,3,10,11}, nonoverlappingRanges.GetFreshIngredientIds());
    }

    [Fact]
    public void Day5_Range1OverlapsWithOneNumberAtTheEnd_OverlappingNumberIsRemoved()
    {
        var nonoverlappingRanges = new Day05Part2.NonoverlappingFreshRanges();
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 3));
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(3, 5));

        Assert.Equal(5, nonoverlappingRanges.Count());
        Assert.Equal(new List<long> {1,2,3,4,5}, nonoverlappingRanges.GetFreshIngredientIds());
    }

    [Fact]
    public void Day5_Range1OverlapsWithOneNumberAtTheStart_OverlappingNumberIsRemoved()
    {
        var nonoverlappingRanges = new Day05Part2.NonoverlappingFreshRanges();
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(3, 5));
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 3));

        Assert.Equal(5, nonoverlappingRanges.Count());
        Assert.Equal(new List<long> {1,2,3,4,5}, nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }

    [Fact]
    public void Day5_Range1CompletelyOverlapsRange2_NoNumbersFromRange2IsKept()
    {
        var nonoverlappingRanges = new Day05Part2.NonoverlappingFreshRanges();
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 5));
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(4, 5));

        Assert.Equal(5, nonoverlappingRanges.Count());
        Assert.Equal(new List<long> {1,2,3,4,5}, nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }

    [Fact]
    public void Day5_Range2CompletelyOverlapsAndExtendsRange1_OnlyExtendingNumberAreKept()
    {
        var nonoverlappingRanges = new Day05Part2.NonoverlappingFreshRanges();
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 3));
        nonoverlappingRanges.AddRange(new Day05Part2.FreshRange(1, 5));

        Assert.Equal(5, nonoverlappingRanges.Count());
        Assert.Equal(new List<long> {1,2,3,4,5}, nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }

    [Fact]
    public void Day5_Range2CompletelyOverlapsAndExtendsRange1InBothDirections_TwoNewRangesAreCreated()
    {
        var ranges = new List<Day05Part2.FreshRange>{
            new Day05Part2.FreshRange(3,4),
            new Day05Part2.FreshRange(1,6)
        };

        var nonoverlappingRanges = new Day05Part2().FindAllUniqueRanges(ranges);
        
        Assert.Equal(6, nonoverlappingRanges.Count());
        Assert.Equal([1,2,3,4,5,6], nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }

    [Fact]
    public void Day5_ThreeRangesGettingProgressivelyLarger_OnlyLargestRangeIsConsidered()
    {
        var ranges = new List<Day05Part2.FreshRange>{
            new Day05Part2.FreshRange(11,12),
            new Day05Part2.FreshRange(9,14),
            new Day05Part2.FreshRange(7,16)
        };

        var nonoverlappingRanges = new Day05Part2().FindAllUniqueRanges(ranges);
        
        Assert.Equal([7,8,9,10,11,12,13,14,15,16], nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }

    [Fact]
    public void Day5_ThreeRangesGettingProgressivelyLargerInDifferentOrder_OnlyLargestRangeIsConsidered()
    {
        var ranges = new List<Day05Part2.FreshRange>{
            new Day05Part2.FreshRange(11,12),
            new Day05Part2.FreshRange(7,16),
            new Day05Part2.FreshRange(9,14)
        };

        var nonoverlappingRanges = new Day05Part2().FindAllUniqueRanges(ranges);
        
        Assert.Equal([7,8,9,10,11,12,13,14,15,16], nonoverlappingRanges.GetFreshIngredientIds().OrderBy(x => x).ToList());
    }
}