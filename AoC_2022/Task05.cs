using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task05
    {
        [Test]
        [TestCase(
            @"    [D]    
[N] [C]    
[Z] [M] [P]
 1   2   3 

move 1 from 2 to 1
move 3 from 1 to 3
move 2 from 2 to 1
move 1 from 1 to 2",
            "CMZ")]
        [TestCase(
            @"Task05.txt"
            ,
            "FRDSQRRCD")]
        public void Task(string input, string expected)
        {
            input = File.Exists(input) ? File.ReadAllText(input) : input;

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            var stackNumberLine = lines.Single(x => x.All(c => c == ' ' || char.IsDigit(c)));
            var stackCount = stackNumberLine.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Max();

            var inputStacksLines = lines.TakeWhile(x => x != stackNumberLine).ToArray();
            var stacks = Enumerable.Range(0, stackCount).Select(_ => new Stack<char>()).ToArray();

            foreach (var line in inputStacksLines.Reverse())
            {
                for (var i = 0; i < stackCount; ++i)
                {
                    var item = new string(line.Skip(i * 3 + i).Take(3).ToArray()).Where(char.IsLetter)
                        .SingleOrDefault();
                    if (char.IsLetter(item)) stacks[i].Push(item);
                }
            }

            foreach (var scenarioLine in lines.SkipWhile(x => x != stackNumberLine).Skip(1))
            {
                //move 1 from 2 to 1
                var scenario = scenarioLine.Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToArray();

                var createsCount = scenario[0];
                var crates = Enumerable.Range(0, createsCount).Select(x => stacks[scenario[1] - 1].Pop()).ToArray();
                foreach (var crate in crates) stacks[scenario[2] - 1].Push(crate);
            }

            var result = new string(stacks.Select(s => s.Peek()).ToArray());
            result.Should().Be(expected);
        }
    }
}