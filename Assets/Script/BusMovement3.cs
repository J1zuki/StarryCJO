using UnityEngine;
using System.Collections;

public class BusMovement3 : MonoBehaviour
{
    public Transform BusStopPoint;
    public Transform BusEndPoint;

    public float speed = 8f;
    public float busStopWaitTime = 10f;

    private int stage = 0;
    private bool waiting = false;

    void Update()
    {
        // Stage 0: Move towards bus stop
        if (stage == 0)
        {
            MoveBus(BusStopPoint);

            if (ReachedPoint(BusStopPoint) && !waiting)
            {
                StartCoroutine(WaitAtBusStop());
            }
        }

        // Stage 1: Move towards end point
        else if (stage == 1)
        {
            MoveBus(BusEndPoint);

            if (ReachedPoint(BusEndPoint))
            {
                stage = 2;
                Debug.Log("Bus reached end point");
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

    IEnumerator WaitAtBusStop()
    {
        waiting = true;

        Debug.Log("Bus stopped at bus stop");

        yield return new WaitForSeconds(busStopWaitTime);

        Debug.Log("Bus leaving bus stop");

        stage = 1;
        waiting = false;
    }
}