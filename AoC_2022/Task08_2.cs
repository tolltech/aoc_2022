using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task08_2
    {
        [Test]
        [TestCase(@"30373
25512
65332
33549
35390", 8)]
        [TestCase(@"Task08.txt", 172224)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();
            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var result = 0;
            var map = new int[lines.Length][];
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                map[i] = line.Where(char.IsDigit).Select(c => int.Parse(c.ToString())).ToArray();
            }

            for (var row = 0; row < map.Length; ++row)
            for (var column = 0; column < map[row].Length; ++column)
            {
                var newResult = IsVisible(row, column, map);
                if (newResult > result)
                    result = newResult;
            }

            result.Should().Be(expected);
        }

        private int IsVisible(int row, int column, int[][] map)
        {
            if (row == 0 || row == map.Length - 1 || column == 0 || column == map[0].Length - 1)
                return 0;

            return IsVisibleDirection(row, column, -1, 0, map)
                   * IsVisibleDirection(row, column, 0, -1, map)
                   * IsVisibleDirection(row, column, 0, 1, map)
                   * IsVisibleDirection(row, column, 1, 0, map);
        }

        private int IsVisibleDirection(int row, int column, int deltaRow, int deltaColumn, int[][] map)
        {
            var val = map[row][column];

            var r = row;
            var c = column;
            while (true)
            {
                if (r > 0 && r < map.Length - 1)
                    r += deltaRow;
                if (c > 0 && c < map[0].Length - 1)
                    c += deltaColumn;

                var otherVal = map[r][c];
                if (otherVal >= val) break;

                if (r == 0 || r == map.Length - 1) break;
                if (c == 0 || c == map[0].Length - 1) break;
            }

            return Math.Abs(c - column) + Math.Abs(r - row);
        }
    }
}