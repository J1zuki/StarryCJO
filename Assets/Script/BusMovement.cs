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

    public float speed = 5f;  //speed of the bus
    public float waitTime = 10f; //time to wait at the bus stop
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
            ) < 0.1f)
            {
                stage = 2;
                Debug.Log("Bus stopped at traffic light");
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
        /// before allowing it to continue towards the traffic light.
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

    }
}

