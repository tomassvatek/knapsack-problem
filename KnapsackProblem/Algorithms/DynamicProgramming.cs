using KnapsackProblem.Helpers;
using System;
using System.Collections.Generic;

namespace KnapsackProblem.Algorithms
{
    public static class DynamicProgramming
    {
        // Decomposition by price   
        public static int[] Solve(IReadOnlyList<Item> items, int maxWeight)
        {
            //Dictionary<int, Record> config = new Dictionary<int, int>();
            Dictionary<int, Record> map = new Dictionary<int, Record>
            {
                {0, new Record(0, 0) }
            };

            foreach (var item in items)
            {
                Dictionary<int, Record> temp = new Dictionary<int, Record>();
                foreach (var row in map)
                {
                    // if the new weight is lighter than previous, use it
                    temp[item.Price + row.Key] = (!map.ContainsKey(item.Price + row.Key) || map[item.Price + row.Key].Weight > row.Value.Weight + item.Weight
                        ? new Record(item.Weight + row.Value.Weight, item.IndexPositionBinValue + row.Value.Config)
                        : map[item.Price + row.Key]);
                }

                foreach (var row in temp)
                {
                    map[row.Key] = row.Value;
                }

            }

            // Find the best config
            int binaryConfig = 0;
            var bestPrice = 0;
            foreach (var item in map)
            {
                if (item.Value.Weight <= maxWeight && item.Key > bestPrice)
                {
                    binaryConfig = item.Value.Config;
                    bestPrice = item.Key;
                }
            }

            var binary = Convert.ToString(binaryConfig, 2).ToCharArray();
            Array.Reverse(binary);

            int[] binaryResult = new int[items.Count + 1];
            for (int i = 0; i < binaryResult.Length - 1; i++)
            {
                if (binary.Length > i)
                    binaryResult[i] = (int)char.GetNumericValue(binary[i]);
                else
                    binaryResult[i] = 0;
            }

            // Last index in the array represents the price
            binaryResult[^1] = bestPrice;

            return binaryResult;
        }

        // Decomposition by weight
        public static int ConstructDecompositionByWeight(int W, int[] wt, int[] val, int n)
        {
            //int i, w;
            int[,] K = new int[n + 1, W + 1];

            // Build table K[][] in bottom up manner 
            for (int i = 0; i <= n; i++)
            {
                for (int w = 0; w <= W; w++)
                {
                    if (i == 0 || w == 0)
                        K[i, w] = 0;
                    else if (wt[i - 1] <= w)
                        K[i, w] = Math.Max(
                            val[i - 1] + K[i - 1, w - wt[i - 1]],
                            K[i - 1, w]);
                    else
                        K[i, w] = K[i - 1, w];
                }
            }

            return K[n, W];
        }

    }

    public class Record
    {
        public Record(int weight, int config)
        {
            Weight = weight;
            Config = config;
        }

        public int Weight { get; set; }
        public int Config { get; set; }
    }

}
