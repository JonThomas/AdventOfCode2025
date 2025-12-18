public class Day07Part1
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day07Part1Input.txt"));

        var (start, width, height) = FindKeyInformation(input);;
        Console.WriteLine($"Start position is {start}");

        // List of all splitters. Each entry in the list is a lost of all splitters on that row
        var splitterRows = FindAllSplitters(input, width, height);

        int totalSplits = 0;
        List<int> beamsOnPreviousRow = new List<int>{start};
        List<int> beamsOnThisRow = new List<int>();

        Console.WriteLine();
        Console.WriteLine("Beginning beam traversal...");

        // Go through each row of splitters ..
        for(int i = 1; i < splitterRows.Count; i++)
        {
            // .. and check if there are any beams that hit any of the splitters on that row
            foreach(var beam in beamsOnPreviousRow)
            {
                if(splitterRows[i].Contains(beam))
                {
                    beamsOnThisRow.Add(beam - 1);
                    beamsOnThisRow.Add(beam + 1);
                    totalSplits++;
                    Console.WriteLine($"Beam at {beam} on row {i} split to {beam - 1} and {beam + 1} on row {i + 1}");
                }
                else
                {
                    // Beam just continues on its way
                    beamsOnThisRow.Add(beam);
                    Console.WriteLine($"Beam at {beam} on row {i} continued");
                }
            }
            beamsOnPreviousRow = beamsOnThisRow.Distinct().ToList();
            beamsOnThisRow = new List<int>();
        }
        
        return totalSplits;
    }

    private List<List<int>> FindAllSplitters(string[] input, int width, int height)
    {
        var splitters = new List<List<int>>();
        for(int y = 1; y < height; y++)
        {
            var splittersRow = new List<int>();
            for(int x = 0; x < width; x++)
            {
                if(input[y][x] == '^')
                {
                    Console.WriteLine($"Found a splitter at ({y},{x})");
                    splittersRow.Add(x);
                }
            }
            splitters.Add(splittersRow);
        }
        return splitters;
    }

    private (int start, int width, int height) FindKeyInformation(string[] input)
    {
        int width = input[0].Length;
        int height = input.Length;
        int start = 0;

        for(int i = 0; i < width; i++)
        {
            if(input[0][i] == 'S') 
            {
                start = i;
                break;
            }
        }   
        return (start, width, height);
    }
}