namespace AdventOfCode2025.Day10Part2;

public class Day10Part2
{
    /// <summary>
    /// My idea for part 2:
    /// Use same method as in part 1.
    /// Since this is going to be slow, I will try to optimize the code a bit.
    /// </summary>
    public static long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day10Part2Input.txt"));
        var machines = ParseInput(input);

        var solution = 0;
        foreach(var machine in machines)
        {
            Console.WriteLine($"Parsed machine {machine}");
            solution += machine.Solve();
        }

        return solution;
    }

    private static List<Machine> ParseInput(string[] input)
    {
        var machines = new List<Machine>();
        for(int i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var requiredJoltageLevels = ParseJoltageLevels(line);
            var buttons = ParseButtons(requiredJoltageLevels, line);
            var machine = new Machine(i, requiredJoltageLevels, buttons);
            machines.Add(machine);
        }
        return machines;
    }

    private static RequiredJoltageLevels ParseJoltageLevels(string line)
    {
        int firstCurlyBracket = line.IndexOf('{') + 1;
        string joltageString = line[firstCurlyBracket..^1];
        List<int> theJoltage = new List<int>();
        var splitJoltage = joltageString.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        foreach(var joltage in splitJoltage)
        {
            if (int.TryParse(joltage, out int joltageLevel))
            {
                theJoltage.Add(joltageLevel);
            }
            else
            {
                throw new Exception($"Expected an integer, got '{joltage}' in machine {line}");
            }
        }
        return new RequiredJoltageLevels(theJoltage);
    }

    private static Buttons ParseButtons(RequiredJoltageLevels requiredJoltageLevels, string line)
    {
        int firstSquareBracket = line.IndexOf(']');
        int firstCurlyBracket = line.IndexOf('{');
        string buttonsString = line.Substring(firstSquareBracket+1, firstCurlyBracket-firstSquareBracket-2);
        var buttonsArray = buttonsString.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        
        var buttonList = new List<Button>();
        foreach(var buttonString in buttonsArray)
        {
            var indicatorsToggled = new List<int>();
            foreach(char c in buttonString)
            {
                if (int.TryParse(c.ToString(), out int buttonNumber))
                {
                    indicatorsToggled.Add(buttonNumber);
                    continue;
                }
                else if(c == ',' || c == '(' || c == ')')
                {
                    continue;
                }
                throw new Exception($"Expected a button, got '{c}' in machine {line}");
            }
            buttonList.Add(new Button(indicatorsToggled));
        }

        return new Buttons(requiredJoltageLevels, buttonList);
    }
}

internal class Machine
{
    private readonly int _id;
    private readonly RequiredJoltageLevels _requiredJoltageLevels;
    private readonly Buttons _buttons;

    public Machine(int id, RequiredJoltageLevels requiredJoltageLevels, Buttons buttons)
    {
        _id = id;
        _requiredJoltageLevels = requiredJoltageLevels;
        _buttons = buttons;
    }

    internal int Solve()
    {
        var presses = _buttons.PressUntilSolved();
        Console.WriteLine($"Machine {_id} solved by pressing {presses} buttons.");
        return presses;
    }

    public override string ToString()
    {
        return $"{_id}: {_buttons} {_requiredJoltageLevels}";
    }
}

internal class RequiredJoltageLevels
{
    private readonly List<int> _requiredJoltageLevels;

    public RequiredJoltageLevels(List<int> requiredJoltageLevels)
    {
        _requiredJoltageLevels = requiredJoltageLevels;
    }

    internal bool AreSatisfiedBy(List<int> test)
    {
        return _requiredJoltageLevels.SequenceEqual(test);
    }

    internal int Length => _requiredJoltageLevels.Count;

    internal int this[int index] 
    { 
        get => _requiredJoltageLevels[index];
    }

    public override string ToString()
    {
        return $"{{{string.Join(",", _requiredJoltageLevels)}}}";
    }
}

internal class Buttons
{
    private List<int> initialState;
    private readonly List<Button> _buttons;
    private readonly RequiredJoltageLevels _requiredJoltageLevels;
    private readonly Queue<StateAndDepth> queue;

    public Buttons(RequiredJoltageLevels requiredJoltageLevels, List<Button> buttons)
    {
        _buttons = buttons;
        _requiredJoltageLevels = requiredJoltageLevels;
        initialState = Enumerable.Repeat(0, _requiredJoltageLevels.Length).ToList();
        queue = new Queue<StateAndDepth>();
    }

    internal int PressUntilSolved()
    {
        foreach(var button in _buttons)
        {
            if(button.WouldExceedMaxJoltage(initialState, _requiredJoltageLevels))
            {
                continue;
            }
            var firstState = button.Toggle(initialState);
            queue.Enqueue(new StateAndDepth(firstState, 1));
        }

        int i = _buttons.Count;
        while(true)
        {
            var currentStateAndDepth = queue.Dequeue();
            var currentState = currentStateAndDepth.State;

            // Add next list of buttons to try to the queue, unless it solves the problem or would exceed max joltage
            foreach(var button in _buttons)
            {
                if(button.WouldExceedMaxJoltage(currentState, _requiredJoltageLevels))
                {
                    //Console.WriteLine($"Iteration {i}: Current state {string.Join(",", currentState)} + Button {button} would exceed max joltage, skipping.");
                    continue;
                }

                var newState = button.Toggle(currentState);
                //Console.WriteLine($"Iteration {i}: Current state {string.Join(",", currentState)} + Button {button} => New state {string.Join(",", newState)}");

                if(_requiredJoltageLevels.AreSatisfiedBy(newState))    // Success 🎉
                {
                    Console.WriteLine($"{_requiredJoltageLevels} reached in iteration {i}");
                    return currentStateAndDepth.Depth + 1;
                }

                queue.Enqueue(new StateAndDepth(newState, currentStateAndDepth.Depth + 1));

                i++;
            }
        }

        throw new ArgumentOutOfRangeException("Could not solve machine with given buttons, within 5 presses.");
    }

    public override string ToString()
    {
        return $"{string.Join(" ", _buttons)}";
    }
}

internal class StateAndDepth
{
    private List<int> _state;
    private int _depth;

    public StateAndDepth(List<int> state, int depth)
    {
        _state = state;
        _depth = depth;
    }
    public int Depth => _depth;
    public List<int> State => _state;
}

internal class Button
{
    private readonly List<int> _indicatorsToggled;

    public Button(List<int> indicatorsToggled)
    {
        _indicatorsToggled = indicatorsToggled;
    }

    internal List<int> Toggle(List<int> previousState)
    {
        var newState = new List<int>(previousState);
        foreach(var indicatorIndex in _indicatorsToggled)
        {
            newState[indicatorIndex] += 1;
        }
        return newState; 
    }

    public override string ToString()
    {
        return $"({string.Join(",", _indicatorsToggled)})";
    }

    internal bool WouldExceedMaxJoltage(List<int> currentState, RequiredJoltageLevels requiredJoltageLevels)
    {
        foreach(var index in _indicatorsToggled)
        {
            if(currentState[index] == requiredJoltageLevels[index])
            {
                return true;
            }
        }
        return false;
    }
}