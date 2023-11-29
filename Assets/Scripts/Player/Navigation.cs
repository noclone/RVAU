using UnityEngine;

public class Navigation : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    public float rotationSpeed = 5.0f;
    public Camera camera;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * (Time.deltaTime * walkingSpeed));
        }

        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(Vector3.back * (Time.deltaTime * walkingSpeed));
        }

        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(Vector3.left * (Time.deltaTime * walkingSpeed));
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(Vector3.right * (Time.deltaTime * walkingSpeed));
        }

        yaw += rotationSpeed * Input.GetAxis("Mouse X");
        pitch -= rotationSpeed * Input.GetAxis("Mouse Y");
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);
        transform.eulerAngles = new Vector3(0.0f, yaw, 0.0f);
        camera.transform.eulerAngles = new Vector3(pitch, transform.eulerAngles.y, 0.0f);
    }
}