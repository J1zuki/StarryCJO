/*
 * Author: Olivia Tan
 * Date: 12th August 2026
 * File: CarMovement_01.cs
 * Description:
 * Controls the movement of the first car in the road scene.
 * The car moves towards a traffic light stop point,
 * waits for a set amount of time, then continues
 * towards its final end point.
 *
 * The script also uses canLeaveTraffic to signal
 * another car when this car is allowed to leave
 * the traffic light.
 */

using UnityEngine;
using System.Collections;
/// <summary>
/// Controls the movement of the car.
/// The car moves towards the traffic light,
/// stops for few seconds, then continues to the end point.
/// </summary>

public class CarMovement_01 : MonoBehaviour
{
    public Transform TrafficStopPoint_car;
    public Transform CarEndPoint;

    public float speed = 8f;  //speed of the car
    public float trafficWaitTime = 10f; //time to wait at the traffic light
    private int stage = 0; //starting stage of the car movement
    private bool waiting = false; // checks whether the car is waiting at the traffic light
    public bool canLeaveTraffic = false;

    // Update is called once per frame
    void Update()
    {
        // Stage 0: Move towards the traffic light.
        if (stage == 0)
        {
            MoveCar(TrafficStopPoint_car);

            if (CheckReachedPoint(TrafficStopPoint_car) && !waiting)
            {
                StartCoroutine(WaitAtTrafficLight());
            }
        }

        // Stage 1: Move towards the end point.
        else if (stage == 1)
        {
            MoveCar(CarEndPoint);

            if (CheckReachedPoint(CarEndPoint))
            {
                stage = 2;
                Debug.Log("Car reached the end point");
            }
        }
        /// <summary>
        /// Moves the car towards the target point.
        /// </summary>
        /// <param name="target">The point the car moves towards.</param>
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
        /// <param name="target">The point to check.</param>
        /// <returns>True when the car reaches the target.</returns>
        bool CheckReachedPoint(Transform target)
        {
            float distance = Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(target.position.x, target.position.z)
            );

            return distance < 0.1f;
        }

        /// <summary>
        /// Stops the car at the traffic light before it continues moving.
        /// </summary>
        IEnumerator WaitAtTrafficLight()
        {
            waiting = true;

            Debug.Log("Car stopped at traffic light");

            yield return new WaitForSeconds(trafficWaitTime);

            Debug.Log("Car leaving traffic light");

            canLeaveTraffic = true; // Tells Car 2 to move same time as car1

            waiting = false;
            stage = 1;
        }
    }
}
