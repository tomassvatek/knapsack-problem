using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace KnapsackProblem.Helpers
{
    public static class InstanceLoader
    {

        public static Instance LoadInstanceAsync(Dataset dataSet, int instanceId)
        {
            var filePath = dataSet.GetFilePath();

            using var streamReader = new StreamReader(filePath);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                var rowInstanceId = line.Split(" ")[0];
                if (rowInstanceId.Equals(instanceId.ToString()))
                    break;
            }

            if (string.IsNullOrWhiteSpace(line))
                return null;

            return ParseInstance(line);
        }

        public static int LoadSolution(Dataset dataSet, int instanceId)
        {
            var filePath = dataSet.GetSolutionFilePath();
            return LoadSolution(instanceId, filePath);
        }

        private static int LoadSolution(int instanceId, string solutionFilePath)
        {
            using var streamReader = new StreamReader(solutionFilePath);
            string line;
            while ((line = streamReader.ReadLine()) != null)
                if (line.StartsWith(instanceId.ToString()))
                    break;

            if (string.IsNullOrWhiteSpace(line))
                throw new Exception($"Solution with id: '{instanceId}' not found.");

            return ParseResult(line);
        }

        private static int ParseResult(string line)
        {
            var items = line.Split(" ");
            return int.Parse(items[2]);
        }

        private static Instance ParseInstance(string line)
        {
            var items = line.Split(" ");
            var instance = new Instance();

            instance.Id = int.Parse(items[0]);
            instance.N = int.Parse(items[1]);
            instance.MaxWeight = int.Parse(items[2]);
            //instance.MaxWeight = int.Parse(items[2]);
            //instance.MinPrice = int.Parse(items[3]);

            instance.Weights = ParseValues(items, 3);
            instance.Prices = ParseValues(items, 4);

            for (int i = 0; i < instance.Weights.Length; i++)
                instance.Items.Add(new Item(i, instance.Weights[i], instance.Prices[i]));

            return instance;
        }

        private static int[] ParseValues(string[] arr, int startIndex)
        {
            var list = new List<int>();
            for (var i = startIndex; i < arr.Length; i += 2)
                list.Add(int.Parse(arr[i]));

            return list.ToArray();
        }
    }

    public class Instance
    {
        public int Id { get; set; }
        public int SolutionId => Id * -1;
        public int N { get; set; }
        public int MaxWeight { get; set; }
        public int MinPrice { get; set; }

        public int[] Prices { get; set; }
        public int[] Weights { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
    }

    public class Item : IComparable<Item>
    {
        public Item(int index, int weight, int price)
        {
            Index = index;
            Weight = weight;
            Price = price;
        }

        public int Index { get; }
        public int IndexPositionBinValue => (int)Math.Pow(2, Index);

        public int Weight { get; }
        public int Price { get; set; }
        public double Ratio => Weight != 0 ? Price / Weight : 0;

        public int CompareTo([AllowNull] Item other)
        {
            if (Ratio < other.Ratio)
                return 1;
            else if (Ratio > other.Ratio)
                return -1;
            else return 0;
        }

        public override string ToString()
            => $"{Price}";
    }

}

