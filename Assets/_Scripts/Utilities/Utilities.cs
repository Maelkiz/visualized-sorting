using UnityEngine;

namespace Utilities
{
    public static class Utility
    {
        public static Vector2 GetMousePosition()
        {
            Vector3 mouseWorldCoords = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePosition = new(mouseWorldCoords.x, mouseWorldCoords.y);
            return mousePosition;
        }
        public static Vector2 RandomNormalizedVector2()
        {
            return new Vector2(
                Random.Range(-1, 1),
                Random.Range(-1, 1)
            ).normalized;
        }

        public static Vector3 RandomNormalizedVector3()
        {
            return new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
        }

        public static Vector2 RotateVector(Vector2 vector, float angle)
        {
            // Convert the angle from degrees to radians
            float angleInRadians = angle * Mathf.Deg2Rad;

            // Calculate sine and cosine of the angle
            float sinAngle = Mathf.Sin(angleInRadians);
            float cosAngle = Mathf.Cos(angleInRadians);

            // Perform rotation using trigonometric functions
            Vector2 rotatedVector = new Vector2(
                vector.x * cosAngle - vector.y * sinAngle,
                vector.x * sinAngle + vector.y * cosAngle
            );

            return rotatedVector;
        }
    }
}