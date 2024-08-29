using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class QuickSortVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrement = 0.1f;
        [SerializeField] bool _applyColorization = false;
        [SerializeField] PivotSelection _pivotSelectionStrategy = PivotSelection.FIRST;
        private SortingDisplay _sortingDisplay;
        private int _partionIndex;
        private int _recursionLevel = 0;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) && !_sortingDisplay.IsSorting)
            {
                StartCoroutine(QuickSort(_sortingDisplay.Heights, 0, _sortingDisplay.Heights.Length - 1));
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator QuickSort(int[] numbers, int low, int high)
        {
            _sortingDisplay.IsSorting = true;

            _recursionLevel++;
            print("Current level of recursion: " + _recursionLevel);

            if (low < high)
            {
                // Start the Partition coroutine and wait for it to finish
                yield return StartCoroutine(Partition(numbers, low, high));

                yield return StartCoroutine(QuickSort(numbers, low, _partionIndex - 1));
                yield return StartCoroutine(QuickSort(numbers, _partionIndex + 1, high));

            }

            _recursionLevel--;
            print("Current level of recursion: " + _recursionLevel);

            if (_recursionLevel == 0)
            {
                _sortingDisplay.IsSorting = false;
                print(string.Join(" ", numbers)); // Print the final sorted array
                StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
            }
        }

        private IEnumerator Partition(int[] numbers, int low, int high)
        {
            // Colorize the partition
            if (_applyColorization)
            {
                for (int n = low; n <= high; n++) _sortingDisplay.ChangeColor(numbers[n], Color.grey);
            }

            // Select the pivot index and value
            int pivotIndex = SelectPivot(numbers, low, high, _pivotSelectionStrategy);
            int pivotValue = numbers[pivotIndex];

            if (_applyColorization)
            {
                _sortingDisplay.ChangeColor(numbers[pivotIndex], Color.yellow);
            }

            // Move pivot to the end
            ArrayUtility.Swap(numbers, pivotIndex, high);

            int i = low - 1;

            // Iterate through the numbers elements
            for (int j = low; j < high; j++)
            {
                // If current element is larger than pivot, swap and increment i
                if (numbers[j] < pivotValue)
                {
                    i++;
                    ArrayUtility.Swap(numbers, i, j);

                    // Update the display
                    _sortingDisplay.UpdateDisplay(numbers);
                    yield return new WaitForSeconds(_timeIncrement);
                }
            }

            // Swap pivot to its correct position
            ArrayUtility.Swap(numbers, i + 1, high);

            // Update the display
            _sortingDisplay.UpdateDisplay(numbers);

            // Visual delay for final pivot placement
            yield return new WaitForSeconds(_timeIncrement);

            // Return the partition index
            _partionIndex = i + 1;

            // Reset the color of the partition
            if (_applyColorization)
            {
                for (int n = low; n <= high; n++) _sortingDisplay.ChangeColor(numbers[n], Color.white);
            }
        }



        private int SelectPivot(int[] numbers, int low, int high, PivotSelection pivotSelectionStrategy)
        {
            switch (pivotSelectionStrategy)
            {
                case PivotSelection.FIRST:
                    return low;

                case PivotSelection.LAST:
                    return high;

                case PivotSelection.RANDOM:
                    return UnityEngine.Random.Range(low, high);

                case PivotSelection.MEDIAN_OF_THREE:
                    return Array.IndexOf(numbers, MedianOfThree(numbers, low, high));

                case PivotSelection.MEDIAN_OF_MEDIANS:
                    int[] threeMedians = {
                        MedianOfThree(numbers, low, high),
                        MedianOfThree(numbers, low, high),
                        MedianOfThree(numbers, low, high)
                    };
                    Array.Sort(threeMedians);
                    return Array.IndexOf(numbers, threeMedians[1]);

                default:
                    throw new ArgumentException("Invalid pivot selection method.");
            }

            static int MedianOfThree(int[] numbers, int low, int high)
            {
                int[] threeRandomNumbers = {
                    numbers[UnityEngine.Random.Range(low, high)],
                    numbers[UnityEngine.Random.Range(low, high)],
                    numbers[UnityEngine.Random.Range(low, high)]
                };
                Array.Sort(threeRandomNumbers);
                return threeRandomNumbers[1];
            }
        }
    }

    enum PivotSelection
    {
        FIRST,
        LAST,
        RANDOM,
        MEDIAN_OF_THREE,
        MEDIAN_OF_MEDIANS,
    }
}