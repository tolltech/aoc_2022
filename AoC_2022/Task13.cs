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
    public class Task13
    {
        private bool Right(Node leftRoot, Node rightRoot)
        {
            return false;
        }

        [Test]
        [TestCase(@"[1,1,3,1,1]
[1,1,5,1,1]

[[1],[2,3,4]]
[[1],4]

[9]
[[8,7,6]]

[[4,4],4,4]
[[4,4],4,4,4]

[7,7,7,7]
[7,7,7]

[]
[3]

[[[]]]
[[]]

[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]", 13)]
        [TestCase(@"[1,1,3,1,1]
[1,1,5,1,1]", 1)]
        [TestCase(@"[[1],[2,3,4]]
[[1],4]", 1)]
        [TestCase(@"[9]
[[8,7,6]]", 0)]
        [TestCase(@"[[4,4],4,4]
[[4,4],4,4,4]", 1)]
        [TestCase(@"[7,7,7,7]
[7,7,7]", 0)]
        [TestCase(@"[]
[3]", 1)]
        [TestCase(@"[[[]]]
[[]]", 0)]
        [TestCase(@"[1,[2,[3,[4,[5,6,7]]]],8,9]
[1,[2,[3,[4,[5,6,0]]]],8,9]", 0)]
        [TestCase(@"Task13.txt", 0)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var pairs = input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);

            var result = new List<int>();
            for (var i = 0; i < pairs.Length; i++)
            {
                var lines = pairs[i].Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                var leftRoot = Build(lines[0]);
                var rightRoot = Build(lines[1]);

                if (Right(leftRoot, rightRoot))
                {
                    result.Add(i + 1);
                }
            }

            result.Sum().Should().Be(expected);
        }

        private Node Build(string str)
        {
            var node = new Node();

            var json = JsonObject.Parse(str);
            if (json is JsonArray)
            {
                var array = json.AsArray();
                node.Children = array.Select(j=>Build(j.ToJsonString())).ToList();
            }
            else
            {
                node.Leave = json.GetValue<int>();
            }
            
            return node;
        }

        class Node
        {
            public Node(int? val = null)
            {
                Leave = val;
                Children = new List<Node>();
            }
            
            public List<Node> Children { get; set; }
            
            public int? Leave { get; set; }
            public bool IsEmpty => Children.Count == 0;
            public bool IsLeave => Leave.HasValue;
        }
    }
}