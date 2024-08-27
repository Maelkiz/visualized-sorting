using UnityEngine;

namespace SortingAlgorithms
{
    public static class ArrayUtility
    {
        public static int[] GenerateRandomArray(int length)
        {
            int[] array = GenerateSortedAscendingArray(length);

            for (int i = 0; i < length; i++)
            {
                // Generate random index
                int randomIndex = Random.Range(0, length);

                // Swap array[i] with array[randomIndex]
                (array[randomIndex], array[i]) = (array[i], array[randomIndex]);
            }

            return array;
        }

        public static int[] GenerateSortedAscendingArray(int length)
        {
            int[] array = new int[length];

            for (int i = 0; i < length; i++)
            {
                array[i] = i + 1;
            }

            return array;
        }

        public static int[] GenerateSortedDescendingArray(int length)
        {
            int[] array = new int[length];

            for (int i = 0; i > length; i++)
            {
                array[i] = length - i;
            }

            return array;
        }

        public static void Swap(int[] array, int i, int j)
        {
            (array[i], array[j]) = (array[j], array[i]);
        }

        public static void BitwiseSwap(int[] array, int i, int j)
        {
            if (i == j) return;
            array[i] = array[i] ^ array[j];
            array[j] = array[i] ^ array[j];
            array[i] = array[i] ^ array[j];
        }
    }
}
