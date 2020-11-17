using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using KnapsackProblem.Algorithms;
using KnapsackProblem.Helpers;

namespace KnapsackProblem
{
    class Program
    {
        const string FilePathResults = @"C:\Users\tomas.svatek\Desktop\KOP\results";
        const int MeasurementCount = 3;

        static void Main(string[] args)
        {
            double[] errors = { 0.05, 0.1, 0.2, 0.3, 0.4, 0.5, 0.6, 0.7, 0.8, 0.9, 1 };

            var csvSummaryBuilder = new StringBuilder();
            csvSummaryBuilder.AppendLine(TestResultHelper.GetFTPASummaryHeader());

            var dataSetsNK = DataSetFactory.CreateDataSets("NK", 25);
            var dataSetsZKC = DataSetFactory.CreateDataSets("ZKC", 25);
            var dataSetsZKW = DataSetFactory.CreateDataSets("ZKW", 25);

            foreach (var dataSet in dataSetsNK)
            {
                var result = RunFPTAS(dataSet, errors);
                //var result = ExecuteTest(dataSet, MeasurementCount);
                var resultFilePath = dataSet.GetResultFilePath(FilePathResults);

                WriteFTPASSummaryRow(csvSummaryBuilder, result.Item1);

                if (!Directory.Exists(resultFilePath))
                    Directory.CreateDirectory(resultFilePath);

                File.WriteAllText($"{resultFilePath}.{dataSet.Name}_FPTAS_result.csv", result.instanceResult);
            }

            var filePathResult = $@"{FilePathResults}\{"NK"}_FPTAS_result-summary.csv";
            File.WriteAllText(filePathResult, csvSummaryBuilder.ToString());

            //csvSummaryBuilder.Clear();
            //csvSummaryBuilder.AppendLine(TestResultHelper.GetFTPASummaryHeader());

            //foreach (var dataSet in dataSetsZKC)
            //{
            //    var result = RunFPTAS(dataSet, errors);
            //    //var result = RunAlgorithms(dataSet, MeasurementCount);
            //    var resultFilePath = dataSet.GetResultFilePath(FilePathResults);

            //    WriteFTPASSummaryRow(csvSummaryBuilder, result.Item1);

            //    if (!Directory.Exists(resultFilePath))
            //        Directory.CreateDirectory(resultFilePath);

            //    File.WriteAllText($"{resultFilePath}.{dataSet.Name}_FPTAS_result.csv", result.instanceResult);
            //}

            //filePathResult = $@"{FilePathResults}\{"ZKC"}_FPTAS_result-summary.csv";
            //File.WriteAllText(filePathResult, csvSummaryBuilder.ToString());

            //csvSummaryBuilder.Clear();
            //csvSummaryBuilder.AppendLine(TestResultHelper.GetFTPASummaryHeader());

            //foreach (var dataSet in dataSetsZKW)
            //{
            //    var result = RunFPTAS(dataSet, errors);
            //    //var result = ExecuteTest(dataSet, MeasurementCount);
            //    var resultFilePath = dataSet.GetResultFilePath(FilePathResults);

            //    WriteFTPASSummaryRow(csvSummaryBuilder, result.Item1);

            //    if (!Directory.Exists(resultFilePath))
            //        Directory.CreateDirectory(resultFilePath);

            //    File.WriteAllText($"{resultFilePath}.{dataSet.Name}_FPTAS_result.csv", result.instanceResult);
            //}

            //filePathResult = $@"{FilePathResults}\{"ZKW"}_FPTAS_result-summary.csv";
            //File.WriteAllText(filePathResult, csvSummaryBuilder.ToString());
        }

