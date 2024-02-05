using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;

public class NavigationPC : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    private bool isMovingForward = false;
    private float startTime;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        startTime = Time.time;
    }

    void Update()
    {
        if (!isMovingForward && Time.time - startTime > 1.5f)
        {
            isMovingForward = true;
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
    }
}