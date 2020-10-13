﻿using System;
using System.Diagnostics;
using System.IO;
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
                @"C:\Users\tomas.svatek\Desktop\NR\NR27_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR30_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\NR\NR32_inst.dat",
            };

            string[] ZRInstanceFilePaths = {
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR4_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR10_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR15_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR20_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR22_inst.dat",
                @"C:\Users\tomas.svatek\Desktop\ZR\ZR25_inst.dat",
            };

            var csvSummaryBuilder = new StringBuilder();
            csvSummaryBuilder.AppendLine(TestResultHelper.GetSummaryResultHeader());

            foreach (var instanceFilePath in NRInstanceFilePaths)
            {
                var instanceResult = ExecuteTest(instanceFilePath, 500, 3, DatasetTypes.NR);
                var fileName = Path.GetFileNameWithoutExtension(instanceFilePath);
                File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\{fileName}_result.result.csv", instanceResult.InstancesResults);

                csvSummaryBuilder.AppendLine(TestResultHelper.FormatSummaryRow(instanceResult.N, instanceResult.BruteForceAvgTime, instanceResult.BAndBAvgTime));
            }

            File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\NR_summary.result.csv", csvSummaryBuilder.ToString());

            foreach (var instanceFilePath in ZRInstanceFilePaths)
            {
                var instanceResult = ExecuteTest(instanceFilePath, 500, 3, DatasetTypes.ZR);
                var fileName = Path.GetFileNameWithoutExtension(instanceFilePath);
                File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\{fileName}.result.csv", instanceResult.InstancesResults);

                csvSummaryBuilder.AppendLine(TestResultHelper.FormatSummaryRow(instanceResult.N, instanceResult.BruteForceAvgTime, instanceResult.BAndBAvgTime));
            }

            File.WriteAllText($@"C:\Users\tomas.svatek\Desktop\knapsack_results\ZR_summary.result.csv", csvSummaryBuilder.ToString());
        }

        static bool KnapsackBruteForce(int index, int[] weights, int[] prices, int currPrice, int currWeight, int maxWeight, int minPrice)
        {
            bool includeItemResult = false, excludeItemResult = false;

            if (index == weights.Length)
            {
                if (currWeight <= maxWeight && currPrice >= minPrice)
                    return true;
            }
            else
            {
                includeItemResult = KnapsackBruteForce(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight, maxWeight, minPrice);
                excludeItemResult = KnapsackBruteForce(index + 1, weights, prices, currPrice, currWeight, maxWeight, minPrice);
            }

            return includeItemResult || excludeItemResult;
        }

        static bool KnapsackBAndB(int index, int[] weights, int[] prices, int currPrice, int currWeight, int maxWeight, int minPrice)
        {
            if (currWeight <= maxWeight && currPrice >= minPrice)
                return true;

            if (index == weights.Length)
                return false;

            var bestPossiblePrice = currPrice;
            for (int i = index; i < weights.Length; i++)
            {
                bestPossiblePrice += prices[i];
                if (bestPossiblePrice + prices[i] >= minPrice)
                    break;
            }

            if (bestPossiblePrice < currPrice)
                return false;

            bool result = false;
            if (maxWeight >= weights[index] + currWeight)
                result = KnapsackBAndB(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight, maxWeight, minPrice);

            if (result)
                return true;
            else
                return KnapsackBAndB(index + 1, weights, prices, currPrice, currWeight, maxWeight, minPrice);
        }


        private static TestResult ExecuteTest(string filePath, int instanceCount, int measurementCount, DatasetTypes datasetType)
        {
            Console.WriteLine("{0}: starting calculating dateset {1}", DateTime.Now, filePath);

            if (measurementCount == 0)
                throw new ArgumentException(nameof(measurementCount));

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(TestResultHelper.GetInstanceResultHeader());

            var stopwatch = new Stopwatch();
            double totalBruteForceTime = 0;
            double totalBranchAndBoundTime = 0;

            for (int i = 1; i <= instanceCount; i++)
            {
                // Load instance from N
                var instance = InstanceLoader.LoadInstanceAsync(filePath, i * -1);
                int optimalSolution = InstanceLoader.LoadSolution(filePath, instance.Id, instance.N, datasetType);

                double instanceAvgBruteForceTime = 0;
                double instanceAvgBranchAndBoundTime = 0;

                for (int j = 0; j < measurementCount; j++)
                {
                    stopwatch.Reset();


                    // Start measurement for Brute Force
                    stopwatch.Start();
                    var bruteForceResult = KnapsackBruteForce(0, instance.Weights, instance.Prices, 0, 0, instance.MaxWeight, instance.MinPrice);
                    stopwatch.Stop();

                    CheckResult(instance, bruteForceResult, optimalSolution, datasetType, nameof(KnapsackBruteForce));
                    instanceAvgBruteForceTime += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();

                    // Start measurement for B&B
                    stopwatch.Start();
                    var bAndBResult = KnapsackBAndB(0, instance.Weights, instance.Prices, 0, 0, instance.MaxWeight, instance.MinPrice);
                    stopwatch.Stop();

                    CheckResult(instance, bAndBResult, optimalSolution, datasetType, nameof(KnapsackBAndB));
                    instanceAvgBranchAndBoundTime += stopwatch.Elapsed.TotalMilliseconds;
                }

                totalBruteForceTime += instanceAvgBruteForceTime;
                totalBranchAndBoundTime += instanceAvgBranchAndBoundTime;

                csvBuilder.AppendLine(TestResultHelper.FormatInstanceResultRow(instance.Id, instance.N, Avg(instanceAvgBruteForceTime, measurementCount), Avg(instanceAvgBranchAndBoundTime, measurementCount)));
            }

            // Instance total count
            csvBuilder.AppendLine(TestResultHelper.FormatInstanceResultRow(0, 0, Avg(totalBruteForceTime, instanceCount), Avg(totalBranchAndBoundTime, instanceCount)));
            Console.WriteLine("{0}: calculating dataset {1} ended", DateTime.Now, filePath);

            return new TestResult(instanceCount, csvBuilder.ToString(), Avg(totalBruteForceTime, instanceCount), Avg(totalBranchAndBoundTime, instanceCount));
        }

        static double Avg(double time, int measurementCount)
            => time / measurementCount;

        private static bool ValidateResult(bool result, int minPrice, int optimalSolution)
        {
            var solutionResult = minPrice <= optimalSolution;
            return solutionResult == result;
        }

        private static void CheckResult(Instance instance, bool result, int optimalPrice, DatasetTypes datasetType, string algorithmName)
        {
            if (!ValidateResult(result, instance.MinPrice, optimalPrice))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.Now}::{datasetType.ToString()}::{instance.N}::{instance.Id} Algorithm '{algorithmName}' result '{result}' is wrong. Optimal price is '{optimalPrice}.'");
                Console.ResetColor();
            }
        }

    }

}
