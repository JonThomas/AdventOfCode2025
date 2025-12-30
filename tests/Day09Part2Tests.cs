using AdventOfCode2025.Day09Part2;

namespace AdventOfCode2025.Tests;

public class Day09Part2Tests
{
    [Fact]
    public void Day9_NewRectangles_ContainAllCorners()
    {
        RedRectangle rectangle = new RedRectangle(new Day09Part2.RedTile(1,2), new Day09Part2.RedTile(4,5));

        var corners = rectangle.GetAllFourCorners();

        Assert.Contains(corners, t => t.X == 1 && t.Y == 2);
        Assert.Contains(corners, t => t.X == 4 && t.Y == 5);
        Assert.Contains(corners, t => t.X == 1 && t.Y == 5);
        Assert.Contains(corners, t => t.X == 4 && t.Y == 2);
    }

    [Fact]
    public void Day9_NewRectanglesWithWidthOne_ContainAllCorners()
    {
        RedRectangle rectangle = new RedRectangle(new Day09Part2.RedTile(1,2), new Day09Part2.RedTile(1,8));

        var corners = rectangle.GetAllFourCorners();

        Assert.Contains(corners, t => t.X == 1 && t.Y == 2);
        Assert.Contains(corners, t => t.X == 1 && t.Y == 8);
        Assert.Equal(2, corners.Count(c => c.X == 1 && c.Y == 2));
        Assert.Equal(2, corners.Count(c => c.X == 1 && c.Y == 8));
    }

    [Fact]
    public void Day9_CheckNumberOfEdgesCrossed_PointOutsideSinceItCrossesTwoEdges()
    {
        List<Day09Part2.RedTile> redTiles = [new(9,5), new(9,7), new(11,1), new(11,7)];

        VerticalEdgeChecker edgeChecker = new(redTiles);
        var numberOfEdgesCrossed = edgeChecker.CountVerticalEdgesCrossedByRay(new(7,7));

        Assert.Equal(2, numberOfEdgesCrossed);
    }

    [Fact]
    public void Day9_CheckNumberOfEdgesCrossed_PointOutsideSinceItCrossesTwoEdgesOnTop()
    {
        List<Day09Part2.RedTile> redTiles = [new(7,1), new(7,3), new(11,1), new(11,7)];

        VerticalEdgeChecker edgeChecker = new(redTiles);
        var numberOfEdgesCrossed = edgeChecker.CountVerticalEdgesCrossedByRay(new(2,1));

        Assert.Equal(2, numberOfEdgesCrossed);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(1, 4)]
    [InlineData(1, 5)]
    [InlineData(3, 3)]
    [InlineData(3, 4)]
    [InlineData(3, 5)]
    public void Day9_IsPointToTheLeft_AllPointsAreLeftOfThisEdge(int x, int y)
    {
        var edge = new VerticalEdgeChecker.RedEdge(new(3,3), new(3,5));
        Assert.True(edge.IsPointToTheLeft(new(x,y)));
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(1, 2)]
    [InlineData(1, 6)]
    [InlineData(1, 7)]
    [InlineData(2, 1)]
    [InlineData(2, 2)]
    [InlineData(2, 6)]
    [InlineData(2, 7)]
    public void Day9_IsPointToTheLeft_PointsAreLeftButOverOrUnderTheEdge(int x, int y)
    {
        var edge = new VerticalEdgeChecker.RedEdge(new(3,3), new(3,5));
        Assert.False(edge.IsPointToTheLeft(new(x,y)));
    }

    [Theory]
    [InlineData(3, 1)]
    [InlineData(3, 2)]
    [InlineData(3, 6)]
    [InlineData(3, 7)]
    public void Day9_IsPointToTheLeft_PointsAreDirectlyOverOrUnderThisEdge(int x, int y)
    {
        var edge = new VerticalEdgeChecker.RedEdge(new(3,3), new(3,5));
        Assert.False(edge.IsPointToTheLeft(new(x,y)));
    }

    [Theory]
    [InlineData(4, 1)]
    [InlineData(4, 2)]
    [InlineData(4, 3)]
    [InlineData(4, 4)]
    [InlineData(4, 5)]
    [InlineData(4, 6)]
    [InlineData(4, 7)]
    [InlineData(5, 1)]
    [InlineData(5, 2)]
    [InlineData(5, 3)]
    [InlineData(5, 4)]
    [InlineData(5, 5)]
    [InlineData(5, 6)]
    [InlineData(5, 7)]
    public void Day9_IsPointToTheLeft_PointsAreRightOfTheEdge(int x, int y)
    {
        var edge = new VerticalEdgeChecker.RedEdge(new(3,3), new(3,5));
        Assert.False(edge.IsPointToTheLeft(new(x,y)));
    }
}