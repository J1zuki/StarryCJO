/*
 * Author: Olivia Tan
 * Date: 10th August 2026
 * File: BusStopCrossPoint.cs
 * Description:
 * Detects the bus to move forward to a certain start and end point.
 */

using UnityEngine;
using System.Collections;

/// <summary>
/// Controls the movement of the opposite-direction bus.
/// Route:
/// Traffic Light -> Bus Stop -> End Point
/// </summary>
public class BusMovement2 : MonoBehaviour
{
    public Transform TrafficStopPoint;
    public Transform BusStopPoint;
    public Transform BusEndPoint;

    public float speed = 8f;
    public float trafficWaitTime = 10f;
    public float busStopWaitTime = 20f;

    private int stage = 0;
    private bool waiting = false;

    void Update()
    {
        // Stage 0: Move to traffic light
        if (stage == 0)
        {
            MoveBus(TrafficStopPoint);

            if (ReachedPoint(TrafficStopPoint) && !waiting)
            {
                StartCoroutine(WaitAtTrafficLight());
            }
        }

        // Stage 1: Move to bus stop
        else if (stage == 1)
        {
            MoveBus(BusStopPoint);

            if (ReachedPoint(BusStopPoint) && !waiting)
            {
                StartCoroutine(WaitAtBusStop());
            }
        }

        // Stage 2: Move to end point
        else if (stage == 2)
        {
            MoveBus(BusEndPoint);

            if (ReachedPoint(BusEndPoint))
            {
                stage = 3;
                Debug.Log("Bus 2 reached end point");
            }
        }
    }

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

    bool ReachedPoint(Transform target)
    {
        float distance = Vector2.Distance(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(target.position.x, target.position.z)
        );

        return distance < 0.1f;
    }

    IEnumerator WaitAtTrafficLight()
    {
        waiting = true;

        Debug.Log("Bus 2 stopped at traffic light");

        yield return new WaitForSeconds(trafficWaitTime);

        Debug.Log("Bus 2 leaving traffic light");

        stage = 1;
        waiting = false;
    }

    IEnumerator WaitAtBusStop()
    {
        waiting = true;

        Debug.Log("Bus 2 reached bus stop");

        yield return new WaitForSeconds(busStopWaitTime);

        Debug.Log("Bus 2 leaving bus stop");

        stage = 2;
        waiting = false;
    }
}
