using KnapsackProblem.Helpers;
using System.Collections.Generic;

namespace KnapsackProblem.Algorithms
{
    public static class GreedyHeuristic
    {
        public static int[] Solve(List<Item> items, int maxWeight)
        {
            // Allocate solution vector
            int[] config = new int[items.Count + 1];

            // Sort by price/weight by ratio
            items.Sort();

            int totalPrice = 0;
            foreach (var item in items)
            {
                // Until the weight is smaller than max weight, keep adding item
                if (item.Weight <= maxWeight)
                {
                    // Add item
                    config[item.Index] = 1;

                    // Reduce knapsack weight
                    maxWeight -= item.Weight;

                    // Add price to
                    totalPrice += item.Price;
                }
                else
                {
                    // Exclude item
                    config[item.Index] = 0;
                }
            }

            // Last index in the array represents the price
            config[items.Count] = totalPrice;

            return config;
        }
    }
}
