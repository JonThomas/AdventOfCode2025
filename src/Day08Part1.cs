public class Day08Part1
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day08Part1Input.txt"));
        var points = new List<JunctionBox>();
        for(int i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var xyz = line.Split(',');
            points.Add(new JunctionBox(i.ToString(), int.Parse(xyz[0]), int.Parse(xyz[1]), int.Parse(xyz[2])));
        }

        // Brute force calculation of all distances between all junction boxes
        var allDistances = CalculateAllDistances(points);

        var orderedDistances = allDistances.OrderBy(d => d.Distance).ToList();

        var circuitManager = new CircuitManager();

        Console.WriteLine("All distances, shortes distances on top:");
        for(int i = 0; i < TestOrRealData(input); i++)
        {
            var dist = orderedDistances[i];
            Console.WriteLine($"{i}: From {dist.Point1} to {dist.Point2} is {dist.Distance}");
            circuitManager.Add(dist);
        }

        var (circuit1, circuit2, circuit3) = circuitManager.ThreeLargestCircuits();
        return circuit1 * circuit2 * circuit3;
    }

    private int TestOrRealData(string[] input)
    {
        if(input.Length == 20)
            return 10;
        return 1000;
    }

    public List<DistanceBetweenJunctionBoxes> CalculateAllDistances(List<JunctionBox> points)
    {  
        Console.Write($"Calculating distances between {points.Count} points ");
        var distances = new List<DistanceBetweenJunctionBoxes>();
        for(int i = 0; i < points.Count; i++)
        {
            var point = points[i];
            for(int j = i+1; j < points.Count; j++)
            {
                var otherPoint = points[j];
                if(point != otherPoint)
                {
                    distances.Add(new DistanceBetweenJunctionBoxes(point, otherPoint, DistanceBetween(point, otherPoint)));
                    Console.Write(".");
                }
            }
        }
        Console.WriteLine();
        Console.WriteLine($"Done calculating {distances.Count} distances.");
        
        return distances;
    }

    private double DistanceBetween(JunctionBox p1, JunctionBox p2)
    {
        return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
    }
}

/// <summary>
/// Organizes all junction boxes (points) into circuits
/// Junction boxes are added to the CircuitManager in ordered fashion, so that the closest boxes are added first.
/// </summary>
public class CircuitManager
{
    readonly List<Circuit> circuits;
    public CircuitManager()
    {
        circuits = new List<Circuit>();
    }

    public void Add(DistanceBetweenJunctionBoxes distance)
    {
        var circuitsToMerge = TryAddToExistingCircuits(distance);
        if(circuitsToMerge == null)
        {
            return;
        }

        var circuit1 = circuitsToMerge.Value.Item1;
        var circuit2 = circuitsToMerge.Value.Item2;

        circuit1.MergeWith(circuit2);
        circuits.Remove(circuit2);

        Console.WriteLine($"\tMerging circuits {circuit1.Id} and {circuit2.Id} into {circuit1.PrintCircuits()}");
    }

    private (Circuit, Circuit)? TryAddToExistingCircuits(DistanceBetweenJunctionBoxes distance)
    {
        foreach (var circuit in circuits)
        {
            if (circuit.Contains(distance.Point1) && circuit.Contains(distance.Point2))
            {
                // Both points are already in this circuit. Do nothing.
                return null;
            }
            if (circuit.Contains(distance.Point1))
            {
                var circuitWithPoint2 = SearchOtherCircuitsForPoint(circuit, distance.Point2);
                if (circuitWithPoint2 != null)
                {
                    return (circuit, circuitWithPoint2);
                }
                circuit.Add(distance.Point2);
                Console.WriteLine($"\tAdded {distance.Point2} to existing circuit {circuit.Id}, which now contains {circuit.PrintCircuits()}");
                return null;
            }
            if (circuit.Contains(distance.Point2))
            {
                var circuitWithPoint1 = SearchOtherCircuitsForPoint(circuit, distance.Point1);
                if (circuitWithPoint1 != null)
                {
                    return (circuit, circuitWithPoint1);
                }
                circuit.Add(distance.Point1);
                Console.WriteLine($"\tAdded {distance.Point1} to existing circuit {circuit.Id}, which now contains {circuit.PrintCircuits()}");
                return null;
            }
        }
        // Neither point is in any existing circuit. Create a new circuit.
        circuits.Add(new Circuit(circuits.Count, distance.Point1, distance.Point2));
        Console.WriteLine($"\tCreated new circuit '{circuits.Count}' with {distance.Point1} and {distance.Point2}");
        return null;
    }

    private Circuit? SearchOtherCircuitsForPoint(Circuit except, JunctionBox point)
    {
        var otherCircuits = circuits.Except(new List<Circuit> { except }).ToList();
        if(otherCircuits.Count == 0)
        {
            return null;
        }
        return otherCircuits.FirstOrDefault(c => c.Contains(point)); 
    }

    public (int, int, int) ThreeLargestCircuits()
    {
        var orderedCircuits = circuits.OrderByDescending(c => c.CircuitCount).ToList();
        Console.WriteLine("The three largest circuits are:");
        for(int i = 0; i < 3; i++)
        {
            Console.WriteLine($"\tCircuit {orderedCircuits[i].Id} containing {orderedCircuits[i].PrintCircuits()}");
        }
        return (orderedCircuits[0].CircuitCount, orderedCircuits[1].CircuitCount, orderedCircuits[2].CircuitCount);
    }

    // A Circuit is basically a list of JunctionBoxes/ Points
    public class Circuit
    {
        private int id;
        private List<JunctionBox> points;
        public Circuit(int id, JunctionBox p1, JunctionBox p2)
        {
            this.id = id;
            points = new List<JunctionBox> { p1, p2 };
        }

        public bool Contains(JunctionBox point)
        {
            return points.Contains(point);
        }

        public void Add(JunctionBox point)
        {
            points.Add(point);
        }

        public int CircuitCount { get { return points.Count; } }
        public int Id { get { return id; }}

        public string PrintCircuits()
        {
            return string.Join(",", points.Select(p => p));
        }

        public void MergeWith(Circuit other)
        {
            foreach(var point in other.points)
            {
                if(!points.Contains(point))
                {
                    points.Add(point);
                }
            }
        }
    }
}

public class DistanceBetweenJunctionBoxes
{
    private readonly JunctionBox point1;
    private readonly JunctionBox point2;
    private readonly double distance;

    public DistanceBetweenJunctionBoxes(JunctionBox point1, JunctionBox point2, double distance)
    {
        this.point1 = point1;
        this.point2 = point2;
        this.distance = distance;
    }

    public JunctionBox Point1 { get { return point1; } }
    public JunctionBox Point2 { get { return point2; } }
    public double Distance { get { return distance; } }
}

public class JunctionBox
{
    private readonly string id;
    private readonly int x;
    private readonly int y;
    private readonly int z;

    public JunctionBox(string id, int x, int y, int z)
    {
        this.id = id;
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public int X { get { return x; } }
    public int Y { get { return y; } }
    public int Z { get { return z; } }
    public string Id { get { return id; } } 

    public override string ToString()
    {
        return $"{id}";//-{x},{y},{z}";
    }
}