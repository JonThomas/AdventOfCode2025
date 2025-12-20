public class Day07Part2
{
    public long Solve()
    {
        // Idea: Model the beams as edges, and the splitters as nodes in a graph.
        // Then traverse the graph recursively, counting all possible ways to reach a leaf node from the start node.

        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day07Part2Input.txt"));

        var (start, width, height) = FindKeyInformation(input); ;

        // List of all splitters. Each entry in the list is a list of all splitters on that row
        var splitterRows = FindAllSplitters(input, width, height);

        Console.WriteLine();
        Console.WriteLine("Building the Splitter-graph...");

        var splitters = CreateSplitters(start, splitterRows);
        Splitter? splitterParent = null;

        BuildSplitterGraph(ref splitters);

        foreach(var splitter in splitters)
        {
            Console.WriteLine(splitter);
            if(splitter.GetY() == 1)
            {
                splitterParent = splitter;
            }
        }

        long pathCount = 0;
        long totalPaths = TraverseGraf(splitterParent!, ref pathCount);

        Console.WriteLine();
        Console.WriteLine($"Total paths through the splitter graph: {totalPaths} - {pathCount}");

        return pathCount;
    }

    private void BuildSplitterGraph(ref List<Splitter> splitters)
    {
        foreach(var splitter in splitters)
        {
            var leftChild = splitters.Where(s => s.GetX() == splitter.GetX() - 1 && s.GetY() > splitter.GetY()).MinBy(s => s.GetY());
            if(leftChild != null)
            {
                splitter.AddLeftChild(leftChild);
            }
            var rightChild = splitters.Where(s => s.GetX() == splitter.GetX() + 1 && s.GetY() > splitter.GetY()).MinBy(s => s.GetY());
            if(rightChild != null)
            {
                splitter.AddRightChild(rightChild);
            }
        }
    }

    private long TraverseGraf(Splitter splitter, ref long pathsSoFar)
    {
        var leftChild = splitter.GetLeftChild();
        if(leftChild == null)
        {
            // Reached an end node
            pathsSoFar++;
            //Console.WriteLine($"Reached left leaf node at {splitter}. Total paths so far: {pathsSoFar}");
        }
        else if(leftChild != null)
        {
            pathsSoFar = TraverseGraf(leftChild, ref pathsSoFar);
        }

        var rightChild = splitter.GetRightChild();
        if(rightChild == null)
        {
            // Reached an end node
            pathsSoFar++;
            //Console.WriteLine($"Reached right leaf node at {splitter}. Total paths so far: {pathsSoFar}");
        }
        else if(rightChild != null)
        {
            pathsSoFar = TraverseGraf(rightChild, ref pathsSoFar);
        }
        if(pathsSoFar % 10000000 == 0)
        {
            Console.WriteLine($"{pathsSoFar}. Processing ({splitter.GetX()},{splitter.GetY()})");
        }
        return pathsSoFar;
    }

    private List<Splitter> CreateSplitters(int start, List<List<int>> splitterRows)
    {
        List<Splitter> splitters = new List<Splitter>();
        List<int> beamsOnPreviousRow = new List<int> { start };
        List<int> beamsOnThisRow = new List<int>();

        // Go through each row of splitters ..
        for (int i = 1; i < splitterRows.Count; i++)
        {
            // .. and check if there are any beams that hit any of the splitters on that row
            foreach (var beam in beamsOnPreviousRow)
            {
                if (splitterRows[i].Contains(beam))
                {
                    splitters.Add(new Splitter(beam, i));
                    beamsOnThisRow.Add(beam - 1);
                    beamsOnThisRow.Add(beam + 1);
                }
                else
                {
                    // Beam just continues on its way
                    beamsOnThisRow.Add(beam);
                }
            }
            beamsOnPreviousRow = beamsOnThisRow.Distinct().ToList();
            beamsOnThisRow = new List<int>();
        }
        return splitters;
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

    public class Splitter
    {
        private Splitter? leftChild;
        private Splitter? rightChild;
        private readonly int x;
        private readonly int y;

        public Splitter(int x, int y)
        {
            this.x = x;
            this.y = y;            
        }

        public void AddLeftChild(Splitter splitter)
        {
            if(leftChild != null)
            {
                throw new Exception($"Trying to add {splitter} as left child to {this}, but it is already set by {leftChild}");
            }
            if(splitter.GetY() < y)
            {
                throw new Exception($"Trying to add {splitter} as left child to {this}, but its Y is smaller than parent's Y");
            }
            leftChild = splitter;
        }

        public void AddRightChild(Splitter splitter)
        {
            if(rightChild != null)
            {
                throw new Exception($"Trying to add {splitter} as right child to {this}, but it is already set by {rightChild}");
            }
            if(splitter.GetY() < y)
            {
                throw new Exception($"Trying to add {splitter} as right child to {this}, but its Y is smaller than parent's Y");
            }
            rightChild = splitter;
        }

        public int GetX()
        {
            return x;
        }

        public int GetY()
        {
            return y;
        }

        public Splitter? GetLeftChild()
        {
            return leftChild;
        }

        public Splitter? GetRightChild()
        {
            return rightChild;
        }

        public override string ToString()
        {
            string left = leftChild == null ? "null" : $"({leftChild.GetX()},{leftChild.GetY()})";
            string right = rightChild == null ? "null" : $"({rightChild.GetX()},{rightChild.GetY()})";
            return $"Splitter at ({x},{y}), with LeftChild: {left}, RightChild: {right}";
        }
    }
}