using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FluentAssertions;
using NUnit.Framework;

namespace AoC_2022
{
    [TestFixture]
    public class Task11
    {
        [Test]
        [TestCase(@"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1", 10605)]
        [TestCase(@"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1", 20, 1)]
        [TestCase(@"Task11.txt", 50616, 20)]
        public void Task(string input, int expected, int rounds = 20)
        {
            input = (File.Exists(input) ? File.ReadAllText(input) : input).Trim();
            var monkeysStr = input.Split(new[] { "\r\n\r\n", "\n\n" }, StringSplitOptions.RemoveEmptyEntries);
            var monkeys = new List<Monkey>();
            foreach (var monkeyInput in monkeysStr)
            {
                var splits = monkeyInput.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

                var items = splits[1].Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .ToArray();
                
                var operationSplits = splits[2].Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                var operationsInt = operationSplits.Where(x => int.TryParse(x, out _)).Select(int.Parse).ToArray();
                Func<int, int> operation = operationsInt.Length > 0 
                    ? operationSplits.Contains("*") ? x => x * operationsInt.Single() : x => x + operationsInt.Single()
                    : operationSplits.Contains("*") ? x => x * x : x => x + x;

                var test = splits[3].Split(new[] { " ", "," }, StringSplitOptions.RemoveEmptyEntries)
                    .Where(x=> int.TryParse(x, out _))
                    .Select(int.Parse)
                    .Single();

                var trueMonkey = splits[4].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .Single();
                
                var falseMonkey = splits[5].Split(" ", StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => int.TryParse(x, out _))
                    .Select(int.Parse)
                    .Single();

                monkeys.Add(new Monkey
                {
                    DivisibleByBase = test,
                    TrueMonkey = trueMonkey,
                    FalseMonkey = falseMonkey,
                    Items = new Queue<int>(items),
                    Operation = operation
                });
            }

            var monkeyInspections = new int [monkeys.Count];

            for (var r = 0; r < rounds; ++r)
            {
                for(var i =0; i< monkeys.Count; ++i)
                {
                    var monkey = monkeys[i];
                    monkeyInspections[i] += monkey.Items.Count;
                    while (monkey.Items.Count > 0)
                    {
                        var item = monkey.Items.Dequeue();
                        var newItem = monkey.Operation(item) / 3;
                        monkeys[newItem % monkey.DivisibleByBase == 0 
                            ? monkey.TrueMonkey 
                            : monkey.FalseMonkey]
                            .Items.Enqueue(newItem);
                    }
                }
            }

            monkeyInspections.OrderByDescending(x => x).Take(2).Aggregate((x, y) => x * y).Should().Be(expected);
        }

        class Monkey
        {
            public Queue<int> Items { get; set; }
            public int DivisibleByBase { get; set; }
            public int TrueMonkey { get; set; }
            public int FalseMonkey { get; set; }
            public Func<int, int> Operation { get; set; }
        }
        
    }
}