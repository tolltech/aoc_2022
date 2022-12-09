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
    public class Task09_2
    {
        [Test]
        [TestCase(@"R 4
U 4
L 3
D 1
R 4
D 1
L 5
R 2", 1)]
        [TestCase(@"R 5
U 8
L 8
D 3
R 17
D 10
L 25
U 20", 36)]
        [TestCase(@"Task09.txt", 2352, Ignore = "pizdec dolgo")]
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

            var visited = new HashSet<(int, int)>();
            var head = (Y: 0, X: 0);
            var tail = (Y: 0, X: 0);

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

            var tails = new List<(int Y, int X)>(9);
            tails.Add((0, 0));

            foreach (var step in poweredSteps)
            {
                head = (head.Y + step.Y, head.X + step.X);

                var lastTail = tails.Last();
                var newStep = step;
                for (var i = 0; i < tails.Count; ++i)
                {
                    var prevTail = tails[i];
                    tails[i] = MoveBy(i == 0 ? head : tails[i - 1], newStep, prevTail);

                    //printed += Print(head, tails, visited) + "\r\n\r\n";

                    if (prevTail == tails[i]) break;

                    newStep = (tails[i].Y - prevTail.Y, tails[i].X - prevTail.X);
                }

                if (tails.Last() != lastTail && tails.Count < 9)
                {
                    tails.Add(lastTail);
                }

                if (tails.Count == 9)
                {
                    visited.Add(tails.Last());
                }

                //printed += Print(head, tails, visited) + "\r\n\r\n";
            }

            visited.Count.Should().Be(expected);
        }

        private static (int Y, int X) MoveBy((int Y, int X) head, (int Y, int X) step, (int Y, int X) tail)
        {
            if (IsNeighbour(head, tail))
            {
                //printed += Print(head, tail) + "\r\n\r\n";
                return tail;
            }

            var defaultTail = (tail.Y + step.Y, tail.X + step.X);

            for (var y = -1; y <= 1; ++y)
            for (var x = -1; x <= 1; ++x)
            {
                if (y == 0 && x == 0) continue;
                var newTail = (Y: tail.Y + y, X: tail.X + x);
                if (!IsNeighbour(head, newTail)) continue;

                if (head.X == newTail.X || head.Y == newTail.Y) return newTail;

                defaultTail = newTail;
            }

            return defaultTail;
        }

        private string Print((int Y, int X) head, List<(int Y, int X)> tails, HashSet<(int, int)> visited)
        {
            var sb = new StringBuilder();
            for (var y = new[] { 0, head.Y }.Concat(tails.Select(x => x.Y)).Min();
                 y <= new[] { 0, head.Y }.Concat(tails.Select(x => x.Y)).Max();
                 ++y)
            {
                for (var x = new[] { 0, head.X }.Concat(tails.Select(x => x.X)).Min();
                     x <= new[] { 0, head.X }.Concat(tails.Select(x => x.X)).Max();
                     ++x)
                {
                    var point = (y, x);
                    var s = point == head ? "H" : tails.Contains(point) ? "+" : visited.Contains(point) ? "#" : ".";
                    if (point == (0, 0)) s = "s";
                    sb.Append(s);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        private static bool IsNeighbour((int Y, int X) left, (int Y, int X) right)
        {
            return Math.Abs(left.X - right.X) < 2 && Math.Abs(left.Y - right.Y) < 2;
        }

        private static bool IsDiagonal((int Y, int X) left, (int Y, int X) right)
        {
            return Math.Abs(left.X - right.X) == 1 && Math.Abs(left.Y - right.Y) == 1;
        }
    }
}