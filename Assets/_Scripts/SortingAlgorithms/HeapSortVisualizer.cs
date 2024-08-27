using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class HeapSortVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementPerIteration = 0.1f;
        [SerializeField] bool _applyColorization = false;
        private SortingDisplay _sortingDisplay;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.H) && !_sortingDisplay.IsSorting)
            {
                StartCoroutine(HeapSort(_sortingDisplay.Heights));
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator HeapSort(int[] numbers)
        {
            _sortingDisplay.IsSorting = true;

            int n = numbers.Length;

            // Build max heap
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                yield return StartCoroutine(Heapify(numbers, n, i));
            }

            // Sort the array
            for (int i = n - 1; i >= 0; i--)
            {
                // Switch first and last elements
                ArrayUtility.Swap(numbers, i, 0);
                //(numbers[i], numbers[0]) = (numbers[0], numbers[i]);

                n--; // Decrement the length

                // Heapify the remaining unsorted numbers
                yield return StartCoroutine(Heapify(numbers, i, 0));
            }

            // Update the visualizer
            _sortingDisplay.UpdateDisplay(numbers);

            _sortingDisplay.IsSorting = false;
            print(string.Join(" ", numbers)); // Print the final sorted array
            StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
        }

        private IEnumerator Heapify(int[] numbers, int n, int i)
        {
            // Colorize the partition
            if (_applyColorization)
            {
                for (int k = n; k <= i; k++) _sortingDisplay.ChangeColor(numbers[k], Color.grey);
            }

            int max = i; // Index of the highest value
            int left = 2 * i + 1; // Index of left child
            int right = 2 * i + 2; // Index of right child

            // Check if the left child has a higher value than the parent 
            if (left < n && numbers[left] > numbers[i])
                max = left;

            // Repeat the process for the right
            if (right < n && numbers[right] > numbers[max])
                max = right;

            if (max != i)
            {
                // In the case that either child was larger a switch is perfomed
                ArrayUtility.Swap(numbers, max, i);
                //(numbers[max], numbers[i]) = (numbers[i], numbers[max]);

                // Update the visualizer
                _sortingDisplay.UpdateDisplay(numbers);

                yield return new WaitForSeconds(_timeIncrementPerIteration);

                // And a recursive call is used to ensure the entire branch is sorted
                yield return StartCoroutine(Heapify(numbers, n, max));

                // Decolorize the partition
                if (_applyColorization)
                {
                    for (int k = n; k <= i; k++) _sortingDisplay.ChangeColor(numbers[k], Color.white);
                }
            }
        }
    }
}
