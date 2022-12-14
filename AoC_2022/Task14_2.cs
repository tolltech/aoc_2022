using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Text.Json.Nodes;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task14_2
    {
        [Test]
        [TestCase(@"498,4 -> 498,6 -> 496,6
503,4 -> 502,4 -> 502,9 -> 494,9", 93)]
        [TestCase(@"Task14.txt", 27566)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var rocks = new HashSet<Point>();
            foreach (var lines in input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var points = lines.Split(" -> ", StringSplitOptions.RemoveEmptyEntries)
                    .Select(x =>
                    {
                        var p = x.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
                        return new Point(p[0], p[1]);
                    })
                    .ToArray();

                for (var i = 1; i < points.Length; ++i)
                {
                    var prev = points[i - 1];
                    var cur = points[i];

                    for (var x = Math.Min(prev.X, cur.X); x <= Math.Max(prev.X, cur.X); ++x)
                    for (var y = Math.Min(prev.Y, cur.Y); y <= Math.Max(prev.Y, cur.Y); ++y)
                    {
                        rocks.Add(new Point(x, y));
                    }
                }
            }

            var rocksCount = rocks.Count;
            var maxY = rocks.Select(x => x.Y).Max();

            while (true)
            {
                var current = new Point(500, 0);
                while (true)
                {
                    var newPoint = GetStep(current, rocks);
                    if (newPoint == null) break;

                    current = newPoint.Value;
                    if (newPoint.Value.Y >= maxY + 1) break;
                }

                rocks.Add(current);
                
                if (Equals(current, new Point(500, 0))) break;

                //var ss = Print(rocks, backupRocks);
            }

            (rocks.Count - rocksCount).Should().Be(expected);
        }

        private static Point[] steps = { new(0, 1), new(-1, 1), new(1, 1) };

        private static Point? GetStep(Point current, HashSet<Point> rocks)
        {
            foreach (var step in steps)
            {
                var newPoint = current.Add(step);
                if (rocks.Contains(newPoint))
                {
                    continue;
                }

                return newPoint;
            }

            return null;
        }

        private static string Print(HashSet<Point> rocks, HashSet<Point> backupRocks)
        {
            var sb = new StringBuilder();
            for (var y = rocks.Select(x => x.Y).Min(); y <= rocks.Select(x => x.Y).Max(); ++y)
            {
                for (var x = rocks.Select(x => x.X).Min(); x <= rocks.Select(x => x.X).Max(); ++x)
                {
                    if (backupRocks.Contains(new Point(x, y))) sb.Append('#');
                    else if (rocks.Contains(new Point(x, y))) sb.Append('o');
                    else sb.Append('.');
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Point Add(Point p)
            {
                return new Point
                {
                    X = this.X + p.X,
                    Y = this.Y + p.Y,
                };
            }
        }
    }
}