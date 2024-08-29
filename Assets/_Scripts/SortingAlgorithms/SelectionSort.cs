using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    [RequireComponent(typeof(SortingDisplay))]
    public class SelectionVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementForScan = 0.0005f;
        [SerializeField] float _timeIncrementForSwap = 0.1f;
        [SerializeField] bool _applyColorization = false;
        private SortingDisplay _sortingDisplay;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S) && !_sortingDisplay.IsSorting)
            {
                StartCoroutine(SelectionSort(_sortingDisplay.Heights)); // Ensure you pass a copy if ListArray is a property
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator SelectionSort(int[] numbers)
        {
            _sortingDisplay.IsSorting = true;

            int n = numbers.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Scan for the smallest value in the unsorted part of the array
                for (int j = i; j < n; j++)
                {
                    if (numbers[j] < numbers[minIndex])
                    {
                        // Color the current smallest value
                        if (_applyColorization)
                        {
                            _sortingDisplay.ChangeColor(numbers[minIndex], Color.white);
                        }

                        minIndex = j;

                        // Color the current smallest value
                        if (_applyColorization)
                        {
                            _sortingDisplay.ChangeColor(numbers[minIndex], Color.blue);
                        }
                    }
                    else if (_applyColorization)
                    {
                        // Color the the current index gray
                        _sortingDisplay.ChangeColorForSeconds(numbers[j], Color.grey, _timeIncrementForScan);
                    }
                    // Delay next iteration
                    yield return new WaitForSeconds(_timeIncrementForScan);
                }

                // Color the minIndex for duration of the swap
                if (_applyColorization)
                    _sortingDisplay.ChangeColorForSeconds(numbers[minIndex], Color.blue, _timeIncrementForSwap * 1.5f);

                // Move minIndex to the end of the sorted part of the array  
                ArrayUtility.BitwiseSwap(numbers, i, minIndex);

                // Update the display
                _sortingDisplay.UpdateDisplay(numbers);
                yield return new WaitForSeconds(_timeIncrementForSwap * 1.75f);
            }

            _sortingDisplay.IsSorting = false;
            print(string.Join(" ", numbers)); // Print the final sorted array
            StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
        }
    }
}