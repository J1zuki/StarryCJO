/*
 * Author: Olivia Tan
 * Date: 12th August 2026
 * File: CarMovement_02.cs
 * Description:
 * Controls the movement of the second car in the road scene.
 * The second car waits at the traffic light until the first car
 * is allowed to leave, then moves together with the first car
 * towards its designated end point.
 */

using UnityEngine;

/// <summary>
/// Controls the movement of the second car.
/// The car starts at the traffic light, and waits until first car and leave together     
/// waits for few seconds, then moves towards the end point.
/// </summary>
public class CarMovement_02 : MonoBehaviour
{
    /// <summary>
    /// Reference to the first car.
    /// </summary>
    public CarMovement_01 FirstCar;
    public Transform CarEndPoint;
    public float speed = 8f;  //speed of the car
    private bool reachedEndPoint = false; // checks whether the car has reached the end point

    // Update is called once per frame
    void Update()
    {
        // Wait until Car 1 finishes waiting at the traffic light.
        if (FirstCar.canLeaveTraffic && !reachedEndPoint)
        {
            MoveCar(CarEndPoint);

            if (CheckReachedPoint(CarEndPoint))
            {
                reachedEndPoint = true;

                Debug.Log("Car 2 reached the end point");
            }
        }
    }

    /// <summary>
    /// Moves the car towards the end point.
    /// </summary>
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
    bool CheckReachedPoint(Transform target)
    {
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.position.x, target.position.z)
        );

        return distance < 0.1f;
    }
}
