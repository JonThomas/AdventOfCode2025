namespace AdventOfCode2025.Day10Part1;

public class Day10Part1
{
    /// <summary>
    /// My idea:
    /// After parsing the input, I want to brute force all possible button presses, trying first to press each button once,
    /// then each combination of two buttons, then each combination of three buttons, etc. 
    /// When I find a combination that turns on indicators matching the maching indicator patterns, I will count that machine as solved.
    /// I will not press same button twice, since that just reverses the effect of the first press.
    /// To do this I'll impmement a breadth first search, using a queue that holds a lists of next sequence of buttons to try.
    /// For performance reasons, I've included the current state into the queued item: `ButtonAndState`.
    /// </summary>
    public static long Solve()
    {
        var input = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Day10Part1Input.txt"));
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
            var indicators = ParseIndicators(line);
            var buttons = ParseButtons(indicators, line);

            var machine = new Machine(i, indicators, buttons);
            machines.Add(machine);
        }
        return machines;
    }

    private static Indicators ParseIndicators(string line)
    {
        int firstSquareBracket = line.IndexOf(']');
        string indicatorsString = line.Substring(1, firstSquareBracket-1);
        List<bool> theIndicators = new List<bool>();
        foreach(var c in indicatorsString)
        {
            if (c == '.')
            {
                theIndicators.Add(false);
            }
            else if (c == '#')
            {
                theIndicators.Add(true);
            }
            else
            {
                throw new Exception($"Unexpected character {c} in machine {line}");
            }
        }

        return new Indicators(theIndicators);
    }

    private static Buttons ParseButtons(Indicators indicators, string line)
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

        return new Buttons(indicators, buttonList);
    }
}

internal class Machine
{
    private readonly int _id;
    private readonly Indicators _indicators;
    private readonly Buttons _buttons;

    public Machine(int id, Indicators indicators, Buttons buttons)
    {
        _id = id;
        _indicators = indicators;
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
        return $"{_id}: {_indicators} {_buttons}";
    }
}

internal class Indicators
{
    private readonly List<bool> _indicators;

    public Indicators(List<bool> indicators)
    {
        _indicators = indicators;
    }

    internal bool AreSatisfiedBy(List<bool> test)
    {
        return _indicators.SequenceEqual(test);
    }

    internal int Length => _indicators.Count;

    public override string ToString()
    {
        return $"[{string.Join("", _indicators.Select(i => i ? '#' : '.'))}]";
    }
}

internal class Buttons
{
    private List<bool> initialState;
    private readonly List<Button> _buttons;
    private readonly Indicators _indicators;
    private readonly Queue<List<ButtonAndState>> queue;

    public Buttons(Indicators indicators, List<Button> buttons)
    {
        _buttons = buttons;
        _indicators = indicators;
        initialState = Enumerable.Repeat(false, indicators.Length).ToList();
        queue = new Queue<List<ButtonAndState>>();
    }

    internal int PressUntilSolved()
    {
        foreach(var button in _buttons)
        {
            queue.Enqueue(new List<ButtonAndState> { new ButtonAndState(button, initialState) });
        }

        while(true)
        {
            var currentButtonSequenceAndState = queue.Dequeue();
            if(currentButtonSequenceAndState.Count == 0)
            {
                throw new ArgumentOutOfRangeException("Could not solve machine: No items in queue");
            }

            var buttonToPress = currentButtonSequenceAndState.Last().Button;
            var currentState = currentButtonSequenceAndState.Last().State;
            var newState = buttonToPress.Toggle(currentState);
            if(_indicators.AreSatisfiedBy(newState))    // Success 🎉
            {
                Console.WriteLine($"{_indicators} reached by pressing button {buttonToPress}");
                return currentButtonSequenceAndState.Count;
            }

            // Add next list of buttons to try to the queue, except the button we just pressed (pressing same button twice will not get us anywhere)
            foreach(var button in _buttons.Except(new List<Button> { buttonToPress }))
            {
                var newButtonAndStateList = new List<ButtonAndState>(currentButtonSequenceAndState) { new ButtonAndState(button, newState) };
                queue.Enqueue(newButtonAndStateList);
            }
        }

        throw new ArgumentOutOfRangeException("Could not solve machine with given buttons, within 5 presses.");
    }

    public override string ToString()
    {
        return $"{string.Join(" ", _buttons)}";
    }
}

internal class ButtonAndState
{
    private Button _button;
    private List<bool> _state;

    public ButtonAndState(Button button, List<bool> state)
    {
        _button = button;
        _state = state;
    }

    public Button Button => _button;
    public List<bool> State => _state;
}

internal class Button
{
    private readonly List<int> _indicatorsToggled;

    public Button(List<int> indicatorsToggled)
    {
        _indicatorsToggled = indicatorsToggled;
    }

    internal List<bool> Toggle(List<bool> previousState)
    {
        var newState = new List<bool>(previousState);
        foreach(var indicatorIndex in _indicatorsToggled)
        {
            newState[indicatorIndex] = !newState[indicatorIndex];
        }
        return newState;
    }

    public override string ToString()
    {
        return $"({string.Join(",", _indicatorsToggled)})";
    }

}