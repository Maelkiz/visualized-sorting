using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/SortingDisplaySettings")]
public class SortingDisplaySettings : ScriptableObject
{
    [Header("Display Settings")]
    public GameObject _squarePrefab;
    public int _arrayLength = 200;
    public float _displayWidth = 12;
    public float _displayHeight = 8;


    [Header("Animation")]
    public bool _useAnimatedMoves = false;
    public float _arcHeight = 0.5f;
    [Tooltip("This value should be equal to or lower than the time per iteration of the sorting algorithm applied.")]
    public float _animationDuration = 0.2f;
    public float _succesfulSortTimeIncrement = 0.005f;
}
