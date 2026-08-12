/*
 * Author: Olivia Chai
 * Date: 10th August 2026
 * File: BusStopCrossPoint.cs
 * Description:
 * Detects the bus to move forward to a certain start and end point.
 */

using UnityEngine;
using System.Collections;
/// <summary>
/// Controls the movement of the bus
/// The bus moves forward then stop at bus stop for 20 seconds then moves to the traffic light and stops there.   
/// </summary>
public class BusMovement : MonoBehaviour
{
    /// <summary>
    /// The position where the bus should stop at the bus stop.
    /// </summary>
    public Transform BusStopPoint;
    public Transform TrafficStopPoint;
    public Transform RightLanePoint;
    public Transform BusEndPoint;

    public float speed = 8f;  //speed of the bus
    public float waitTime = 20f; //time to wait at the bus stop
    public float trafficWaitTime = 10f; //time to wait at the traffic light
    private int stage = 0;
    private bool waiting = false; // checks whether the bus is waiting at the bus stop

    // Update is called once per frame
    void Update()
    {
        // Stage 0: Move towards the bus stop.
        if (stage == 0)
        {
            MoveBus(BusStopPoint);

            if (Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(BusStopPoint.position.x, BusStopPoint.position.z)
            ) < 0.1f && !waiting)
            {
                StartCoroutine(WaitAtBusStop());
            }
        }

        // Stage 1: Move towards the traffic light.
        else if (stage == 1)
        {
            MoveBus(TrafficStopPoint);

            if (Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(TrafficStopPoint.position.x, TrafficStopPoint.position.z)
                ) < 0.1f && !waiting)
            {
                StartCoroutine(WaitAtTrafficLight());
            }
        }
        // Stage 2: Change to the right lane.
        else if (stage == 2)
        {
            MoveBus(RightLanePoint);

            if (Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(RightLanePoint.position.x, RightLanePoint.position.z)
                ) < 0.1f)
            {
                stage = 3;
                Debug.Log("Bus changed to right lane");
            }
        }

        // Stage 3: Continue straight to the bus end point.
        else if (stage == 3)
        {
            MoveBus(BusEndPoint);

            if (Vector2.Distance(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(BusEndPoint.position.x, BusEndPoint.position.z)
                ) < 0.1f)
            {
                stage = 4;
                Debug.Log("Bus reached the bus end point");
            }
        }
    }
    /// <summary>
    /// Moves the bus towards the given target position.
    /// The Y position is kept unchanged so the bus stays on the road.
    /// </summary>
    /// <param name="target">The target point that the bus should move towards.</param>
    void MoveBus(Transform target)
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
    /// Stops the bus at the bus stop for the specified waiting time
    /// before allowing it to continue towards the bus end point.
    /// </summary>
    IEnumerator WaitAtBusStop()
    {
        waiting = true;

        Debug.Log("Bus reached bus stop");

        yield return new WaitForSeconds(waitTime);

        Debug.Log("Bus leaving bus stop");

        stage = 1;
        waiting = false;
    }
    /// <summary>
    /// Stops the bus at the traffic light for the specified waiting time
    /// before allowing it to continue towards the bus end point.
    /// </summary>
    IEnumerator WaitAtTrafficLight()
    {
        waiting = true;

        Debug.Log("Bus stopped at traffic light");

        yield return new WaitForSeconds(trafficWaitTime);

        Debug.Log("Bus leaving traffic light");

        waiting = false;
        stage = 2;
    }
}