        // Measure algorithms time
        private static TestResult RunAlgorithms(Dataset dataSet, int measurementCount)
        {
            Console.WriteLine("{0}: starting calculating dateset {1}", DateTime.Now, dataSet.GetFilePath());

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(TestResultHelper.GetInstanceResultHeader());

            var stopwatch = new Stopwatch();

            double BFTotal = 0;
            double BandBTotal = 0;
            double dpTotal = 0;

            double reduxTotal = 0;
            double reduxErrSum = 0;
            double reduxMaxErr = 0;

            double greedyTotal = 0;
            double greedyErrSum = 0;
            double greedyMaxErr = 0;

            double ftpasTotal = 0;

            for (int i = 1; i <= dataSet.LastInstanceId; i++)
            {
                Console.WriteLine($"{DateTime.Now}::{dataSet.Name}::{dataSet.InstanceItemsCount}::{i}");

                // Load instance and solution
                var instance = InstanceLoader.LoadInstanceAsync(dataSet, i);
                if (instance == null)
                    continue;

                int optimalResult = InstanceLoader.LoadSolution(dataSet, i);

                double bruteForceTimeAvg = 0;
                double branchAndBoundTimeAvg = 0;
                double dynamicProgrammingAvg = 0;
                double greedyAvg = 0;
                double reduxAvg = 0;

                int[] greedyHeuristicResult = null;
                int[] reduxHeuristicResult = null;
                for (int j = 0; j < measurementCount; j++)
                {
                    stopwatch.Reset();

                    // Start Brute Force
                    stopwatch.Start();
                    var bruteForceResult = BruteForce.Solve(0, instance.Weights, instance.Prices, 0, 0, instance.MaxWeight, instance.MinPrice, new int[instance.N + 1], new int[instance.N + 1]);
                    stopwatch.Stop();

                    CheckResult(instance, bruteForceResult[instance.N], optimalResult, dataSet.Name, nameof(BruteForce));
                    bruteForceTimeAvg += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();

                    // Start Branch and Bound
                    stopwatch.Start();
                    var branchAndBoundResult = BranchAndBound.Solve(0, instance.Weights, instance.Prices, 0, 0, instance.MaxWeight, new int[instance.N + 1], new int[instance.N + 1]);
                    //BranchAndBound.Decide(0, instance.Weights, instance.Prices, 0, 0, instance.MaxWeight, 324);
                    stopwatch.Stop();

                    CheckResult(instance, branchAndBoundResult[instance.N], optimalResult, dataSet.Name, nameof(BranchAndBound));
                    branchAndBoundTimeAvg += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();

                    // Start Dynamic programming
                    stopwatch.Start();
                    var dynamicProgrammingResult = DynamicProgramming.Solve(instance.Items, instance.MaxWeight);
                    stopwatch.Stop();

                    CheckResult(instance, dynamicProgrammingResult[instance.N], optimalResult, dataSet.Name, nameof(DynamicProgramming));
                    dynamicProgrammingAvg += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();

                    // Start Greedy heuristic
                    stopwatch.Start();
                    greedyHeuristicResult = GreedyHeuristic.Solve(instance.Items.ToList(), instance.MaxWeight);
                    stopwatch.Stop();

                    //CheckResult(instance, greedyHeuristicResult[instance.N], optimalPrice, dataSet.Name, nameof(GreedyHeuristic));
                    greedyAvg += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();

                    // Start Redux heuristic
                    stopwatch.Start();
                    reduxHeuristicResult = ReduxHeuristic.Solve(instance.Items.ToList(), instance.MaxWeight);
                    stopwatch.Stop();

                    reduxAvg += stopwatch.Elapsed.TotalMilliseconds;

                    stopwatch.Reset();
                }

                BFTotal += Avg(bruteForceTimeAvg, measurementCount);
                BandBTotal += Avg(branchAndBoundTimeAvg, measurementCount);
                dpTotal += Avg(dynamicProgrammingAvg, measurementCount);
                greedyTotal += Avg(greedyAvg, measurementCount);
                reduxTotal += Avg(reduxAvg, measurementCount);

                csvBuilder.AppendLine(TestResultHelper.FormatInstanceResultRow(
                    instance.Id,
                    instance.N,
                    Avg(bruteForceTimeAvg, measurementCount),
                    Avg(branchAndBoundTimeAvg, measurementCount),
                    Avg(dynamicProgrammingAvg, measurementCount),
                    Avg(greedyAvg, measurementCount),
                    Avg(reduxAvg, measurementCount),
                    0
                 ));
            }

            Console.WriteLine("{0}: calculating dataset {1} ended", DateTime.Now, dataSet.GetFilePath());

            return new TestResult
            {
                InstanceSize = dataSet.InstanceItemsCount,
                InstancesResults = csvBuilder.ToString(),
                BFTime = Avg(BFTotal, dataSet.InstancesCount),
                BandBTime = Avg(BandBTotal, dataSet.InstancesCount),
                GreedyHeuristicTime = Avg(greedyTotal, dataSet.InstancesCount),
                ReduxHeuristicTime = Avg(reduxTotal, dataSet.InstancesCount),
                DPTime = Avg(dpTotal, dataSet.InstancesCount),
                FTPASTime = Avg(ftpasTotal, dataSet.InstancesCount),

                GreedyRelErr = Avg(greedyErrSum, dataSet.InstancesCount),
                GreedyMaxErr = greedyMaxErr,
                ReduxRelErr = Avg(reduxErrSum, dataSet.InstancesCount),
                ReduxMaxErr = reduxMaxErr
            };
        }

