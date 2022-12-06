using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task06_2
    {
        [Test]
        [TestCase("mjqjpqmgbljsphdztnvjfqwrcgsmlb", 19)]
        [TestCase("bvwbjplbgvbhsrlpgdmjqwftvncz", 23)]
        [TestCase("nppdvjthqldpwncqszvftbrmjlhg", 23)]
        [TestCase("nznrnfrfntjfmvfwmzdfjlvtqnbhcprsg", 29)]
        [TestCase("zcfzfwzzqfrljwzlrfnpqdbhtmscgvjw", 26)]
        [TestCase(@"Task06.txt", 3452)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var result = 0;
            var substring = new LinkedList<char>();
            for (var i = 0; i < input.Length; ++i)
            {
                substring.AddLast(input[i]);

                if (substring.Count < 14) continue;

                if (substring.Distinct().Count() == 14)
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