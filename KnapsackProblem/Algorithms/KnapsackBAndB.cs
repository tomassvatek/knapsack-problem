namespace KnapsackProblem.Algorithms
{
    public static class KnapsackBAndB
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

        public static int[] Construct(int index, int[] weights, int[] prices, int currPrice,
            int currWeight, int maxWeight, int minPrice, int[] config, int[] bestConfig)
        {
            if (currWeight <= maxWeight && currPrice >= minPrice)
            {
                config.CopyTo(bestConfig, 0);
                bestConfig[weights.Length] = currPrice;
                return bestConfig;
            }

            if (index == weights.Length)
            {
                bestConfig[weights.Length] = 0;
                return bestConfig;
            }

            var bestPossiblePrice = currPrice;
            for (int i = index; i < weights.Length; i++)
            {
                bestPossiblePrice += prices[i];
                if (bestPossiblePrice + prices[i] >= minPrice)
                    break;
            }

            if (bestPossiblePrice < currPrice)
            {
                bestConfig[weights.Length] = currPrice;
                return bestConfig;
            }

            //bool result = false;
            if (maxWeight >= weights[index] + currWeight)
            {
                config[index] = 1;
                config = Construct(index + 1, weights, prices, prices[index] + currPrice, weights[index] + currWeight,
                    maxWeight, minPrice, config, bestConfig);
            }

            if (config[weights.Length] > 0)
            {
                return bestConfig;
            }
            else
            {
                config[index] = 0;
                return Construct(index + 1, weights, prices, currPrice, currWeight,
                    maxWeight, minPrice, config, bestConfig);
            }
        }
    }
}
