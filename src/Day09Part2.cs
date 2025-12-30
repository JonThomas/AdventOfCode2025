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
            redTile.SetIsInsidePolygon(true);   // The input tiles form the polygon, so they are by definition inside it
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
                Console.WriteLine($"\tFound largest rectangle inside polygon with area {redRectangle.Area}");
                Console.WriteLine("\tNow checking if any parts of the polygon crosses any of the edges of this rectangle");
                bool rectangleIntersectsWithPolygon = edgeChecker.DoesRectangleIntersectPolygon(redRectangle);
                if(!rectangleIntersectsWithPolygon)
                {
                    Console.WriteLine($"\tThe rectangle with area {redRectangle.Area} does not intersect with the polygon, so it is fully inside it");
                    return redRectangle.Area;
                }
                else
                {
                    Console.WriteLine($"\tThe rectangle with area {redRectangle.Area} intersects with the polygon");
                }
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
    private List<RedEdge> verticalEdges;
    private List<RedEdge> horizontalEdges;

    public VerticalEdgeChecker(List<RedTile> redTiles)
    {
        verticalEdges = new List<RedEdge>();
        horizontalEdges = new List<RedEdge>();
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
                    verticalEdges.Add(new RedEdge(previousTile, redTile));
                    Console.WriteLine($"Added vertical edge between {previousTile} and {redTile}");
                }
                else
                {
                    horizontalEdges.Add(new RedEdge(previousTile, redTile));
                    Console.WriteLine($"Added horizontal edge between {previousTile} and {redTile}");
                }
            }
            previousTile = redTile;
        }
        Console.WriteLine($"Total vertical edges: {verticalEdges.Count}");
        Console.WriteLine($"Total horizontal edges: {horizontalEdges.Count}");
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

    public bool DoesRectangleIntersectPolygon(RedRectangle redRectangle)
    {
        var redTiles = redRectangle.GetAllFourCorners().ToArray();

        // TODO: This loop doesn't always find the four edges
        for(int i = 0; i < 4; i++)
        {
            RedEdge? redEdge = null;
            List<RedEdge> edgesPotentiallyCrossing = new List<RedEdge>();
            if(redTiles[i].X == redTiles[(i+1) % 4].X)
            {
                // Vertical edge
                redEdge = verticalEdges.FirstOrDefault(e => e.IsDefinedByTheseTiles(redTiles[i], redTiles[(i + 1) % 4]));
                edgesPotentiallyCrossing = horizontalEdges;
            }
            else
            {
                // Horizontal edge
                redEdge = horizontalEdges.FirstOrDefault(e => e.IsDefinedByTheseTiles(redTiles[i], redTiles[(i + 1) % 4]));
                edgesPotentiallyCrossing = verticalEdges;
            }

            if(redEdge == null)
            {
                throw new Exception($"Could not find edge defined by tiles {redTiles[i]} and {redTiles[(i + 1) % 4]}");
            }

            var crossingEdge = redEdge.CrossesAnotherEdge(edgesPotentiallyCrossing);
            if(crossingEdge != null)
            {
                Console.WriteLine($"\t\tThe edge {redEdge} of Rectangle {redRectangle} crosses edge {crossingEdge}");
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }

    internal class RedEdge
    {
        private readonly RedTile P0;
        private readonly RedTile P1;
        private bool? crossesOtherEdge;
        private RedEdge? edgeThatThisEdgeCrossesWith;
        private readonly bool isVertical;

        public RedEdge(RedTile p0, RedTile p1)
        {
            P0 = p0;
            P1 = p1;
            crossesOtherEdge = null;
            if(p0.X == p1.X)
            {
                isVertical = true;
            }
            else if(p0.Y == p1.Y)
            {
                isVertical = false;
            }
            else
            {
                throw new Exception($"Edge defined by tiles {p0} and {p1} is neither vertical nor horizontal");
            }
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

        internal bool IsDefinedByTheseTiles(RedTile t1, RedTile t2)
        {
            return (P0 == t1 && P1 == t2) || (P0 == t2 && P1 == t1);
        }

        public override string ToString()
        {
            return $"{P0} to {P1}";
        }

        internal RedEdge? CrossesAnotherEdge(List<RedEdge> edgesPotentiallyCrossing)
        {
            if(crossesOtherEdge.HasValue)
            {
                return edgeThatThisEdgeCrossesWith;
            }
            if(isVertical)
            {
                foreach(var edge in edgesPotentiallyCrossing)
                {
                    // This is a vertical edge, so edgesPotentiallyCrossing only contains horizontal edges
                    if(P0.X >= Math.Min(edge.P0.X, edge.P1.X) &&
                        P0.X <= Math.Max(edge.P0.X, edge.P1.X) &&
                        edge.P0.Y >= Math.Min(P0.Y, P1.Y) &&
                        edge.P0.Y <= Math.Max(P0.Y, P1.Y))
                    {
                        Console.WriteLine($"\t\tEdge {this} crosses edge {edge}");
                        crossesOtherEdge = true;
                        edgeThatThisEdgeCrossesWith = edge;
                        return edge;
                    }
                } 
            }
            else
            {
                // This is a horizontal edge, so edgesPotentiallyCrossing only contains vertical edges
                foreach(var edge in edgesPotentiallyCrossing)
                {
                    if(P0.X >= Math.Min(edge.P0.X, edge.P1.X) &&
                        P0.X <= Math.Max(edge.P0.X, edge.P1.X) &&
                        edge.P0.Y >= Math.Min(P0.Y, P1.Y) &&
                        edge.P0.Y <= Math.Max(P0.Y, P1.Y))
                    {
                        Console.WriteLine($"\t\tEdge {this} crosses edge {edge}");
                        crossesOtherEdge = true;
                        edgeThatThisEdgeCrossesWith = edge;
                        return edge;
                    }
                }
            }
            crossesOtherEdge = false;
            return null;
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

    internal List<RedTile> GetAllFourCorners()
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
        var isInside = IsInsideCalculationDone ? (isInsidePolygon ? " inside" : " **outside**") : "";
        return $"{X},{Y}{isInside}";
    }
}
