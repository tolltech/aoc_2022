using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task10_2
    {
        [Test]
        [TestCase(@"addx 15
addx -11
addx 6
addx -3
addx 5
addx -1
addx -8
addx 13
addx 4
noop
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx 5
addx -1
addx -35
addx 1
addx 24
addx -19
addx 1
addx 16
addx -11
noop
noop
addx 21
addx -15
noop
noop
addx -3
addx 9
addx 1
addx -3
addx 8
addx 1
addx 5
noop
noop
noop
noop
noop
addx -36
noop
addx 1
addx 7
noop
noop
noop
addx 2
addx 6
noop
noop
noop
noop
noop
addx 1
noop
noop
addx 7
addx 1
noop
addx -13
addx 13
addx 7
noop
addx 1
addx -33
noop
noop
noop
addx 2
noop
noop
noop
addx 8
noop
addx -1
addx 2
addx 1
noop
addx 17
addx -9
addx 1
addx 1
addx -3
addx 11
noop
noop
addx 1
noop
addx 1
noop
noop
addx -13
addx -19
addx 1
addx 3
addx 26
addx -30
addx 12
addx -1
addx 3
addx 1
noop
noop
noop
addx -9
addx 18
addx 1
addx 2
noop
noop
addx 9
noop
noop
noop
addx -1
addx 2
addx -37
addx 1
addx 3
noop
addx 15
addx -21
addx 22
addx -6
addx 1
noop
addx 2
addx 1
noop
addx -10
noop
noop
addx 20
addx 1
addx 2
addx 2
addx -6
addx -11
noop
noop
noop", 13140)]
        [TestCase(@"Task10.txt", 13820)]
        public void Task(string input, long expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();
            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            
            var x = 1L;
            foreach (var line in lines)
            {
                ++cycleNumber;

                var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                if (cycleNumbers.Contains(cycleNumber))
                {
                    result += cycleNumber * x;
                }
                
                if (splits[0] == "noop") continue;

                ++cycleNumber;

                if (cycleNumbers.Contains(cycleNumber))
                {
                    result += cycleNumber * x;
                }
                
                x += int.Parse(splits[1]);
            }

            if (cycleNumber < 20)
                result = x;

            result.Should().Be(expected);
        }
    }
}