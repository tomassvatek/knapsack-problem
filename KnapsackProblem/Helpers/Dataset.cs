using System.IO;
using System.Linq;

namespace KnapsackProblem.Helpers
{
    public class Dataset
    {
        private int _instanceCout = 0;
        private int _lastInstanceId = 0;

        public Dataset(string baseFilePath, string name, int instanceItemsCount)
        {
            BaseFilePath = baseFilePath;
            Name = name;
            InstanceItemsCount = instanceItemsCount;
        }

        public string BaseFilePath { get; set; }
        public string Name { get; }
        public int InstanceItemsCount { get; }

        public int InstancesCount => _instanceCout != 0 ? _instanceCout : GetDataSetCount();
        public int LastInstanceId => _lastInstanceId != 0 ? _lastInstanceId : GetDataSetLastIntanceId();


        public string GetFilePath()
            => @$"{BaseFilePath}\{Name}\{Name}{InstanceItemsCount}_inst.dat";

        public string GetSolutionFilePath()
            => @$"{BaseFilePath}\{Name}\{Name}{InstanceItemsCount}_sol.dat";

        public string GetResultFilePath(string resultFilePath)
            => @$"{resultFilePath}\{Name}\{Name}{InstanceItemsCount}_inst";

        private int GetDataSetCount()
        {
            using var streamReader = new StreamReader(GetFilePath());
            int lineCount = 0;
            while (streamReader.ReadLine() != null)
                lineCount++;

            _instanceCout = lineCount;

            return lineCount;
        }

        private int GetDataSetLastIntanceId()
        {
            var lastRow = File.ReadLines(GetFilePath()).Last();
            var split = lastRow.Split(" ");
            if (split.Length > 0)
            {
                var lastInstanceId = int.Parse(split[0]);
                _lastInstanceId = lastInstanceId;
                return lastInstanceId;
            }
            else
                throw new System.Exception("Last instance is invalid");
        }
    }
}
