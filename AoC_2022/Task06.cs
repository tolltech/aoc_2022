using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task06
    {
        [Test]
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 7)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 5)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 6)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 10)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 11)]
        [TestCase(@"Task06.txt", 1896)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var result = 0;
            var substring = new LinkedList<char>();
            for (var i = 0; i < input.Length; ++i)
            {
                substring.AddLast(input[i]);

                if (substring.Count < 4) continue;

                if (substring.Distinct().Count() == 4)
                {
                    result = i + 1;
                    break;
                }

                substring.RemoveFirst();
            }

            result.Should().Be(expected);
        }
    }
}