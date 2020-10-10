using System.Collections.Generic;
using System.IO;

namespace KnapsackProblem
{
    public static class InputLoader
    {
        public static Instance LoadInstanceAsync(string filePath, int instanceId)
        {
            using var streamReader = new StreamReader(filePath);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.StartsWith(instanceId.ToString()))
                    break;
            }

            if (string.IsNullOrWhiteSpace(line))
                throw new System.Exception($"Instance with id: '{instanceId}' not found.");

            return ParseInstance(line);
        }

        public static int LoadSolutionForNR(string instanceFilePath, int N, int instanceId)
        {
            var solutionFilePath = $"{Path.GetDirectoryName(instanceFilePath)}\\NK{ N}_sol.dat";
            return LoadSolution(instanceId, solutionFilePath);
        }


        public static int LoadSolutionForZR(string instanceFilePath, int N, int instanceId)
        {
            var solutionFilePath = $"{Path.GetDirectoryName(instanceFilePath)}\\ZK{N}_sol.dat";
            return LoadSolution(instanceId, solutionFilePath);
        }

        private static int LoadSolution(int instanceId, string solutionFilePath)
        {
            using var streamReader = new StreamReader(solutionFilePath);
            string line;
            while ((line = streamReader.ReadLine()) != null)
            {
                if (line.StartsWith(instanceId.ToString()))
                    break;
            }

            if (string.IsNullOrWhiteSpace(line))
                throw new System.Exception($"Solution with id: '{instanceId}' not found.");

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
            instance.Weight = int.Parse(items[2]);

            instance.Weights = ParseValues(items, 4);
            instance.Prices = ParseValues(items, 5);


            return instance;
        }

        private static int[] ParseValues(string[] arr, int startIndex)
        {
            var list = new List<int>();
            for (var i = startIndex; i < arr.Length; i += 2)
            {
                list.Add(int.Parse(arr[i]));
            }

            return list.ToArray();
        }
    }

    public class Instance
    {
        public int Id { get; set; }
        public int SolutionId => Id * -1;
        public int N { get; set; }
        public int Weight { get; set; }

        public int[] Prices { get; set; }
        public int[] Weights { get; set; }
    }

}

