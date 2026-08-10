using UnityEngine;

/// <summary>
/// Controls the movement of the third car.
/// The car starts moving immediately when the game begins and continues towards the end point.
/// </summary>

public class CarMovement_03 : MonoBehaviour
{
    /// <summary>
    /// The final position where the car should move to.
    /// </summary>
    public Transform CarEndPoint;

    public float speed = 8f; // Speed of the car

    private bool reachedEnd = false; // Checks whether the car reached the end point

    /// <summary>
    /// Updates the movement of the car every frame.
    /// </summary>
    void Update()
    {
        if (!reachedEnd)
        {
            MoveCar(CarEndPoint);

            if (CheckReachedPoint(CarEndPoint))
            {
                reachedEnd = true;

                Debug.Log("Car reached the end point");
            }
        }
    }

    /// <summary>
    /// Moves the car towards the target point.
    /// The Y position is kept unchanged so the car stays on the road.
    /// </summary>
    /// <param name="target">The target point that the car moves towards.</param>
    void MoveCar(Transform target)
    {
        Vector3 targetPosition = new Vector3(
            target.position.x,
            transform.position.y,
            target.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );
    }

    /// <summary>
    /// Checks whether the car has reached the target point.
    /// </summary>
    /// <param name="target">The target point to check.</param>
    /// <returns>True when the car reaches the target point.</returns>
    bool CheckReachedPoint(Transform target)
    {
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.position.x, target.position.z)
        );

        return distance < 0.1f;
    }
}
