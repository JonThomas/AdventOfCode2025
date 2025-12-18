using Xunit;

namespace AdventOfCode2025.Tests;

public class Day01Part2Tests
{
    [Fact]
    public void Day1_DialStartsAtZero_ReturnsZero()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 1, 'L');

        Assert.Equal(0, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialStartsAtZeroAndTwoRounds_ReturnsTwo()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 204, 'L');

        Assert.Equal(2, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialStartsAtZeroAndTwoRoundsAndEndsAtZero_ReturnsThree()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 300, 'L');

        Assert.Equal(3, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialEndsAtZero_ReturnsOne()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 1, 'L');

        Assert.Equal(1, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialPassesZero_ReturnsOne()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 2, 'L');

        Assert.Equal(1, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialPassesZeroTwice_ReturnsTwo()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 102, 'L');

        Assert.Equal(2, timesTurnedPastZero);
    }
    
    [Fact]
    public void Day1_DialPassesZeroThreeTimes_ReturnsThree()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 202, 'L');

        Assert.Equal(3, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialPassesZeroThreeTimesAndEndsOnZero_ReturnsFour()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 301, 'L');

        Assert.Equal(4, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightStartsAtZero_ReturnsZero()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 1, 'R');

        Assert.Equal(0, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightStartsAtZeroAndTwoRounds_ReturnsTwo()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 204, 'R');

        Assert.Equal(2, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightStartsAtZeroAndTwoRoundsAndEndsAtZero_ReturnsThree()
    {
        var dialAt = 0;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 300, 'L');

        Assert.Equal(3, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightEndsAtZero_ReturnsOne()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 99, 'R');

        Assert.Equal(1, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightPassesZero_ReturnsOne()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 101, 'R');

        Assert.Equal(1, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightPassesZeroTwice_ReturnsTwo()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 201, 'R');

        Assert.Equal(2, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightPassesZeroThreeTimes_ReturnsThree()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 302, 'R');

        Assert.Equal(3, timesTurnedPastZero);
    }

    [Fact]
    public void Day1_DialRightPassesZeroThreeTimesAndEndsOnZero_ReturnsFour()
    {
        var dialAt = 1;
        var timesTurnedPastZero = new Day01Part2.Combination().CheckForZero(dialAt, 399, 'R');

        Assert.Equal(4, timesTurnedPastZero);
    }
}
