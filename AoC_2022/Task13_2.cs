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
    public class Task13_2
    {
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
[1,[2,[3,[4,[5,6,0]]]],8,9]", 140)]
        [TestCase(@"Task13.txt", 22288)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
            lines.Add("[[2]]");
            lines.Add("[[6]]");

            var nodes = lines.Select(x => (Node: Build(x), Str: x)).ToArray();

            var sorted = nodes.OrderByDescending(x => x.Node, new Customomparer()).ToArray();

            sorted.Select((tuple, i) => (tuple, i))
                .Where(x => x.tuple.Str is "[[2]]" or "[[6]]")
                .Select(x => x.i + 1)
                .Aggregate((x, y) => x * y).Should().Be(expected);
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
        
        class Customomparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                if (Right(x, y) == true) return 1;
                if (Right(x, y) == false) return -1;
                return 0;
            }
            
            private static bool? Right(Node left, Node right)
            {
                if (left.Leave.HasValue && right.Leave.HasValue)
                {
                    if (left.Leave > right.Leave)
                    {
                        return false;
                    }
                
                    if (left.Leave < right.Leave)
                    {
                        return true;
                    }

                    return null;
                }

                if (left.Leave.HasValue || right.Leave.HasValue)
                {
                    return Right(left.ConvertToList(), right.ConvertToList());
                }

                for (var i = 0; i < Math.Max(left.Children.Count, right.Children.Count); ++i)
                {
                    if (i >= left.Children.Count) return true;
                    if (i >= right.Children.Count) return false;

                    var rightCurrent = Right(left.Children[i], right.Children[i]);
                    if (rightCurrent.HasValue) return rightCurrent.Value;
                }

                return null;
            }
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

            public Node ConvertToList()
            {
                if (!IsLeave) return this;
                return new Node
                {
                    Children = new[] { new Node(Leave) }.ToList(),
                    Leave = null
                };
            }
        }
    }
}