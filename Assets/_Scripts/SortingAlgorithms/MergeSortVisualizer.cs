using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class MergeSortVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementPerIteration = 0.1f;
        private SortingDisplay _sortingDisplay;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.M) && !_sortingDisplay.IsSorting && _sortingDisplay != null)
            {
                StartCoroutine(MergeSort(_sortingDisplay.Heights, 0, _sortingDisplay.Heights.Length - 1));
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        // Modify your MergeSort to accept a callback action and invoke it after sorting is complete
        private IEnumerator MergeSort(int[] numbers, int left, int right)
        {
            _sortingDisplay.IsSorting = true;

            if (left < right)
            {
                int middle = left + (right - left) / 2;

                yield return StartCoroutine(MergeSort(numbers, left, middle));
                yield return StartCoroutine(MergeSort(numbers, middle + 1, right));

                yield return StartCoroutine(Merge(numbers, left, middle, right));

                // Update the display
                _sortingDisplay.UpdateDisplay(numbers);
                yield return new WaitForSeconds(_timeIncrementPerIteration);
            }

            if (left == 0 && right == numbers.Length - 1)
            {
                _sortingDisplay.IsSorting = false;
                print(string.Join(" ", numbers)); // Print the final sorted array
                StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
            }
        }

        private IEnumerator Merge(int[] numbers, int left, int middle, int right)
        {
            // Find sizes of two subarrays to be merged
            int leftLength = middle - left + 1;
            int rightLength = right - middle;

            // Create temp arrays
            int[] tempLeft = new int[leftLength];
            int[] tempRight = new int[rightLength];
            int i, j;

            //Copy data to temp arrays
            for (i = 0; i < leftLength; ++i)
            {
                tempLeft[i] = numbers[left + i];
            }

            for (j = 0; j < rightLength; ++j)
            {
                tempRight[j] = numbers[middle + 1 + j];
            }

            // Merge the temp arrays back into numbers[left..right]
            i = 0;
            j = 0;
            int k = left; // Initial index of merged subarray


            // Merge the temp arrays into numbers
            while (i < leftLength && j < rightLength)
            {
                if (tempLeft[i] <= tempRight[j])
                {
                    numbers[k] = tempLeft[i];
                    i++;
                }
                else
                {
                    numbers[k] = tempRight[j];
                    j++;
                }

                k++;
            }

            // Copy remaining elements of tempLeft[] if any
            while (i < leftLength)
            {
                numbers[k] = tempLeft[i];
                i++;
                k++;
            }

            // Copy remaining elements of tempRight[] if any
            while (j < rightLength)
            {
                numbers[k] = tempRight[j];
                j++;
                k++;
            }

            // Update the display
            _sortingDisplay.UpdateDisplay(numbers);
            yield return new WaitForSeconds(_timeIncrementPerIteration);
        }
    }
}