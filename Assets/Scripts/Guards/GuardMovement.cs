using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author 
// Tutorial on how to follow a path: https://www.youtube.com/watch?v=jUdx_Nj4Xk0&t
// Tutorial on how to use Animator: https://www.youtube.com/watch?v=vApG8aYD5aI&t=283s&ab_channel=iHeartGameDev
public class GuardMovement : MonoBehaviour
{
    public float viewDistance;
    public float waitTime = .3f;
    public Transform pathHolder;
    public float turnSpeed = 90;
    public float speed = 5f;

    private Animator animator;
    private int isWalkingHash;
    private int loseGameHash;

    private bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // get animator parameters
        isWalkingHash = Animator.StringToHash("IsWalking");
        loseGameHash = Animator.StringToHash("LoseGame");

        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }
        StartCoroutine(FollowPath(waypoints));
    }

    void Update()
    {
        UpdateAnimation(isMoving);
    }

    // Update the guard animation when moving and idling
    void UpdateAnimation(bool isMoving)
    {
        bool isWalking = animator.GetBool(isWalkingHash);

        if (!isWalking && isMoving)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !isMoving)
        {
            animator.SetBool(isWalkingHash, false);
        }
    }

    // Transform the position of the GameObject to follow the waypoints
    IEnumerator FollowPath(Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
            isMoving = true;
            if (transform.position == targetWaypoint)
            {
                targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
                targetWaypoint = waypoints[targetWaypointIndex];
                isMoving = false;
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(TurnToFace(targetWaypoint));
            }
            yield return null;
        }
    }

    // Rotate the GameObject to face the lookTarget
    IEnumerator TurnToFace(Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.05f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    // Show waypoints in Editor mode
    void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 prevPosition = startPosition;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, .3f);
            Gizmos.DrawLine(prevPosition, waypoint.position);
            prevPosition = waypoint.position;
        }
        Gizmos.DrawLine(prevPosition, startPosition);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDistance);
    }
}
