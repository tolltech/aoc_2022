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
    public class Task09
    {
        [Test]
        [TestCase(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2", 13)]
        [TestCase(@"Task09.txt", 5981, Ignore = "dolgo")]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();
            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            (int Y, int X)[] steps = lines.Select(x => x.Split(" ", StringSplitOptions.RemoveEmptyEntries))
                .Select(x => x[0] switch
                {
                    "R" => (0, int.Parse(x[1])),
                    "U" => (-int.Parse(x[1]), 0),
                    "L" => (0, -int.Parse(x[1])),
                    "D" => (int.Parse(x[1]), 0),
                    _ => throw new ArgumentOutOfRangeException()
                })
                .ToArray();

            var result = 0;

            var visited = new HashSet<(int, int)>();
            var head = (Y: 0,X: 0);
            var tail = (Y: 0,X: 0);

            var poweredSteps = new List<(int Y, int X)>();

            foreach (var step in steps)
            {
                for (var y = 1; y <= Math.Abs(step.Y); ++y)
                {
                    poweredSteps.Add((Math.Sign(step.Y), 0));
                }
                for (var x = 1; x <= Math.Abs(step.X); ++x)
                {
                    poweredSteps.Add((0, Math.Sign(step.X)));
                }
            }

            var printed = string.Empty;
            visited.Add((0, 0));
            foreach (var step in poweredSteps)
            {
                head = (head.Y + step.Y, head.X + step.X);

                if (IsNeighbour(head, tail))
                {
                    printed += Print(head, tail) + "\r\n\r\n";
                    continue;
                }

                tail = (tail.Y + step.Y, tail.X + step.X);

                if (IsDiagonal(head, tail))
                {
                    if (step.X != 0) tail = (head.Y, tail.X);
                    if (step.Y != 0) tail = (tail.Y, head.X);
                }

                printed += Print(head, tail) + "\r\n\r\n";
                
                visited.Add(tail);
            }
            
            visited.Count.Should().Be(expected);
        }

        private string Print((int Y, int X) head, (int Y, int X) tail)
        {
            var sb = new StringBuilder();
            for (var y = new[] { 0, head.Y, tail.Y }.Min(); y <= new[] { 0, head.Y, tail.Y }.Max(); ++y)
            {
                for (var x = new[] { 0, head.X, tail.X }.Min(); x <= new[] { 0, head.X, tail.X }.Max(); ++x)
                {
                    var point = (y, x);
                    var s = point == head ? "H" : point == tail ? "T" : ".";
                    if (point == (0, 0)) s = "s";
                    sb.Append(s);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static bool IsNeighbour((int Y, int X) left, (int Y,int X) right)
        {
            return Math.Abs(left.X - right.X) < 2 && Math.Abs(left.Y - right.Y) < 2;
        }
        
        private static bool IsDiagonal((int Y, int X) left, (int Y,int X) right)
        {
            return Math.Abs(left.X - right.X) == 1 && Math.Abs(left.Y - right.Y) == 1;
        }
    }
}