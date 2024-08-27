using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class InsertionSortVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementPerIteration = 0.1f;
        [SerializeField] bool _applyColorization = false;
        private SortingDisplay _sortingDisplay;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I) && !_sortingDisplay.IsSorting)
            {
                StartCoroutine(InsertionSort(_sortingDisplay.Heights)); // Ensure you pass a copy if ListArray is a property
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator InsertionSort(int[] numbers)
        {
            _sortingDisplay.IsSorting = true;

            int n = numbers.Length;
            for (int i = 1; i < n; i++)
            {
                int key = numbers[i];
                int j = i - 1;

                if (_applyColorization)
                {
                    // Color the key
                    _sortingDisplay.ChangeColor(key, Color.red);
                }

                // Move squares that are greater than the key one position ahead
                while (j >= 0 && numbers[j] > key)
                {
                    if (_applyColorization)
                    {
                        // Color the objects being swapped
                        _sortingDisplay.ChangeColor(numbers[j], Color.blue);
                    }

                    ArrayUtility.Swap(numbers, j, j + 1);

                    _sortingDisplay.UpdateDisplay(numbers);
                    yield return new WaitForSeconds(_timeIncrementPerIteration);

                    if (_applyColorization)
                    {
                        // Reset the color of the objects
                        _sortingDisplay.ChangeColor(numbers[j + 1], Color.white);
                    }

                    j--;
                }

                yield return new WaitForSeconds(_timeIncrementPerIteration);

                if (_applyColorization)
                {
                    // Reset the color of the key
                    _sortingDisplay.ChangeColor(key, Color.white);
                }
            }

            _sortingDisplay.IsSorting = false;
            print(string.Join(" ", numbers)); // Print the final sorted array
            StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
        }
    }
}