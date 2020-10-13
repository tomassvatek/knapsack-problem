using System;
using System.Collections.Generic;
using System.Text;

namespace KnapsackProblem
{
    public static class TestResultHelper
    {
        public static string GetInstanceResultHeader()
            => $"N;BruteForceTime;B&BTime";

        public static string GetSummaryResultHeader()
            => $"InstanceId;N;BruteForceTime;B&BTime";

        public static string FormatSummaryRow(int N, double bruteForceTime, double branchAndBoundTime)
            => $"{N};{bruteForceTime};{branchAndBoundTime}";

        public static string FormatInstanceResultRow(int instanceId, int inputSize, double bruteForceTime, double branchAndBoundTime)
            => $"{instanceId};{inputSize};{bruteForceTime};{branchAndBoundTime}";
    }
}
