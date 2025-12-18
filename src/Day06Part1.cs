public class Day06Part1
{
    public long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day06Part1Input.txt"));

        List<int[]> allTheNumbers = ReadTheNumbers(input);
        char[] operators = ReadTheOperators(input);

        long total = 0;
        for(int i = 0; i < operators.Length; i++)
        {
            char op = operators[i];
            long rowResult = 0;
            if(op == '*')
            {
                rowResult = 1;
            }

            foreach(var rowOfNumbers in allTheNumbers)
            {
                Console.Write($"{rowOfNumbers[i]}");
                switch(op)
                {
                    case '+':
                        rowResult += rowOfNumbers[i];
                        Console.Write("+");
                        break;
                    case '*':
                        rowResult *= rowOfNumbers[i];
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
        List<int[]> allTheNumbers = new List<int[]>();
        foreach (var line in input)
        {
            var lotsOfNumberStrings = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (!int.TryParse(lotsOfNumberStrings[0], out _))
                break;

            allTheNumbers.Add(lotsOfNumberStrings.Select(s => int.Parse(s)).ToArray());
        }
        return allTheNumbers;
    }
}