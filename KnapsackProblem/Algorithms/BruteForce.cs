namespace KnapsackProblem.Algorithms
{
    public static class BruteForce  
    {
        public static bool Decide(int index, int[] weights, int[] prices, int currPrice, int currWeight, int maxWeight, int minPrice)
        {
            bool includeItemResult = false, excludeItemResult = false;

            if (index == weights.Length)
            {
                if (currWeight <= maxWeight && currPrice >= minPrice)
                    return true;
            }
            else
            {
                includeItemResult = Decide(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight, maxWeight, minPrice);
                excludeItemResult = Decide(index + 1, weights, prices, currPrice, currWeight, maxWeight, minPrice);
            }

            return includeItemResult || excludeItemResult;
        }


        public static int[] Solve(int index, int[] weights, int[] prices, int currPrice, 
            int currWeight, int maxWeight, int minPrice, int[] config, int[] bestConfig)
        {
            if (index == weights.Length)
            {
                if (currWeight <= maxWeight && currPrice >= minPrice)
                {
                    // If current config has higher price then the old one, store as the best solution
                    if (bestConfig[weights.Length] < currPrice)
                    {
                        // Save current config vector as the best solution
                        config.CopyTo(bestConfig, 0);
                        bestConfig[weights.Length] = currPrice;

                        return bestConfig;
                    }
                }
            }
            else
            {
                // Include item to the knapsack
                config[index] = 1;
                Solve(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight, maxWeight, minPrice, config, bestConfig);

                // Not include item to the knapsack
                config[index] = 0;
                Solve(index + 1, weights, prices, currPrice, currWeight, maxWeight, minPrice, config, bestConfig);
            }

            return bestConfig;
        }
    }
}
