using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task03_2
    {
        [Test]
        [TestCase(
            @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw",
            70)]
        [TestCase(
            @"Task03.txt"
            ,
            2821)]
        public void Task(string input, int expected)
        {
            input = File.Exists(input) ? File.ReadAllText(input) : input;

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=> x.Select(c=> char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27).ToArray())
                .ToList();

            var score = 0;
            while (lines.Count > 0)
            {
                var common = lines.Take(3).Aggregate((x, y) => x.Intersect(y).ToArray()).Single();
                score += common;

                lines = lines.Skip(3).ToList();
            }

            score.Should().Be(expected);
        }
    }
}