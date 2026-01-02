using AdventOfCode2025.Day10Part2;

namespace AdventOfCode2025.Tests;

public class Day10Part2Tests
{
    [Fact]
    public void Day10_PressUntilSolved_SolvedByPressingFirstButtonThreeTimes()
    {
        var buttons = new Buttons(
            new RequiredJoltageLevels(new List<int> { 3, 3, 0 }),
            new List<Button>
            {
                new Button(new List<int> { 0, 1 }),
                new Button(new List<int> { 1, 2 }),
            });

        var solution = buttons.PressUntilSolved();
        Assert.Equal(3, solution);  
    }
    [Fact]
    public void Day10_PressUntilSolved_DoesNotCrash()
    {
        var buttons = new Buttons(
            new RequiredJoltageLevels(new List<int> { 7,5,1,2,7,2 }),
            new List<Button>
            {
                new Button(new List<int> { 0,2,3,4 }),
                new Button(new List<int> { 2,3 }),
                new Button(new List<int> { 0,4 }),
                new Button(new List<int> { 0,1,2 }),
                new Button(new List<int> { 1,2,3,4 })
            });

        var solution = buttons.PressUntilSolved();
        Assert.Equal(3, solution);  
    }
}