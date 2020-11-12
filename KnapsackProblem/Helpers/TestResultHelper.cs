using System;

namespace KnapsackProblem.Helpers
{
    public static class TestResultHelper
    {

        public static string GetSummaryResultHeader()
            => $"N;BF;B&B;DP;Greedy;Redux;FPTAS";

        public static string FormatSummaryRow(int N, double BF, double BandB, double DP, double greedy, double redux, double ftpas)
            => $"{N};{BF};{BandB};{DP};{greedy};{redux};{ftpas}";


        public static string GetInstanceResultHeader()
            => $"InstanceId;N;BruteForceTime;B&BTime;DP;Greedy;Redux;FTPAS";

        public static string FormatInstanceResultRow(
            int instanceId,
            int inputSize,
            double bruteForceTime,
            double branchAndBoundTime,
            double dynamicProgramming,
            double greedy,
            double redux,
            double ftpas)
            => $"{instanceId};{inputSize};{bruteForceTime};{branchAndBoundTime};{dynamicProgramming};{greedy};{redux};{ftpas}";


        public static string GetFTPASummaryHeader()
            => "N;FPTASERR;Time;RelError;MaxError";

        public static string FormatFTPASSummaryRow(FPTASSummary summary)
            => $"{summary.InstanceSize};{summary.Errr};{summary.Time};{summary.RelError};{summary.MaxError}";

        public static string GetFPTASHeader()
            => "InstanceId;FPTASERR;Time;OptimalResult;Result;RelError";

        public static string FormatFPTASRow(FPTASResult result)
            => $"{result.InstanceId};FPTAS ({result.Err});{result.Time};{result.OptimalResult};{result.Result};{result.RelativeError}";

        public static string GetHeuristicSummaryHeader()
            => "N;Greedy [ms];Redux [ms];ε Greedy [%];ε Redux [%];ε_max Greedy [%];ε_max Redux [%]";

        public static string FormatSummaryHeuristicRow(
            int instanceSize,
            double greedyTime,
            double reduxTime,
            double greedyRelError,
            double greedyMaxError,
            double reduxRelError,
            double reduxMaxError)
        {
            return $"{instanceSize};{greedyTime};{reduxTime};{greedyRelError};{reduxRelError};{greedyMaxError};{reduxMaxError}";
        }


        public static string GetHeuristicResultHeader()
            => $"InstanceId;Greedy;Redux;Solution;GreedySol;GreedySolDiff;ReduxSol;ReduxSolDiff;GreedyDiff;ReduxDiff";

        public static string FormatHeuristicResultRow(
            int instanceId,
            int result,
            double greedy,
            int greedyResult,
            double greedyRelErr,
            double redux,
            int reduxResult,
            double reduxRelErr)
        {
            return $"{instanceId};{greedy};{redux};{result};{greedyResult};{Math.Abs(result - greedyResult)};{reduxResult};{Math.Abs(result - reduxResult)};{greedyRelErr};{reduxRelErr}";
        }

    }
}
