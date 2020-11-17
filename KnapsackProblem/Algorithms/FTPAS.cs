using KnapsackProblem.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace KnapsackProblem.Algorithms
{
    public class FTPAS
    {
        public static int[] Solve(IReadOnlyList<Item> items, int maxWeight, double maxError)
        {
            var itemsFit = items.Where(x => x.Weight <= maxWeight).ToList();//.Max(x => x.Price);
            if (itemsFit == null || !itemsFit.Any())
                return new int[items.Count + 1];

            var highestPrice = itemsFit.Max(x => x.Price);

            double shift = highestPrice * maxError / items.Count;
            var reducedItems = new List<Item>();
            foreach (var item in items)
            {
                reducedItems.Add(new Item(item.Index, item.Weight, (int)(item.Price / shift)));
            }

            var result = DynamicProgramming.Solve(reducedItems, maxWeight);

            var price = 0;
            for (int i = 0; i < items.Count; i++)
            {
                price += result[i] == 1 ? items[i].Price : 0;
            }

            result[^1] = price;
            return result;
        }

    }
}
