public class Day04Part1
{
    public long Solve()
    {
        var paperRollRows = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day04Part1Input.txt"));
 
        int paperRollRowWidth = paperRollRows[0].Length;
        int numPaperRollRows = paperRollRows.Length;
        int ret = 0;

        for(int y = 0; y < paperRollRows.Length; y++)
        {
            for(int x = 0; x < paperRollRowWidth; x++)
            {
                if(paperRollRows[y][x] != '@')
                {
                    Console.WriteLine($"({y},{x}): Skip");
                    continue;
                }

                var numNeighbours = CountNumberOfNeighbours(paperRollRows, x, y, paperRollRowWidth, numPaperRollRows);
                if(numNeighbours < 4)
                {
                    Console.WriteLine($"({y},{x}): Found paper roll with {numNeighbours} neighbours - keep");
                    ret++;
                }
                else
                {
                    Console.WriteLine($"({y},{x}): Has {numNeighbours} neighbours - skip");
                }
            }
        }
        return ret;
    }

    private int CountNumberOfNeighbours(string[] paperRollRows, int x, int y, int xMax, int yMax)
    {
        int numNeighbours = 0;
        if(y > 0)
        {
            if(x > 0)
            {
                if(paperRollRows[y-1][x-1] == '@')
                {
                    numNeighbours++;
                }
            }
            if(paperRollRows[y-1][x] == '@')
            {
                numNeighbours++;
            }
            if(x < xMax - 1)
            {
                if(paperRollRows[y-1][x+1] == '@')
                {
                    numNeighbours++;
                }
            }
        }
        if(x > 0)
        {
            if(paperRollRows[y][x-1] == '@')
            {
                numNeighbours++;
            }
        }
        if(x < xMax - 1)
        {
            if(paperRollRows[y][x+1] == '@')
            {
                numNeighbours++;
            }
        }
        if(y < yMax - 1)
        {
            if(x > 0)
            {
                if(paperRollRows[y+1][x-1] == '@')
                {
                    numNeighbours++;
                }
            }
            if(paperRollRows[y+1][x] == '@')
            {
                numNeighbours++;
            }
            if(x < xMax - 1)
            {
                if(paperRollRows[y+1][x+1] == '@')
                {
                    numNeighbours++;
                }
            }
        }
        return numNeighbours;
    }
}
