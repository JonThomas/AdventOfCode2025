using System.Net.Http.Headers;

namespace AdventOfCode2025.Day09Part2;

public class Day09Part2
{
    /// <summary>
    /// Problem: Finding the largest rectangle that will fit inside the orthogonal polygon formed by all red tiles (The red
    /// tiles are the corners)
    /// Idea: For each possible rectangle, find out if it is inside the polygon by using the Ray-Casting Algorithm (odd–even rule):
    /// If a ray drawn right from each of the corners of the rectangle towards infinity, and it only crosses an odd number of edges, 
    /// the rectangle is inside the orthogonal polygon.
    /// </summary>
    public static long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day09Part2Input.txt"));

        var redTiles = new List<RedTile>();
        foreach(var line in input)
        {
            var coordinates = line.Split(',');
            var redTile = new RedTile(int.Parse(coordinates[0]), int.Parse(coordinates[1]));
            //redTile.SetIsInsidePolygon(true);   // The input tiles form the polygon, so they are by definition inside it
            redTiles.Add(redTile);
        }

        var rectanglesSortedBySize = new RectangleFinder(redTiles).Rectangles;

        var edgeChecker = new VerticalEdgeChecker(redTiles);

        foreach(var redRectangle in rectanglesSortedBySize)
        {
            Console.WriteLine($"Rectangles sorted by size {redRectangle}");
            foreach(var redTile in redRectangle.GetAllFourCorners())
            {
                if(redTile.IsInsideCalculationDone)
                {
                    Console.WriteLine($"\tRedRectangle {redRectangle} contains RedTile {redTile}");
                    continue;
                }
                var edgesCrossed = edgeChecker.CountVerticalEdgesCrossedByRay(redTile);
                redTile.SetIsInsidePolygon(edgesCrossed % 2 != 0);
                Console.WriteLine($"\tRedRectangle {redRectangle} contains RedTile {redTile}");
            }
            if(redRectangle.IsInsidePolygon())
            {
                Console.WriteLine();
                Console.WriteLine($"\tFound largest rectangle inside polygon with area {redRectangle.Area}");
                return redRectangle.Area;
            }
        }

        foreach(var rectangle in rectanglesSortedBySize)
        {
            Console.WriteLine($"{rectangle.Tile1} and {rectangle.Tile2} has area {rectangle.Area}");
        }
        return 0;
    }
}

internal class VerticalEdgeChecker
{
    private List<Edge> verticalEdges;

    public VerticalEdgeChecker(List<RedTile> redTiles)
    {
        verticalEdges = new List<Edge>();
        RedTile? previousTile = null;
        var firstRedTile = redTiles.First();

        var fullCircle = new List<RedTile>(redTiles)
        {
            firstRedTile
        };

        // Initializing this class with all verical edges
        foreach(var redTile in fullCircle)
        {
            if(previousTile != null)
            {
                if(redTile.X == previousTile.X && redTile.Y != previousTile.Y)
                {
                    // Found a vertical edge
                    verticalEdges.Add(new Edge(previousTile, redTile));
                    Console.WriteLine($"Added vertical edge between {previousTile} and {redTile}");
                }
            }
            previousTile = redTile;
        }
        Console.WriteLine($"Total vertical edges: {verticalEdges.Count}");
    }

    public int CountVerticalEdgesCrossedByRay(RedTile tile)
    {
        // Imagining a ray going right from the tile towards infinity,
        var count = 0;
        foreach(var edge in verticalEdges)
        {
            if(edge.IsPointToTheLeft(tile))
            {
                Console.WriteLine($"\t\tRay from {tile} crosses edge between {edge}");
                count++;
            }
        }
        return count;
    }

    internal class Edge
    {
        private readonly RedTile P0;
        private readonly RedTile P1;

        public Edge(RedTile p0, RedTile p1)
        {
            P0 = p0;
            P1 = p1;
        }

        internal bool IsPointToTheLeft(RedTile tile)
        {
            if(tile.Y > Math.Max(P0.Y, P1.Y))
            {
                return false;
            }
            if(tile.Y < Math.Min(P0.Y, P1.Y))
            {
                return false;
            }
            if(tile.X > Math.Max(P0.X, P1.X))
            {
                return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"{P0} to {P1}";
        }
    }
}

class RectangleFinder
{
    readonly List<RedRectangle> redRectangles;

    public RectangleFinder(List<RedTile> redTiles)
    {
        redRectangles = new List<RedRectangle>();

        for (var i = 0; i < redTiles.Count; i++)
        {
            for (var j = i + 1; j < redTiles.Count; j++)
            {
                var tile1 = redTiles[i];
                var tile2 = redTiles[j];

                redRectangles.Add(new RedRectangle(tile1, tile2));
            }
        }
    }

    public List<RedRectangle> Rectangles { get { return redRectangles.OrderByDescending(rs => rs.Area).ToList(); } }
}

internal class RedRectangle
{
    public RedTile Tile1 { get; private set; }
    public RedTile Tile2 { get; private set; }
    public RedTile Tile3 { get; private set; }
    public RedTile Tile4 { get; private set; }
    public long Area { get; private set; }  

    public RedRectangle(RedTile tile1, RedTile tile2)
    {
        Tile1 = tile1;
        Tile2 = tile2;

        // Add the other two corners: (x2, y1) and (x1, y2)
        Tile3 = new RedTile(tile2.X, tile1.Y);
        Tile4 = new RedTile(tile1.X, tile2.Y);

        Area = (long)(Math.Abs(tile2.X - tile1.X) + 1) * (Math.Abs(tile2.Y - tile1.Y) + 1);;
    }

    internal IEnumerable<RedTile> GetAllFourCorners()
    {
        return new List<RedTile>{ Tile1, Tile2, Tile3, Tile4 };
    }

    internal bool IsInsidePolygon()
    {
        return GetAllFourCorners().All(t => t.IsInsidePolygon);
    }

    public override string ToString()
    {
        return $"Area: {Area}. Tile1: {Tile1}. Tile2: {Tile2}";
    }
}

internal class RedTile
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private bool isInsidePolygon;
    private bool insideCalculationDone;

    public RedTile(int x, int y)
    {
        X = x;
        Y = y;
        isInsidePolygon = false;
        insideCalculationDone = false;
    }

    public void SetIsInsidePolygon(bool inside)
    {
        isInsidePolygon = inside;
        insideCalculationDone = true;
    }

    public bool IsInsidePolygon { get { return isInsidePolygon; } }

    public bool IsInsideCalculationDone { get { return insideCalculationDone; } }

    public override string ToString()
    {
        var isInside = IsInsideCalculationDone ? (isInsidePolygon ? " inside" : " outside") : "";
        return $"{X},{Y}{isInside}";
    }
}
