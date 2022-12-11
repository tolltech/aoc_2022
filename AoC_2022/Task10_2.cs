using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
noop", @"##..##..##..##..##..##..##..##..##..##..
###...###...###...###...###...###...###.
####....####....####....####....####....
#####.....#####.....#####.....#####.....
######......######......######......####
#######.......#######.......#######.....")]
        [TestCase(@"Task10.txt", @"####.#..#..##..###..#..#..##..###..#..#.
...#.#.#..#..#.#..#.#.#..#..#.#..#.#.#..
..#..##...#....#..#.##...#....#..#.##...
.#...#.#..#.##.###..#.#..#.##.###..#.#..
#....#.#..#..#.#.#..#.#..#..#.#.#..#.#..
####.#..#..###.#..#.#..#..###.#..#.#..#.")]
        public void Task(string input, string expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();
            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            var cycleNumber = 0;
            var x = 1L;
            var i = 0;
            var rows = new List<char[]>
            {
                new char[40],
                new char[40],
                new char[40],
                new char[40],
                new char[40],
                new char[40],
            };

            foreach (var row in rows)
                for (var r = 0; r < 40; ++r)
                {
                    row[r] = '.';
                }

            while (cycleNumber <= 240)
            {
                ++cycleNumber;

                if (cycleNumber >= 240) break;
                
                Draw(rows, cycleNumber - 1, x);

                var line = lines[i++];
                var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);

                // if (cycleNumbers.Contains(cycleNumber))
                // {
                //     result += cycleNumber * x;
                // }

                if (splits[0] == "noop") continue;

                ++cycleNumber;
                Draw(rows, cycleNumber - 1, x);

                // if (cycleNumbers.Contains(cycleNumber))
                // {
                //     result += cycleNumber * x;
                // }

                x += int.Parse(splits[1]);
            }

            string.Join("\r\n", rows.Select(x => new string(x))).Should().Be(expected);
        }

        private static void Draw(List<char[]> rows, int position, long x)
        {
            var row = position / 40;
            var column = position % 40;

            rows[row][column] = (column <= x + 1) && (column >= x - 1) ? '#' : '.';
        }
    }
}