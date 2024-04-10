using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField]
    private Transform targetObject; // The object that will move between waypoints
    
    [SerializeField]
    private Transform waypoint1; // First waypoint
    
    [SerializeField]
    private Transform waypoint2; // Second waypoint

    [SerializeField]
    private float speed = 5f; // Movement speed
    
    [SerializeField]
    private AnimationCurve speedCurve = AnimationCurve.Linear(0, 0, 1, 1); // Speed curve
    
    private float journeyLength;
    private float startTime;
    private bool movingToWaypoint1 = true; // Direction of movement

    void Start()
    {
        if (targetObject == null)
        {
            Debug.LogError("Target object is not assigned!");
            enabled = false;
            return;
        }

        startTime = Time.time;
        journeyLength = Vector3.Distance(waypoint1.position, waypoint2.position);
    }

    void Update()
    {
        if (journeyLength == 0) return;

        // Calculate the current time along the journey
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;

        // Use the speed curve to adjust the fraction
        float curveFraction = speedCurve.Evaluate(fractionOfJourney);

        // Move the target object
        targetObject.position = Vector3.Lerp(movingToWaypoint1 ? waypoint1.position : waypoint2.position,
                                              movingToWaypoint1 ? waypoint2.position : waypoint1.position, curveFraction);

        // Switch direction when reaching a waypoint
        if (curveFraction >= 1)
        {
            movingToWaypoint1 = !movingToWaypoint1;
            startTime = Time.time;
        }
    }

    // Draw gizmos in the editor to visualize the waypoints
    void OnDrawGizmos()
    {
        if (waypoint1 != null && waypoint2 != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(waypoint1.position, 0.05f);
            Gizmos.DrawSphere(waypoint2.position, 0.05f);
            Gizmos.DrawLine(waypoint1.position, waypoint2.position);
        }
    }
}
