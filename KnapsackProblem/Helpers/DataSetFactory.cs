using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem.Helpers
{
    public static class DataSetFactory
    {
        private const string BaseFilePath = @"C:\Users\tomas.svatek\Desktop\KOP";

        private static int[] dataSetCount = { 4, 10, 15, 20, 22, 25, 27, 30, 32, 35, 37, 40 };

        public static List<Dataset> CreateDataSets(string dataSetName, int maxInstanceCount)
        {
            var counts = dataSetCount.Where(i => i <= maxInstanceCount);
            var dataSets = new List<Dataset>();
            foreach (var item in counts)
                dataSets.Add(new Dataset(BaseFilePath, dataSetName, item));

            return dataSets;
        }

        public static Dataset CreateDataSet(string dataSetName, int maxInstanceCount)
            => new Dataset(BaseFilePath, dataSetName, maxInstanceCount);
    }
}
