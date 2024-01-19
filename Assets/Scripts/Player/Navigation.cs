using UnityEngine;

public class Navigation : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    public float sideShiftSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float jumpDuration = 1.0f;
    public Rigidbody Rigidbody;

    // -1: left, 0: middle, 1: right
    private int currentLane;
    private int targetLane;
    private bool isMoving;
    private bool isJumping;
    private float jumpTimer;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        MoveForward();

        if (Input.GetKeyDown(KeyCode.A) && ((!isMoving && currentLane > -1) || (isMoving && targetLane > -1)))
        {
            MoveToLane(targetLane - 1);
        }

        if (Input.GetKeyDown(KeyCode.D) && ((!isMoving && currentLane < 1) || (isMoving && targetLane < 1)))
        {
            MoveToLane(targetLane + 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        if (isJumping)
        {
            UpdateJump();
        }

        if (isMoving)
        {
            MoveLaterally();
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * walkingSpeed));
    }

    void MoveLaterally()
    {
        Vector3 targetPosition;
        if (currentLane == 0)
            targetPosition = targetLane == -1 ? Vector3.left : Vector3.right;
        else
            targetPosition = currentLane == -1 ? Vector3.right : Vector3.left;
        transform.Translate(targetPosition * (Time.deltaTime * sideShiftSpeed));

        if (transform.position.x >= 3)
        {
            transform.position = new Vector3(3, transform.position.y, transform.position.z);
            currentLane = 1;
            isMoving = false;
        }
        else if (transform.position.x <= -3)
        {
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
            currentLane = -1;
            isMoving = false;
        }
        else if (targetLane == 0 && transform.position.x >= -0.1 && transform.position.x <= 0.1)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            currentLane = 0;
            isMoving = false;
        }
    }

    void MoveToLane(int lane)
    {
        if (isMoving)
            currentLane = targetLane;
        targetLane = lane;
        isMoving = true;
    }

    void StartJump()
    {
        Rigidbody.velocity = new Vector3(0, jumpForce, 0);
        isJumping = true;
        jumpTimer = 0.0f;
    }

    void UpdateJump()
    {
        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpDuration)
        {
            isJumping = false;
        }
    }
}