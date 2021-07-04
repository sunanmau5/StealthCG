using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Sunan Regi Maunakea & Louis Sutopo
// Tutorial for spotting the player and drawing Gizmos: https://www.youtube.com/watch?v=jUdx_Nj4Xk0&t
// Changed accordingly to be reusable for both guards (Golems and CCTV).
public class GuardController : MonoBehaviour
{
    public static event System.Action OnGuardHasSpottedPlayer;

    public float timeToSpotPlayer = .5f;

    public Light spotlight;
    public float viewDistance;
    public LayerMask viewMask;
    float viewAngle;
    float playerVisibleTimer;

    Transform player;
    Color originalSpotlightColor;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        viewAngle = spotlight.spotAngle;
        spotlight.range = viewDistance;
        originalSpotlightColor = spotlight.color;
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            playerVisibleTimer += Time.deltaTime;
        }
        else
        {
            playerVisibleTimer -= Time.deltaTime;
        }
        playerVisibleTimer = Mathf.Clamp(playerVisibleTimer, 0, timeToSpotPlayer);
        spotlight.color = Color.Lerp(originalSpotlightColor, Color.red, playerVisibleTimer / timeToSpotPlayer);

        if (playerVisibleTimer >= timeToSpotPlayer)
        {
            if (OnGuardHasSpottedPlayer != null)
            {
                OnGuardHasSpottedPlayer();
            }
        }
    }

    // Returns true if Player is in the view cone of the guard
    bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDistance)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);
            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, viewMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    // Used for debugging the view cone.
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + (transform.forward * viewDistance), 2);
    }
}
