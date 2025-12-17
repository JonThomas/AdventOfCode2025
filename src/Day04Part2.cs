public class Day04Part2
{
    public long Solve()
    {
        var paperRollRowsString = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day04Part2Input.txt"));
        char[][] paperRollRows = paperRollRowsString.Select(row => row.ToCharArray()).ToArray();

        int paperRollRowWidth = paperRollRows[0].Length;
        int numPaperRollRows = paperRollRows.Length;
        int ret = 0;
        while (true)
        {
            (int removedPaperRolls, paperRollRows) = FindAllPaperRolls(paperRollRows, paperRollRowWidth, numPaperRollRows);
            if (removedPaperRolls == 0)
            {
                return ret;
            }
            ret += removedPaperRolls;
        }
    }

    private static (int, char[][]) FindAllPaperRolls(char[][] paperRollRows, int paperRollRowWidth, int numPaperRollRows)
    {
        int ret = 0;
        char[][] nextRoundOfPaperRollRows = new char[numPaperRollRows][];
        for(int i = 0; i < nextRoundOfPaperRollRows.Length; i++)
        {
            nextRoundOfPaperRollRows[i] = new char[paperRollRowWidth];
        }

        for (int y = 0; y < paperRollRows.Length; y++)
        {
            for (int x = 0; x < paperRollRowWidth; x++)
            {
                if (paperRollRows[y][x] != '@')
                {
                    Console.WriteLine($"({y},{x}): Skip");
                    nextRoundOfPaperRollRows[y][x] = '.';
                    continue;
                }

                var numNeighbours = CountNumberOfNeighbours(paperRollRows, x, y, paperRollRowWidth, numPaperRollRows);
                if (numNeighbours < 4)
                {
                    Console.WriteLine($"({y},{x}): Found paper roll with {numNeighbours} neighbours - keep");
                    nextRoundOfPaperRollRows[y][x] = '.';
                    ret++;
                }
                else
                {
                    Console.WriteLine($"({y},{x}): Has {numNeighbours} neighbours - skip");
                    nextRoundOfPaperRollRows[y][x] = '@';
                }
            }
        }
        return (ret, nextRoundOfPaperRollRows);
    }

    private static int CountNumberOfNeighbours(char[][] paperRollRows, int x, int y, int xMax, int yMax)
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