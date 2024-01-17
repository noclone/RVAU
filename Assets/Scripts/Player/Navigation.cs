using UnityEngine;

public class Navigation : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    public float sideShiftSpeed = 5.0f;
    public float sideShiftDistance = 3.0f; // Distance à parcourir lors du déplacement latéral
    public Camera camera;

    // -1: left, 0: middle, 1: right
    private int currentLane = 0;
    private Vector3 targetPosition;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * (Time.deltaTime * walkingSpeed));

        if (Input.GetKeyDown(KeyCode.A) && currentLane >= 0)
        {
            MoveToLane(currentLane - 1);
        }

        if (Input.GetKeyDown(KeyCode.D) && currentLane < 1)
        {
            MoveToLane(currentLane + 1);
        }

        if (transform.position.x != targetPosition.x)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * sideShiftSpeed);
        }
    }

    void MoveToLane(int lane)
    {
        currentLane = lane;
        targetPosition = new Vector3(currentLane * sideShiftDistance, transform.position.y, transform.position.z);
    }
}