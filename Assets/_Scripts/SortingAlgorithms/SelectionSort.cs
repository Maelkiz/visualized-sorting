using System;
using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class SelectionVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementPerIteration = 0.1f;
        //[SerializeField] bool _applyColorization = false;
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

                for (int j = i; j < n; j++)
                {
                    if (numbers[j] < numbers[minIndex])
                    {
                        minIndex = j;
                    }
                    yield return new WaitForSeconds(_timeIncrementPerIteration);
                }

                ArrayUtility.BitwiseSwap(numbers, i, minIndex);
                _sortingDisplay.UpdateDisplay(numbers);
                yield return new WaitForSeconds(_timeIncrementPerIteration);
            }

            _sortingDisplay.IsSorting = false;
            print(string.Join(" ", numbers)); // Print the final sorted array
            StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
        }
    }
}