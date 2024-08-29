using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingDisplay : MonoBehaviour
{
    [SerializeField] SortingDisplaySettings _displaySettings;
    private float _squareWidth = 0;
    private float _squareHeightMultiplier = 0;

    // Fields, Getters, and Setters
    private Vector2 _origin;
    private List<GameObject> _rectangles;
    public List<GameObject> Rectangles { get { return _rectangles; } private set { _rectangles = value; } }
    private int[] _heights;
    public int[] Heights { get { return _heights; } set { _heights = value; } }
    private bool _isSorting = false;
    public bool IsSorting { get { return _isSorting; } set { _isSorting = value; } }
    private bool _spacePressedThisFrame;
    public bool SpacePressedThisFrame { get { return _spacePressedThisFrame; } }
    private int[] _oldNumbers;

    private void Start() => InitializeOrReset();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) _spacePressedThisFrame = true;
        else _spacePressedThisFrame = false;

        if (_spacePressedThisFrame)
        {
            foreach (GameObject obj in _rectangles) ChangeColor(obj, Color.white);

            if (_isSorting) _isSorting = false;
            else InitializeOrReset();
        }

        // Print the unsorted array
        if (Input.GetKeyDown(KeyCode.P)) print(string.Join(" ", _heights));
    }

    private void InitializeOrReset()
    {
        // Reset the isSorting boolean
        _isSorting = false;

        // Setup the lists and arrays
        _rectangles?.ForEach(Destroy); // Destroy the objects
        _rectangles = new List<GameObject>(); // Create a new empty list
        _heights = ArrayUtility.GenerateRandomArray(_displaySettings._arrayLength); // Generate a new list of jumbled heights
        _oldNumbers = (int[])_heights.Clone();

        // Calculate the sizing variables
        _squareWidth = _displaySettings._displayWidth / _displaySettings._arrayLength;
        _squareHeightMultiplier = _displaySettings._displayHeight / _displaySettings._arrayLength;

        // Calculate the origin position from which to start spawning the elements
        Vector2 offset = new((_displaySettings._arrayLength * _squareWidth / 2 * -1f) + _squareWidth / 2f, -_displaySettings._displayHeight / 2);
        _origin = (Vector2)transform.position + offset;

        for (int height = 1; height <= _displaySettings._arrayLength; height++)
        {
            int index = Array.IndexOf(_heights, height);

            // Calculate the position where the rectangle should be instantiated
            float x = _origin.x + index * _squareWidth;
            float y = _origin.y + height * _squareHeightMultiplier / 2;
            Vector2 position = new(x, y);

            // Instantiate the rectangle object
            GameObject rectangle = Instantiate(_displaySettings._squarePrefab, position, Quaternion.identity);

            // Set the height of the rectangle
            rectangle.transform.localScale = new(_squareWidth, height * _squareHeightMultiplier, 1);

            // Add the object to the list
            _rectangles.Add(rectangle);
        }
    }

    public void UpdateDisplay(int[] newNumbers)
    {
        for (int n = 1; n <= _displaySettings._arrayLength; n++)
        {
            // Find index of n in newNumbers
            int newIndex = Array.IndexOf(newNumbers, n);

            // Find index of n in oldNumbers
            int oldIndex = Array.IndexOf(_oldNumbers, n);

            // Skip if they match
            if (newIndex == oldIndex) continue;

            // Move to correct position based on the new index
            float x = _origin.x + newIndex * _squareWidth;
            float y = _origin.y + _rectangles[n - 1].transform.localScale.y / 2;
            Vector2 newPosition = new(x, y);

            // Move the rectangle either instantly or with animation
            if (_displaySettings._useAnimatedMoves)
            {
                StartCoroutine(AnimatedMove(_rectangles[n - 1], newPosition, _displaySettings._arcHeight, _displaySettings._animationDuration));
            }
            else
            {
                _rectangles[n - 1].transform.position = newPosition;
            }
        }

        // Update oldNumbers
        _oldNumbers = (int[])newNumbers.Clone();
    }

    private IEnumerator AnimatedMove(GameObject obj, Vector3 targetPos, float arcHeight, float duration)
    {
        Vector3 startPos = obj.transform.position;
        bool movingRight = startPos.x < targetPos.x;

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the interpolation factor
            float t = elapsedTime / duration;

            // Dertemine wether the arc curves up or down
            Vector3 arcDirection = movingRight ? Vector3.up : Vector3.down;

            // Calculate the offset of the current point on the curve from the line between point a and b
            Vector3 offset = (startPos + targetPos) / 2 + arcDirection * arcHeight;

            // Calculate the current bezier curve point
            Vector3 currentPos = Vector3.Lerp(Vector3.Lerp(startPos, offset, t), Vector3.Lerp(offset, new(targetPos.x, startPos.y), t), t);

            // Move the object to the current position
            obj.transform.position = currentPos;

            // Increment the time elapsed
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure final position is exactly the end position
        obj.transform.position = targetPos;
    }

    private void ChangeColor(GameObject obj, Color color)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            obj.GetComponent<SpriteRenderer>().color = color;
        }
    }

    public IEnumerator ChangeColorAfterDelay(GameObject obj, Color color, float delay)
    {
        if (obj.GetComponent<SpriteRenderer>() != null)
        {
            yield return new WaitForSeconds(delay);
            obj.GetComponent<SpriteRenderer>().color = color;
        }

        yield return null;
    }

    public void ChangeColor(int n, Color color)
    {
        if (_rectangles[n - 1].GetComponent<SpriteRenderer>() != null)
        {
            _rectangles[n - 1].GetComponent<SpriteRenderer>().color = color;
        }
    }

    public void ChangeColorForSeconds(int n, Color color, float seconds)
    {
        if (_rectangles[n - 1].GetComponent<SpriteRenderer>() != null)
        {
            _rectangles[n - 1].GetComponent<SpriteRenderer>().color = color;
            StartCoroutine(ChangeColorAfterDelay(_rectangles[n - 1], Color.white, seconds));
        }
    }

    public IEnumerator ChangeColorAfterDelay(int n, Color color, float delay)
    {
        if (_rectangles[n - 1].GetComponent<SpriteRenderer>() == null)
        {
            yield break;
        }

        yield return new WaitForSeconds(delay);
        _rectangles[n - 1].GetComponent<SpriteRenderer>().color = color;
    }

    public IEnumerator SuccesfulSortAnimation()
    {
        float t = _displaySettings._succesfulSortTimeIncrement;

        foreach (GameObject obj in _rectangles)
        {
            if (_isSorting) break;
            ChangeColor(obj, Color.green);
            yield return new WaitForSeconds(t);
        }

        foreach (GameObject obj in _rectangles)
        {
            if (_isSorting) break;
            ChangeColor(obj, Color.white);
            yield return new WaitForSeconds(t);
        }

        foreach (GameObject obj in _rectangles) ChangeColor(obj, Color.white);
    }
}
