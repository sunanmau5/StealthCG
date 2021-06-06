using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2;

    [SerializeField] private float sprintSpeed = 5;

    public event System.Action OnReachedEndOfLevel;

    Vector3 velocity;
    bool disabled;


    // Start is called before the first frame update
    void Start()
    {
        GuardController.OnGuardHasSpottedPlayer += Disable;
    }
    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        float movementSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : speed;

        Vector3 move = transform.right * moveX + transform.forward * moveY;
        if (!disabled)
        {
            transform.position += move * Time.deltaTime * movementSpeed;
        }
    }

    void Disable()
    {
        disabled = true;
    }

    void OnTriggerEnter(Collider hitCollider)
    {
        if (hitCollider.tag == "Finish")
        {
            Disable();
            if (OnReachedEndOfLevel != null)
            {
                OnReachedEndOfLevel();
            }
        }
    }

    void OnDestroy()
    {
        GuardController.OnGuardHasSpottedPlayer -= Disable;
    }
}
