using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: https://www.youtube.com/watch?v=rO19dA2jksk
public class PlatformAttach : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
        }
    }

    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}
