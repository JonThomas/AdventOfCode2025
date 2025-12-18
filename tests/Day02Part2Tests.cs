using Xunit;

namespace AdventOfCode2025.Tests;

public class Day02Part2Tests
{
    [Fact]
    public void Day2_Check1000ForRepeatedChars_False()
    {
        var containsRepeatedChars = Day02Part2.ContainsRepeatedSubstrings(1000.ToString());
        Assert.False(containsRepeatedChars);
    }

    [Fact]
    public void Day2_Check2121212118ForRepeatedChars_False()
    {
        var containsRepeatedChars = Day02Part2.ContainsRepeatedSubstrings(2121212118.ToString());
        Assert.False(containsRepeatedChars);
    }

    [Fact]
    public void Day2_Check2121212120ForRepeatedChars_False()
    {
        var containsRepeatedChars = Day02Part2.ContainsRepeatedSubstrings(2121212120.ToString());
        Assert.False(containsRepeatedChars);
    }

    [Fact]
    public void Day2_Check22222ForRepeatedChars_True()
    {
        var containsRepeatedChars = Day02Part2.ContainsRepeatedSubstrings(22222.ToString());
        Assert.True(containsRepeatedChars);
    }

    [Fact]
    public void Day2_Range111To111_ContainsOneSubstringSinceAllCharsAreSame()
    {
        Day02Part2.Range range = new Day02Part2.Range(111, 111);
        foreach(var item in range)
        {
            Assert.Equal(111, item);
        }
    }
}
