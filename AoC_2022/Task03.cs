using System;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task03
    {
        [Test]
        [TestCase(
            @"vJrwpWtwJgWrhcsFMMfFFhFp
jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL
PmmdzqPrVvPwwTWBwg
wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn
ttgJtRGJQctTZtZT
CrZsJsPPZsGzwwsLwLmpwMDw",
            157)]
        [TestCase(
            @"Task03.txt"
            ,
            8233)]
        public void Task(string input, int expected)
        {
            input = File.Exists(input) ? File.ReadAllText(input) : input;

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x=> x.Select(c=> char.IsLower(c) ? c - 'a' + 1 : c - 'A' + 27).ToArray())
                .ToList();

            var score = 0;
            foreach (var line in lines)
            {
                var first = line.Take(line.Length / 2).ToArray();
                var second = line.Skip(line.Length / 2).ToArray();

                var common = first.Intersect(second).Single();
                score += common;
            }

            score.Should().Be(expected);
        }
    }
}