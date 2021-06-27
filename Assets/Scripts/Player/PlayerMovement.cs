using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2;

    [SerializeField] private float sprintSpeed = 6;

    private bool IS_CHEAT_ON = true;

    private Animator animator;
    private StaminaManager staminaManager;
    private int isWalkingHash;
    private int isRunningHash;
    private int loseGameHash;

    private float stamina = 1f;
    public float staminaDepletionRate = 2; // duration until stamina is empty
    public float staminaRegenRate = 5; // duration until stamina is back


    public event System.Action OnReachedEndOfLevel;

    Vector3 velocity;
    bool disabled;


    // Start is called before the first frame update
    void Start()
    {
        GuardController.OnGuardHasSpottedPlayer += Disable;
        animator = GetComponent<Animator>();
        staminaManager = FindObjectOfType<StaminaManager>();
        // get animator parameters
        isWalkingHash = Animator.StringToHash("IsWalking");
        isRunningHash = Animator.StringToHash("IsRunning");
        loseGameHash = Animator.StringToHash("LoseGame");

    }
    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        // moveY: move vertically
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift);
        float movementSpeed = speed;
        float time = Time.deltaTime;
        float currStamina = stamina; // temp variable to save the value of stamina before updating

        if (shiftPressed)
        {
            movementSpeed = stamina == 0 ? speed : sprintSpeed;
            if (!IS_CHEAT_ON)
            {
                stamina -= (1 / staminaDepletionRate) * time;
            }
        }
        else
        {
            if (stamina != 1 && IS_CHEAT_ON)
            {
                stamina += 1 / staminaRegenRate * time;
            }
        }

        stamina = Mathf.Clamp01(stamina);

        // check if stamina is modified
        if (stamina != currStamina)
        {
            UpdateStaminaUI();
        }

        UpdateAnimation(moveY != 0, movementSpeed != speed);
        Vector3 move = transform.right * moveX + transform.forward * moveY;

        if (!disabled)
        {
            transform.position += move * Time.deltaTime * movementSpeed;
        }
    }

    void UpdateAnimation(bool isMoving, bool shiftPressed)
    {
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isRunning = animator.GetBool(isRunningHash);

        if (!isWalking && isMoving)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (isWalking && !isMoving)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isRunning && shiftPressed)
        {
            animator.SetBool(isRunningHash, true);
        }

        if (isRunning && !shiftPressed)
        {
            animator.SetBool(isRunningHash, false);
        }
    }

    void Disable()
    {
        animator.SetBool(loseGameHash, true);
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

    void UpdateStaminaUI()
    {
        staminaManager.OnStaminaUpdate(stamina);
    }
}
