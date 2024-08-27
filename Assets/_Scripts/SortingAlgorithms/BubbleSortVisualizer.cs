using System.Collections;
using UnityEngine;

namespace SortingAlgorithms
{
    public class BubbleSortVisualizer : MonoBehaviour
    {
        [SerializeField] float _timeIncrementPerIteration = 0.1f;
        [SerializeField] bool _applyColorization = false;
        private SortingDisplay _sortingDisplay;

        private void Start() => _sortingDisplay = GetComponent<SortingDisplay>();

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.B) && !_sortingDisplay.IsSorting)
            {
                StartCoroutine(BubbleSort(_sortingDisplay.Heights)); // Ensure you pass a copy if ListArray is a property
            }

            if (_sortingDisplay.SpacePressedThisFrame)
            {
                StopAllCoroutines();
            }
        }

        private IEnumerator BubbleSort(int[] numbers)
        {
            _sortingDisplay.IsSorting = true;
            int n = numbers.Length;

            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (numbers[j] > numbers[j + 1])
                    {
                        // Swap elements
                        ArrayUtility.Swap(numbers, j + 1, j);
                        //(numbers[j + 1], numbers[j]) = (numbers[j], numbers[j + 1]);

                        if (_applyColorization)
                        {
                            // Color the objects being swapped
                            _sortingDisplay.ChangeColor(numbers[j], Color.blue);
                            _sortingDisplay.ChangeColor(numbers[j + 1], Color.red);
                        }

                        // Update the visualizer
                        _sortingDisplay.UpdateDisplay(numbers); // Ensure CheckChanges can handle this scenario
                    }
                    else
                    {
                        if (_applyColorization)
                        {
                            // Colorize the objects
                            _sortingDisplay.ChangeColor(numbers[j], Color.green);
                            _sortingDisplay.ChangeColor(numbers[j + 1], Color.green);
                        }
                    }

                    yield return new WaitForSeconds(_timeIncrementPerIteration);

                    if (_applyColorization)
                    {
                        // Reset the color of the objects
                        _sortingDisplay.ChangeColor(numbers[j], Color.white);
                        _sortingDisplay.ChangeColor(numbers[j + 1], Color.white);
                    }
                }
            }

            _sortingDisplay.IsSorting = false;
            print(string.Join(" ", numbers)); // Print the final sorted array
            StartCoroutine(_sortingDisplay.SuccesfulSortAnimation());
        }
    }
}
