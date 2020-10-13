namespace KnapsackProblem
{
    public class TestResult
    {
        public TestResult(int N, string instancesResults, double bruteForceAvgTime, double bAndBAvgTime)
        {
            this.N = N;
            this.InstancesResults = instancesResults;
            this.BruteForceAvgTime = bruteForceAvgTime;
            this.BAndBAvgTime = bAndBAvgTime;
        }

        public int N { get; set; }
        public string InstancesResults { get; }
        public double BruteForceAvgTime { get; }
        public double BAndBAvgTime { get; }
    }
}
