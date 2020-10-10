using System;
using System.Diagnostics.CodeAnalysis;

namespace KnapsackProblem
{
    public class Item : IComparable<Item>
    {
        public Item(int weight, int price)
        {
            Weight = weight;
            Price = price;
        }

        public int Weight { get; }
        public int Price { get; }

        public int CompareTo([AllowNull] Item other)
        {
            var ratio = Price / Weight;
            var otherRation = other.Price / other.Weight;

            if (ratio > otherRation)
                return -1;
            if (ratio < otherRation)
                return 1;
            else
                return 0;
        }
    }
}
