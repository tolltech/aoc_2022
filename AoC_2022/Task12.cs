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
    public class Task12
    {
        [Test]
        [TestCase(@"Sabqponm
abcryxxl
accszExk
acctuvwj
abdefghi", 31)]
        [TestCase(@"Task12.txt", 520)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var result = 0;

            var map = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => line.ToArray())
                .ToArray();

            (int Y, int X) start = (0, 0);
            (int Y, int X) finish = (0, 0);
            for (var r = 0; r < map.Length; ++r)
            for (var c = 0; c < map[r].Length; ++c)
            {
                if (map[r][c] == 'S')
                {
                    start = (r, c);
                    map[r][c] = 'a';
                }

                if (map[r][c] == 'E')
                {
                    finish = (r, c);
                    map[r][c] = 'z';
                }
            }

            var queue = new PriorityQueue<(int Y, int X), int>();
            var weights = new Dictionary<(int Y, int X), int>();
            var visited = new HashSet<(int Y, int X)> { start };
            weights[start] = 0;
            queue.Enqueue(start, 0);

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var notVisitedNeighbours = GetNeighbours(current, map, visited)
                    .ToArray();

                var currentWeight = weights[current];
                foreach (var notVisitedNeighbour in notVisitedNeighbours)
                {
                    var currentNeighbourWeight = weights.SafeGet(notVisitedNeighbour, int.MaxValue);
                    if (currentNeighbourWeight > currentWeight + 1)
                    {
                        weights[notVisitedNeighbour] = currentWeight + 1;
                        queue.Enqueue(notVisitedNeighbour, currentWeight + 1);
                    }
                }
                
                visited.Add(current);
            }

            weights[finish].Should().Be(expected);
        }

        private static IEnumerable<(int Y, int X)> GetNeighbours((int Y, int X) point, char[][] map,
            HashSet<(int Y, int X)> visited)
        {
            var steps = new[] { (0, 1), (1, 0), (-1, 0), (0, -1) };
            foreach (var step in steps)
            {
                var newPoint = (Y: point.Y + step.Item1, X: point.X + step.Item2);
                if (newPoint.Y < 0 || newPoint.X < 0) continue;
                if (newPoint.Y >= map.Length || newPoint.X >= map[newPoint.Y].Length) continue;

                if (map[newPoint.Y][newPoint.X] > map[point.Y][point.X] + 1) continue;
                if (visited.Contains(newPoint)) continue;

                yield return newPoint;
            }
        }
    }
}