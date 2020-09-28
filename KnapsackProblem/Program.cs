using System;

namespace KnapsackProblem
{
    class Program
    {
        static void Main(string[] args)
        {
            var instanceFilePath = @"C:\Users\tomas.svatek\Desktop\NR\NR10_inst.dat";

            var instance = InputLoader.LoadInstanceAsync(instanceFilePath, -488);
            var solution = InputLoader.LoadSolution(instanceFilePath, instance.N, instance.SolutionId);

            var experimentSolution = Knapsack(instance.N, instance.Weight, instance.Weights, instance.Prices);

            Console.WriteLine("Solution is: {0}, experiment result: {1}", solution, experimentSolution);
        }

        static int Knapsack(int n, int weight, int[] weights, int[] prices)
        {
            if (n == 0 || weight == 0)
            {
                Console.WriteLine("N or Weight is 0.");
                return 0;
            }

            if (weights[n - 1] > weight)
            {
                Console.WriteLine("Weight of item {0} is bigger than capacity of bag.", n - 1);
                return Knapsack(n - 1, weight, weights, prices);
            }
            else
            {
                var includedItem = prices[n - 1] + Knapsack(n - 1, weight - weights[n - 1], weights, prices);
                Console.WriteLine("Price of included item {0} is {1}.", n - 1, includedItem);
                var notIncludedItem = Knapsack(n - 1, weight, weights, prices);
                Console.WriteLine("Price of not included item {0} is {1}.", n - 1, notIncludedItem);


                if (includedItem > notIncludedItem)
                    return includedItem;
                else
                    return notIncludedItem;
            }
        }

        static int knapSack(int weight, int[] weights,
                    int[] prices, int n)
        {

            // Base Case 
            if (n == 0 || weight == 0)
                return 0;

            // If weight of the nth item is 
            // more than Knapsack capacity W, 
            // then this item cannot be 
            // included in the optimal solution 
            if (weights[n - 1] > weight)
                return knapSack(weight, weights, prices, n - 1);

            // Return the maximum of two cases: 
            // (1) nth item included 
            // (2) not included 
            else
                return max(
                    prices[n - 1] + knapSack(
                                     weight - weights[n - 1], weights, prices, n - 1),
                    knapSack(weight, weights, prices, n - 1));
        }

        static int max(int a, int b)
        {
            return (a > b) ? a : b;
        }
    }

    public class KnapsackResult
    {
        public KnapsackResult(int price, int weight, bool included)
        {
            Price = price;
            Weight = weight;
            Included = included;
        }

        public int Price { get; set; }
        public int Weight { get; set; }
        public bool Included { get; set; }
    }
}
