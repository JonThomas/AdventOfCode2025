public class Day09Part1
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day09Part1Input.txt"));

        var redTiles = new List<RedTile>();
        foreach(var line in input)
        {
            var coordinates = line.Split(',');
            redTiles.Add(new RedTile(int.Parse(coordinates[0]), int.Parse(coordinates[1])));
        }
        var largestSquareFinder = new LargestSquareFinder(redTiles);
        return largestSquareFinder.LargestSquareArea;
    }
}

class LargestSquareFinder
{
    long largestSquareArea = 0;

    public LargestSquareFinder(List<RedTile> redTiles)
    {
        for (var i = 0; i < redTiles.Count; i++)
        {
            for (var j = i + 1; j < redTiles.Count; j++)
            {
                var tile1 = redTiles[i];
                var tile2 = redTiles[j];

                var area = (long)(Math.Abs(tile2.X - tile1.X) + 1) * (Math.Abs(tile2.Y - tile1.Y) + 1);

                //Console.WriteLine($"Considering red tiles {tile1} and {tile2} with area {area}.");

                if(area > largestSquareArea)
                {
                    largestSquareArea = area;
                    Console.WriteLine($"New largest square: Area={largestSquareArea}, ReqSquare1={tile1}, RedSquare2={tile2}");
                }   
            }
        }
    }

    public long LargestSquareArea { get { return largestSquareArea; } }
}

class RedTile
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public RedTile(int x, int y)
    {
        X = x;
        Y = y;
    }

    public override string ToString()
    {
        return $"({X},{Y})";
    }
}