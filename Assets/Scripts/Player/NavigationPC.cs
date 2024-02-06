using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

public class NavigationPC : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    private bool isMovingForward = false;
    private float startTime;
    private bool started = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startTime = Time.time;
    }

    void Update()
    {
        if (!started && Time.time - startTime > 1.5f)
        {
            isMovingForward = true;
            started = true;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            walkingSpeed = 15.0f;
        }

        else if (Input.GetKeyDown(KeyCode.S))
        {
            walkingSpeed = -8.0f;
        }

        else if (Input.GetKeyDown(KeyCode.Space))
        {
            walkingSpeed = 8.0f;
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkingSpeed = 0.0f;
        }

        if (isMovingForward)
            MoveForward();
    }

    void MoveForward()
    {
        transform.position += Vector3.forward * walkingSpeed * Time.deltaTime;
    }

    public void StopMoving()
    {
        isMovingForward = false;
    }

    public void ResetAll()
    {
        isMovingForward = false;
        startTime = Time.time;
        started = false;
    }
}