        // Measure FPTAS time, average and max error
        static (List<FPTASSummary>, string instanceResult) RunFPTAS(Dataset dataset, double[] errors)
        {
            Console.WriteLine("{0}: starting calculating dateset {1}", DateTime.Now, dataset.GetFilePath());

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine(TestResultHelper.GetFPTASHeader());

            List<FPTASResult> instacesResults = new List<FPTASResult>();

            for (int i = 1; i <= dataset.LastInstanceId; i++)
            {
                Console.WriteLine($"{DateTime.Now}::{dataset.Name}::{dataset.InstanceItemsCount}::{i}");

                // Load instance
                var instance = InstanceLoader.LoadInstanceAsync(dataset, i);

                if (instance == null)
                    continue;

                // Load optimal soultion
                int optimalResult = InstanceLoader.LoadSolution(dataset, i);

                foreach (var err in errors)
                {
                    instacesResults.Add(RunFPAS(instance, optimalResult, csvBuilder, err));
                }
            }

            var summaryItems = new List<FPTASSummary>();
            foreach (var err in errors)
            {
                summaryItems.Add(GetSummaryItem(dataset, instacesResults, err));
            }

            return (summaryItems, csvBuilder.ToString());
        }

        static FPTASSummary GetSummaryItem(Dataset dataset, List<FPTASResult> results, double err)
        {
            var s = results.Where(s => s.Err == err).ToList();

            var sumErr = results.Where(s => s.Err == err).Sum(s => s.RelativeError);
            var result = sumErr / dataset.InstanceItemsCount;

            return new FPTASSummary
            {
                InstanceSize = dataset.InstanceItemsCount,
                Errr = err,
                RelError = sumErr / dataset.InstancesCount,
                MaxError = results.Where(s => s.Err == err).Max(s => s.RelativeError),
                Time = results.Where(s => s.Err == err).Sum(s => s.Time) / dataset.InstancesCount
            };
        }

        static FPTASResult RunFPAS(Instance instance, int optimalResult, StringBuilder sb, double err)
        {
            var stopwatch = new Stopwatch();

            double time = 0;
            int[] result = null;
            for (int i = 0; i < MeasurementCount; i++)
            {
                stopwatch.Start();
                result = FTPAS.Solve(instance.Items, instance.MaxWeight, err);
                stopwatch.Stop();

                time += stopwatch.Elapsed.TotalMilliseconds;

                stopwatch.Reset();
            }

            var fptasResult = new FPTASResult
            {
                InstanceId = instance.Id,
                Err = err,
                Time = time / MeasurementCount,
                RelativeError = CalculatePercErr(optimalResult, result[instance.N]),
                Result = result[instance.N],
                OptimalResult = optimalResult
            };

            sb.AppendLine(TestResultHelper.FormatFPTASRow(fptasResult));

            return fptasResult;
        }

        static double CalculateRelativeError(double optimalResult, double approximationResult)
        {
            if (optimalResult == 0 && approximationResult == 0)
                return 0;

            return Math.Abs(approximationResult - optimalResult) / Math.Max(optimalResult, approximationResult);
        }

        static double CalculatePercErr(double optimalResult, double approximationResult)
        {
            if (optimalResult == 0 && approximationResult == 0)
                return 0;

            double diff = approximationResult / (optimalResult / 100);
            double result = 100 - Math.Round(diff, 5);

            return result;
        }

        static double Avg(double result, double count)
            => result / count;

        private static void CheckResult(Instance instance, int result, int optimalPrice, string dataSetName, string algorithmName)
        {
            if (result != optimalPrice)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"{DateTime.Now}::{dataSetName}::{instance.N}::{instance.Id} Algorithm '{algorithmName}' result '{result}' is wrong. Optimal price is '{optimalPrice}.'");
                Console.ResetColor();
            }
        }

        static void WriteHeuristicSummaryRow(StringBuilder builder, Dataset dataSet, TestResult result)
        {
            builder.AppendLine(TestResultHelper.FormatSummaryHeuristicRow(
                dataSet.InstanceItemsCount,
                result.GreedyHeuristicTime,
                result.ReduxHeuristicTime,
                result.GreedyRelErr,
                result.GreedyMaxErr,
                result.ReduxRelErr,
                result.ReduxMaxErr
            ));
        }

        static void WriteTimeSummaryRow(StringBuilder builder, Dataset dataSet, TestResult result)
        {
            builder.AppendLine(TestResultHelper.FormatSummaryRow(
                dataSet.InstanceItemsCount,
                result.BFTime,
                result.BandBTime,
                result.DPTime,
                result.GreedyHeuristicTime,
                result.ReduxHeuristicTime,
                result.FTPASTime
            ));
        }

        static void WriteFTPASSummaryRow(StringBuilder builder, List<FPTASSummary> summaries)
        {
            foreach (var summary in summaries)
            {
                builder.AppendLine(TestResultHelper.FormatFTPASSummaryRow(summary));
            }
        }

    }

}
