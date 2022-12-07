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
    public class Task07_2
    {
        [Test]
        [TestCase(@"$ cd /
$ ls
dir a
14848514 b.txt
8504156 c.dat
dir d
$ cd a
$ ls
dir e
29116 f
2557 g
62596 h.lst
$ cd e
$ ls
584 i
$ cd ..
$ cd ..
$ cd d
$ ls
4060174 j
8033020 d.log
5626152 d.ext
7214296 k", 24933642)]
        [TestCase(@"Task07.txt", 6183184L)]
        public void Task(string input, long expected)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();

            var lines = input.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            var root = new Node("/");

            var current = root;
            foreach (var line in lines.Skip(1))
            {
                var splits = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                if (splits[0] == "$")
                {
                    if (splits[1] == "cd")
                    {
                        current = splits[2] == ".." ? current.Parent : current.Nodes[splits[2]];
                    }
                    continue;
                }

                if (splits[0] == "dir")
                {
                    current.Nodes.GetOrAdd(splits[1], s => new Node(s) { Parent = current });
                }
                else
                {
                    current.Files.GetOrAdd(splits[1], long.Parse(splits[0]));
                }
            }

            var freeSpace = 70000000 - root.TotalSize;
            var needSpace = 30000000 - freeSpace;
            var directories = new List<long>();
            
            var queue = new Queue<Node>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();

                directories.Add(currentNode.TotalSize);

                foreach (var node in currentNode.Nodes)
                {
                    queue.Enqueue(node.Value);
                }
            }

            var result = directories.OrderBy(x => x).SkipWhile(x => x < needSpace).First();

            //var s = Print(root, " - ");
            
            result.Should().Be(expected);
        }

        public class Node
        {
            public Node(string name)
            {
                Name = name;
                Nodes = new ConcurrentDictionary<string, Node>();
                Files = new ConcurrentDictionary<string, long>();
            }
            
            public ConcurrentDictionary<string, Node> Nodes { get; set; }
            public Node Parent { get; set; }
            public string Name { get; set; }
            public ConcurrentDictionary<string, long> Files { get; set; }

            public long TotalSize => Files.Sum(x => x.Value) + Nodes.Sum(x => x.Value.TotalSize);
        }

        private static string Print(Node node, string prefix)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"{prefix} {node.Name} (dir)");

            foreach (var childNode in node.Nodes.Values)
            {
                sb.AppendLine(Print(childNode, " - " + prefix));
            }
            
            foreach (var file in node.Files)
            {
                sb.AppendLine($" - {prefix} {file.Key} (file, {file.Value})");
            }
            
            return sb.ToString();
        }
    }
}