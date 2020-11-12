using KnapsackProblem.Helpers;
using System.Collections.Generic;

namespace KnapsackProblem.Algorithms
{
    public static class BranchAndBound
    {
        public static bool Decide(int index, int[] weights, int[] prices, int currPrice,
            int currWeight, int maxWeight, int minPrice)
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
                result = Decide(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight, maxWeight, minPrice);

            if (result)
                return true;
            else
                return Decide(index + 1, weights, prices, currPrice, currWeight, maxWeight, minPrice);
        }

        public static int[] Solve(int index, int[] weights, int[] prices, int currPrice,
            int currWeight, int maxWeight, int[] config, int[] bestConfig)
        {
            if (currPrice > bestConfig[weights.Length])
            {
                for (int i = 0; i < weights.Length; i++)
                {
                    bestConfig[i] = config[i];
                }
                bestConfig[weights.Length] = currPrice;
            }


            if (index == weights.Length)
                return bestConfig;

            var bestPossiblePrice = currPrice;
            for (int i = index; i < weights.Length; i++)
            {
                bestPossiblePrice += prices[i];
                if (bestPossiblePrice + prices[i] >= bestConfig[weights.Length])
                    break;
            }

            if (bestPossiblePrice < currPrice)
                return bestConfig;

            if (maxWeight >= weights[index] + currWeight)
            {
                config[index] = 1;
                config = Solve(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight,
                    maxWeight, config, bestConfig);
            }

            config[index] = 0;
            return Solve(index + 1, weights, prices, currPrice, currWeight,
                maxWeight, config, bestConfig);
        }


        // Object oriented 
        public static int[] Solve(int index, IReadOnlyList<Item> items, int currPrice,
            int currWeight, int maxWeight, int[] config, int[] bestConfig)
        {
            if (currPrice > bestConfig[items.Count])
            {
                for (int i = 0; i < items.Count; i++)
                {
                    bestConfig[i] = config[i];
                }

                bestConfig[items.Count] = currPrice;
            }

            if (index == items.Count)
                return bestConfig;

            if (items[index].Price + bestConfig[items.Count] <= bestConfig[items.Count]) return bestConfig;

            if (maxWeight < items[index].Weight + currWeight)
                return Solve(index + 1, items, currPrice, currWeight,
                    maxWeight, config, bestConfig);


            config[index] = 1;
            var includeSolution = Solve(index + 1, items, items[index].Price + currPrice, items[index].Weight + currWeight,
                maxWeight, config, bestConfig);

            config[index] = 0;
            var excludeSolution = Solve(index + 1, items, currPrice, currWeight,
                maxWeight, config, bestConfig);

            bestConfig = includeSolution[items.Count] > excludeSolution[items.Count] ? includeSolution : excludeSolution;
            return bestConfig;
        }

    }
}
