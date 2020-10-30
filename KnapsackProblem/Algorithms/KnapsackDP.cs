using System;
using System.Linq;

namespace KnapsackProblem.Algorithms
{
    public static class KnapsackDP
    {
        //public static int[] Construct(int index, int[] weights, int[] prices, int maxWeight)
        //{
        //    if (IsTrivial(weights.Length, maxWeight, minPrice))
        //        return new int[weights.Length];

        //    Construct(weights.Where(w => w)
        //}

        //private static bool IsTrivial(int N, int maxWeight)
        //    => N <= 0 || maxWeight <= 0;

        public static int Construct(int[] weights, int[] prices, int maxWeight, int maxPrice)
        {
            int[,] result = new int[weights.Length + 1, maxPrice + 1];

            for (int i = 0; i <= weights.Length; i++)
            {
                for (int j = 0; j <= maxPrice; j++)
                {
                    if (i == 0 || j == 0)
                        result[i, j] = 0;
                    else if (prices[i - 1] <= maxPrice)
                    {
                        int a = weights[i - 1] + result[i - 1, j - prices[i - 1]];
                        int b = result[i - 1, j];

                        result[i, j] = Math.Min(a, b);
                    }
                    else
                    {
                        result[i, j] = result[i - 1, j];
                    }
                }
            }

            return result[weights.Length, maxWeight];
        }

        public static int knapSack(int W, int[] wt, int[] val, int n)
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
                    else if (val[i - 1] <= w)
                        K[i, w] = Math.Min(
                            wt[i - 1] + K[i - 1, w - wt[i - 1]],
                            K[i - 1, w]);
                    else
                        K[i, w] = K[i - 1, w];
                }
            }

            return K[n, W];
        }

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
}
