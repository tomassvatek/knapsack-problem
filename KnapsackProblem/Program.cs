using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] NRInstanceFilePaths = {
                @"C:\Users\tomas.svatek\Desktop\NR\NR4_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR10_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR15_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR20_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR22_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR25_inst.dat",
            };

            string[] ZRInstanceFilePaths = {
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR4_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR10_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR15_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR20_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR22_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR25_inst.dat",
            };

            foreach (var instanceFilePath in NRInstanceFilePaths)
            {
                var instanceResult = ExecuteTest(instanceFilePath, 500, 3, DatasetTypes.NR);
                var fileName = Path.GetFileNameWithoutExtension(instanceFilePath);
                File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\{fileName}.result.csv", instanceResult);
            }

            foreach (var instanceFilePath in ZRInstanceFilePaths)
            {
                var instanceResult = ExecuteTest(instanceFilePath, 500, 3, DatasetTypes.ZR);
                var fileName = Path.GetFileNameWithoutExtension(instanceFilePath);
                File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\{fileName}.result.csv", instanceResult);
            }
        }

        static string ExecuteTest(string filePath, int instanceCount, int measurementCount, DatasetTypes datasetType)
        {
            Console.WriteLine("{0}: starting calculating dateset {1}", DateTime.Now, filePath);

            if (measurementCount == 0)
                throw new ArgumentException(nameof(measurementCount));

            //var instance = InputLoader.LoadInstanceAsync(filePath, 1 * -1);
            //var items = Map(instance.N, instance.Weights, instance.Prices);
            //var result = SolveKnapsackBAndBPrice(instance.N, instance.Weight, items);
            //Console.WriteLine(result);

            //SolveKnapsackBAndB(instance.N, instance.Weights, instance.Prices, instance.Weight);
            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(GetResultHeader());

            var stopwatch = new Stopwatch();
            double totalBruteForceTime = 0;
            double totalBranchAndBoundTime = 0;

            for (int i = 1; i <= instanceCount; i++)
            {
                // Load isntance from N
                var instance = InputLoader.LoadInstanceAsync(filePath, i * -1);
                int solution = LoadSolution(filePath, instance.Id, instance.N, datasetType);

                double instanceAvgBruteForceTime = 0;
                double instanceAvgBranchAndBoundTime = 0;

                for (int j = 0; j < measurementCount - 1; j++)
                {
                    // Reset stopwatch
                    stopwatch.Reset();

                    // Start measurement for Brute Force
                    stopwatch.Start();
                    var bruteForceResult = SolveKnapsackBruteForce(instance.N, instance.Weights, instance.Prices, instance.Weight);
                    stopwatch.Stop();

                    if (bruteForceResult != solution)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkMagenta;
                        Console.WriteLine($"{DateTime.Now}: dataset {datasetType.ToString()} N={instance.N} instance={instance.Id} brute force result {bruteForceResult} is wrong." +
                            $"Solution is {solution}");
                        Console.ResetColor();
                    }

                    // Add time
                    instanceAvgBruteForceTime += stopwatch.Elapsed.TotalMilliseconds;


                    //Console.Write("Instance {0}: ", i * -1);
                    //Console.Write("BF {0} ", stopwatch.Elapsed.TotalMilliseconds);

                    // Reset stopwatch
                    stopwatch.Reset();

                    // Start measurement for B&B
                    stopwatch.Start();
                    var items = Map(instance.N, instance.Weights, instance.Prices);
                    var BandBResult = SolveKnapsackBAndBPrice(instance.N, instance.Weight, items);
                    //SolveKnapsackBAndBPrice(instance.N, instance.Weights, instance.Prices, instance.Weight);
                    stopwatch.Stop();

                    if (BandBResult != solution)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine($"{DateTime.Now}: dataset {datasetType.ToString()} N={instance.N} instance={instance.Id} B&B result {BandBResult} is wrong." +
                            $"Solution is {solution}");
                        Console.ResetColor();
                    }

                    // Add time
                    instanceAvgBranchAndBoundTime += stopwatch.Elapsed.TotalMilliseconds;

                    //Console.Write("B&B {0}", stopwatch.Elapsed.TotalMilliseconds);
                    //Console.WriteLine();
                }
                //Console.WriteLine("Instance {0} result: {1}", i * -1, result2);
                //Console.WriteLine("Instance {0} elapsed {1}", i * -1, stopwatch.Elapsed.TotalMilliseconds);

                totalBruteForceTime += instanceAvgBruteForceTime;
                totalBranchAndBoundTime += instanceAvgBranchAndBoundTime;

                csvBuilder.AppendLine(FormatResultRow(instance.Id, instance.N, Avg(instanceAvgBruteForceTime, measurementCount), Avg(instanceAvgBranchAndBoundTime, measurementCount)));
            }

            // Instance total count
            csvBuilder.AppendLine(FormatResultRow(0, 0, Avg(totalBruteForceTime, instanceCount), Avg(totalBranchAndBoundTime, instanceCount)));
            Console.WriteLine("{0}: calculating dataset {1} ended", DateTime.Now, filePath);

            return csvBuilder.ToString();
        }

        static string GetResultHeader()
            => $"InstanceId;N;BruteForceTime;B&BTime";

        static string FormatResultRow(int instanceId, int inputSize, double bruteForceTime, double branchAndBoundTime)
            => $"{instanceId};{inputSize};{bruteForceTime};{branchAndBoundTime}";

        static double Avg(double time, int measurementCount)
            => time / measurementCount;

        static int LoadSolution(string instanceFilePath, int instanceId, int N, DatasetTypes datasetType)
        {
            return datasetType switch
            {
                DatasetTypes.NR => InputLoader.LoadSolutionForNR(instanceFilePath, N, instanceId * -1),
                DatasetTypes.ZR => InputLoader.LoadSolutionForZR(instanceFilePath, N, instanceId * -1),
                _ => throw new Exception("Dataset is not supported"),
            };
        }

        static int SolveKnapsackBruteForce(int n, int[] weights, int[] prices, int capacity)
        {
            int includeItem, excludeItem;
            if (n == 0)
                return 0;

            if (weights[n - 1] > capacity)
                return SolveKnapsackBruteForce(n - 1, weights, prices, capacity);
            else
            {
                includeItem = prices[n - 1] + SolveKnapsackBruteForce(n - 1, weights, prices, capacity - weights[n - 1]);
                excludeItem = SolveKnapsackBruteForce(n - 1, weights, prices, capacity);
                return Math.Max(includeItem, excludeItem);
            }
        }

        static int SolveKnapsackBAndB(int n, int[] weights, int[] prices, int capacity)
        {
            int includeItem, excludeItem;
            if (n == 0 || capacity <= 0)
                return 0;

            if (weights[n - 1] > capacity)
            {
                var notInclude = SolveKnapsackBAndB(n - 1, weights, prices, capacity);
                return notInclude;
            }
            else
            {
                includeItem = prices[n - 1] + SolveKnapsackBAndB(n - 1, weights, prices, capacity - weights[n - 1]);
                excludeItem = SolveKnapsackBAndB(n - 1, weights, prices, capacity);
                return Math.Max(includeItem, excludeItem);
            }
        }

        // 
        //static int SolveKnapsackBAndB(int n, int[] weights, int[] prices, int capacity, int bestSolution)
        //{
        //    int includeItem, excludeItem;
        //    if (n == 0 || capacity <= 0 || bestSolution > prices[n - 1])
        //        return 0;

        //    if (weights[n - 1] > capacity)
        //    {
        //        var notInclude = SolveKnapsackBAndB(n - 1, weights, prices, capacity, 0);
        //        return notInclude;
        //    }
        //    else
        //    {
        //        includeItem = prices[n - 1] + SolveKnapsackBAndB(n - 1, weights, prices, capacity - weights[n - 1], prices[n - 1]);
        //        excludeItem = SolveKnapsackBAndB(n - 1, weights, prices, capacity, 0);
        //        return Math.Max(includeItem, excludeItem);
        //    }
        //}

        static List<Item> Map(int n, int[] weights, int[] prices)
        {
            var items = new List<Item>();
            for (int i = 0; i < n; i++)
            {
                items.Add(new Item(weights[i], prices[i]));
            }

            return items;
        }

        static int SolveKnapsackBAndBPrice(int n, int capacity, List<Item> items)
        {
            items.Sort();

            var queue = new Queue<Node>();
            Node u = new Node { Level = -1, Profit = 0, Weight = 0 };
            Node v = new Node();

            queue.Enqueue(u);

            int maxProfit = 0;
            while (queue.Any())
            {
                u = queue.Dequeue();

                if (u.Level == -1)
                    v.Level = 0;

                if (u.Level == n - 1)
                    continue;

                v.Level = u.Level + 1;

                v.Weight = u.Weight + items[v.Level].Weight;
                v.Profit = u.Profit + items[v.Level].Price;

                if (v.Weight <= capacity && v.Profit > maxProfit)
                    maxProfit = v.Profit;

                v.Bound = Bound(v, n, capacity, items);

                if (v.Bound > maxProfit)
                    queue.Enqueue(v);

                v.Weight = u.Weight;
                v.Profit = u.Profit;
                v.Bound = Bound(v, n, capacity, items);
                if (v.Bound > maxProfit)
                    queue.Enqueue(v);
            }

            return maxProfit;
        }

        private static int Bound(Node u, int n, int capacity, List<Item> items)
        {
            // if weight overcomes the knapsack capacity, return 
            // 0 as expected bound 
            if (u.Weight >= capacity)
                return 0;

            // initialize bound on profit by current profit 
            int profit_bound = u.Profit;

            // start including items from index 1 more to current 
            // item index 
            int j = u.Level + 1;
            var totweight = u.Weight;

            // checking index condition and knapsack capacity 
            // condition 
            while ((j < n) && (totweight + items[j].Weight <= capacity))
            {
                totweight += items[j].Weight;
                profit_bound += items[j].Price;
                j++;
            }

            // If k is not n, include last item partially for 
            // upper bound on profit 
            if (j < n)
                profit_bound += (int)(capacity - totweight) * items[j].Price /
                                                 items[j].Price;

            return profit_bound;
        }
    }

}
