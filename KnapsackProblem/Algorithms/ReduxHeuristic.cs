using KnapsackProblem.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem.Algorithms
{
    public static class ReduxHeuristic
    {
        public static int[] Solve(List<Item> items, int maxWeight)
        {
            var mostValuableItem = items
                .Where(s => s.Weight <= maxWeight)
                .OrderByDescending(s => s.Price)
                .FirstOrDefault();

            var result = GreedyHeuristic.Solve(items, maxWeight);
            var totalPrice = result[items.Count];

            if (mostValuableItem?.Price > totalPrice)
            {
                result = new int[items.Count + 1];

                result[mostValuableItem.Index] = 1;
                result[items.Count] = mostValuableItem.Price;
            }

            return result;
        }

    }

}
