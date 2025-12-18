public class Day06Part2
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day06Part2Input.txt"));

        List<int[]> allTheNumbers = ReadTheNumbers(input);
        Print(allTheNumbers);
        char[] operators = ReadTheOperators(input).Reverse().ToArray();

        if(operators.Length != allTheNumbers.Count)
        {
            throw new Exception("Number of operators does not match number sequences");
        }

        long total = 0;
        
        for(int i = 0; i < operators.Length; i++)
        {
            char op = operators[i];
            long rowResult = 0;
            if(op == '*')
            {
                rowResult = 1;
            }

            foreach(int number in allTheNumbers[i])
            {
                Console.Write($"{number}");
                switch(op)
                {
                    case '+':
                        rowResult += number;
                        Console.Write("+");
                        break;
                    case '*':
                        rowResult *= number;
                        Console.Write("*");
                        break;
                    default:
                        throw new Exception("Unknown operator " + op);
                }
            }
            Console.WriteLine($" = {rowResult}");
            total += rowResult;
        }   
        return total;
    }

    private void Print(List<int[]> allTheNumbers)
    {
        Console.WriteLine("The numbers are:");
        foreach(var numberSequence in allTheNumbers)
        {
            Console.WriteLine(string.Join(" ", numberSequence));
        }
    }

    private char[] ReadTheOperators(string[] input)
    {
        char[] operators = [];
        foreach (var line in input)
        {
            var numsOrOps = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (int.TryParse(numsOrOps[0], out _))
                continue;

            operators = numsOrOps.Select(s => s[0]).ToArray();
        }
        return operators;
    }

    private static List<int[]> ReadTheNumbers(string[] input)
    {
        List<int[]> allTheNumbers = [];
        int length = input[0].Length;
        var thisNumberSequence = new List<int>();

        for(int i = length - 1; i >= 0; i--)
        {
            string numberOrEmpty = "";
            foreach(var line in input)
            {
                char c = line[i];
                if(c == '*' || c == '+')
                {
                    break;
                }
                numberOrEmpty += c;
            }

            int num;
            if(string.IsNullOrWhiteSpace(numberOrEmpty))
            {
                // Start a new sequence of numbers that shall be added or multiplied
                allTheNumbers.Add(thisNumberSequence.ToArray());
                thisNumberSequence = new List<int>();
                continue;
            }
            else if (!int.TryParse(numberOrEmpty, out num))
            {
                throw new Exception($"Could not parse number '{numberOrEmpty}'");
            }

            // Add this number to the current sequence
            thisNumberSequence.Add(num);
        }
        allTheNumbers.Add(thisNumberSequence.ToArray());
        return allTheNumbers;
    }
}