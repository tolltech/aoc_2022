using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task15_2
    {
        [Test]
        [TestCase(@"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3", 20, 56000011)]
        [TestCase(@"Task15.txt", 4000000, 12051287042458L)]
        public void Task(string input, int maxRow, long expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var inputs = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line =>
                {
                    var sensor = new Point(
                        int.Parse(new string(line.Skip(12).TakeWhile(c => char.IsDigit(c) || c == '-').ToArray())),
                        int.Parse(new string(line.SkipWhile(c => c != ',').Skip(4)
                            .TakeWhile(c => char.IsDigit(c) || c == '-').ToArray()))
                    );
                    var beacon = new Point(
                        int.Parse(new string(line.SkipWhile(c => c != ':').Skip(25)
                            .TakeWhile(c => char.IsDigit(c) || c == '-').ToArray())),
                        int.Parse(new string(line.SkipWhile(c => c != ':').Skip(1).SkipWhile(c => c != ',').Skip(4)
                            .TakeWhile(c => char.IsDigit(c) || c == '-').ToArray()))
                    );

                    return (Sensor: sensor, Beacon: beacon);
                })
                .ToArray();


            var resultX = 0;
            var resultY = 0;
            for (var row = 0; row <= maxRow; ++row)
            {
                var r=  GetResultX(inputs, row);
                if (r.HasValue)
                {
                    resultX = r.Value;
                    resultY = row;
                    break;
                }
            }

            (resultX * 4000000L + resultY).Should().Be(expected);
        }

        private int? GetResultX((Point Sensor, Point Beacon)[] inputs, int checkRow)
        {
            var insideSensors = new List<(int From, int To)>(inputs.Length);
            foreach (var (sensor, beacon) in inputs)
            {
                var distance = sensor.Distance(beacon);
                var deltaY = Math.Abs(sensor.Y - checkRow);
                if (deltaY > distance) continue;

                var deltaX = distance - deltaY;

                var fromX = sensor.X - deltaX;
                var toX = sensor.X + deltaX;

                insideSensors.Add((fromX, toX));
            }

            insideSensors = Merge(insideSensors.OrderBy(x => x.From).ToArray());

            if (insideSensors.Count > 1)
            {
                return insideSensors[0].To + 1;
            }

            return null;
        }
        
        private List<(int From, int To)> Merge((int From, int To)[] lines)
        {
            var newLines = new List<(int From, int To)>(lines.Length);

            lines = lines.OrderBy(x => x.From).ToArray();

            var current = lines[0];
            for (var i = 1; i < lines.Length; ++i)
            {
                if (Intersects(current, lines[i], out var merged))
                {
                    current = merged;
                }
                else
                {
                    newLines.Add(current);
                    current = lines[i];
                }
            }

            newLines.Add(current);

            return newLines;
        }

        private bool Intersects((int From, int To) first, (int From, int To) second, out (int From, int To) merged)
        {
            var left = first.From <= second.From ? first : second;
            var right = left == first ? second : first;

            if (left.To < right.From)
            {
                merged = (0, 0);
                return false;
            }

            merged = (left.From, Math.Max(left.To, right.To));
            return true;
        }

        [DebuggerDisplay("({X},{Y)")]
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

            public int Distance(Point other)
            {
                return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
            }
        }
    }
}