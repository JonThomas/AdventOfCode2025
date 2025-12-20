# AdventOfCode2025

Solving puzzles using C#: [Advent of Code 2025](https://adventofcode.com/2025)

The code that calculates solutions to the Advent of Code puzzles is started from `program.cs`, by editing the file to start a given puzzle.
All input files are copied to the output directory on build, and are loaded from that directory.

# Running the applicaiton
1. Clone the repo
1. Install .NET 10
1. Run `dotnet run` on the command line in the /src/ folder to run a puzzle

# Running tests
1. Run `dotnet test` on the command line 

# Running CPU intensive puzzles
1. Run `dotnet build -c Release` on the command line, and start the applicaition from the bin\Release\net10.0\ folder.
This will speed up the execution several times.

# Overview and progress
Day1: [src/Day01Part1.cs](src/Day01Part1.cs) ⭐ [src/Day01Part2.cs](src/Day01Part2.cs) ⭐

Day2: [src/Day02Part1.cs](src/Day02Part1.cs) ⭐ [src/Day02Part2.cs](src/Day02Part2.cs) ⭐

Day3: [src/Day03Part1.cs](src/Day03Part1.cs) ⭐ [src/Day03Part2.cs](src/Day03Part2.cs) ⭐

Day4: [src/Day04Part1.cs](src/Day04Part1.cs) ⭐ [src/Day04Part2.cs](src/Day04Part2.cs) ⭐

Day5: [src/Day05Part1.cs](src/Day05Part1.cs) ⭐ [src/Day05Part2.cs](src/Day05Part2.cs) ⭐

Day6: [src/Day06Part1.cs](src/Day06Part1.cs) ⭐ [src/Day06Part2.cs](src/Day06Part2.cs) ⭐ Both part one and were mostly an input problem - when everything was read in correctly, it was just a matter of calculating the sum (and remember to not start multiplying when the initial value was 0)

Day7: [src/Day07Part1.cs](src/Day07Part1.cs) ⭐ [src/Day07Part2.cs](src/Day07Part2.cs) ⭐ After some trials and errors on Part2 I finally managed to have a proper graph and a recursive method to traverse it. But for a long time I though my implementation was flawed, because is seemed to go through an infinite loop. But the solution was to just wait, while the computer calculated the insanely large number of solutions!