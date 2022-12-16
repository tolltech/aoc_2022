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
    public class Task16
    {
        [Test]
        [TestCase(@"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II
", 1651)]
        [TestCase(@"Task16.txt", 4725496)]
        public void Task(string input, int expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var name = new string(line.Skip(6).Take(2).ToArray());
                var rate = int.Parse(new string(line.Skip(23).TakeWhile(char.IsDigit).ToArray()));

                var node = new Node
                {
                    Name = name,
                    Rate = rate
                };
                Nodes[name] = node;

                var childNodes = new string(line.Split(";", StringSplitOptions.RemoveEmptyEntries)[1].Replace("valve", "valves").Skip(24).ToArray())
                    .Split(", ", StringSplitOptions.RemoveEmptyEntries);
                ChildNodes[name] = childNodes;
            }

            var root = "AA";
            var remaining = 30;
            var opened = new HashSet<string> { "AA" };
            var result = Dfs(root, remaining, 0, opened);
            result.Should().Be(expected);
        }

        private long Dfs(string current, int remaining, int acc, HashSet<string> opened)
        {
            if (remaining == 0) return acc;

            var max = 0;
            var children = ChildNodes[current];
            foreach (var child in children)
            {
                //var 
            }
            
            --remaining;
            return 0;
        }

        private readonly Dictionary<string, Node> Nodes = new();
        private readonly Dictionary<string, string[]> ChildNodes = new();

        struct Node
        {
            public string Name { get; set; }
            public int Rate { get; set; }
        }
    }
}