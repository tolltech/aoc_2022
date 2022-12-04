using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task04
    {
        [Test]
        [TestCase(
            @"2-4,6-8
2-3,4-5
5-7,7-9
2-8,3-7
6-6,4-6
2-6,4-8",
            2)]
        [TestCase(
            @"Task04.txt"
            ,
            444)]
        public void Task(string input, int expected)
        {
            input = File.Exists(input) ? File.ReadAllText(input) : input;

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Split(",", StringSplitOptions.RemoveEmptyEntries).ToArray())
                .Select(x =>
                    x.Select(e => e.Split("-", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray())
                        .ToArray())
                .Select(x => (First: (Left: x[0][0], Right: x[0][1]), Second: (Left: x[1][0], Right: x[1][1])))
                .ToList();

            var score = 0;
            foreach (var line in lines)
            {
                if (Contains(line.First, line.Second) || Contains(line.Second, line.First)) score++;
            }

            score.Should().Be(expected);
        }

        private static bool Contains((int Left, int Right) source, (int Left, int Right) target)
        {
            return source.Left <= target.Left && source.Right >= target.Right;
        }
    }
}