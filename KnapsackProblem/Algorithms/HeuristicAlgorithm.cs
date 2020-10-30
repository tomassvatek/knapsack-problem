using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KnapsackProblem.Algorithms
{
    public static class HeuristicAlgorithm
    {
        public static int[] SolveUsingGreedyHeuristic(int[] weights, int[] prices, int maxWeight)
        {
            // Allocate solution vector
            int[] config = new int[weights.Length];

            // Get items with ratio
            var ratios = GetRatios(weights, prices);

            int totalPrice = 0;
            for (int i = 0; i < ratios.Count; i++)
            {
                // Until the weight is smaller than max weight, keep adding item
                if (ratios[i].Weight <= maxWeight)
                {
                    // Record an item was added
                    config[ratios[i].Index] = 1;

                    // Reduce knapsack weight
                    maxWeight -= ratios[i].Weight;

                    // Add price to
                    totalPrice += ratios[i].Price;
                }
                else
                {
                    // Record an item was not added
                    config[ratios[i].Index] = 0;
                }
            }

            return config;
        }

        public static int[] SolveUsingReduxHeuristic(int[] weights, int[] prices, int maxWeight)
        {
            // Allocate solution vector
            int[] config = new int[weights.Length];

            // Get items with ratio
            var items = GetRatios(weights, prices);

            var mostValuableItem = items
                .Where(s => s.Weight <= maxWeight)
                .OrderByDescending(s => s.Price)
                .First();

            int totalPrice = 0;
            for (int i = 0; i < items.Count; i++)
            {
                // Until the weight is smaller than max weight, keep adding item
                if (items[i].Weight <= maxWeight)
                {
                    // Record an item was added
                    config[items[i].Index] = 1;

                    // Reduce knapsack weight
                    maxWeight -= items[i].Weight;

                    // Add price to
                    totalPrice += items[i].Price;
                }
                else
                {
                    // Record an item was not added
                    config[items[i].Index] = 0;
                }
            }

            if (mostValuableItem.Price > totalPrice)
            {
                config = new int[weights.Length];
                config[mostValuableItem.Index] = 1;
            }

            return config;
        }

        private static List<GreedyItem> GetRatios(int[] weights, int[] prices)
        {
            var ratio = new List<GreedyItem>();
            for (int i = 0; i < weights.Length; i++)
            {
                ratio.Insert(i, new GreedyItem(i, prices[i], weights[i]));
            }

            ratio.Sort();
            return ratio;
        }
    }

    public class GreedyItem : IComparable<GreedyItem>
    {
        public GreedyItem(int index, int price, int weight)
        {
            Index = index;
            Price = price;
            Weight = weight;
            Ratio = price / weight;
        }

        public int Index { get; }
        public double Ratio { get; }
        public int Price { get; }
        public int Weight { get; }

        public int CompareTo([AllowNull] GreedyItem other)
        {
            if (Ratio < other.Ratio)
                return 1;
            else if (Ratio > other.Ratio)
                return -1;
            else return 0;
        }
    }
}
