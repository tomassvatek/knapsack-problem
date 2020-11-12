namespace KnapsackProblem.Helpers
{
    public class TestResult
    {
        public int InstanceSize { get; set; }
        public string InstancesResults { get; set; }

        public double BFTime { get; set; }
        public double BandBTime { get; set; }
        public double DPTime { get; set; }

        public double GreedyHeuristicTime { get; set; }
        public double GreedyRelErr { get; set; }
        public double GreedyMaxErr { get; set; }

        public double ReduxHeuristicTime { get; set; }
        public double ReduxRelErr { get; set; }
        public double ReduxMaxErr { get; set; }

        public double FTPASTime { get; set; }
    }
}
