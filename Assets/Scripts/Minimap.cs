using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: https://www.youtube.com/watch?v=28JTTXqMvOU
public class Minimap : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
    }
}
