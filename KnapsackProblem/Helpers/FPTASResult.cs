namespace KnapsackProblem.Helpers
{
    public class FPTASResult
    {
        public int InstanceId { get; set; }
        public double Err { get; set; }

        public double Time { get; set; }
        public double RelativeError { get; set; }

        public int OptimalResult { get; set; }
        public int Result { get; set; }

    }
}
