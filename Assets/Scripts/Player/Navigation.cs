using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;
using XRController = UnityEngine.XR.Interaction.Toolkit.XRController;
using Photon.Pun;

public class Navigation : MonoBehaviour
{
    public float walkingSpeed = 5.0f;
    public float sideShiftSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public Rigidbody Rigidbody;
    public Animator animator;

    // -1: left, 0: middle, 1: right
    private int currentLane;
    private int targetLane;
    private bool isMovingForward = true;
    private bool isMovingLateral;
    private bool isJumping;
    private float jumpTimer;

    private XRController leftController;
    private XRController rightController;

    private bool isLeftPressed;
    private bool isRightPressed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        leftController = GameObject.Find("Left Controller").GetComponent<XRController>();
        rightController = GameObject.Find("Right Controller").GetComponent<XRController>();
    }

    void Update()
    {
        if (isMovingForward)
            MoveForward();

        if (isLeftPressed && leftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isLeftPressedValue) && !isLeftPressedValue)
        {
            isLeftPressed = false;
        }

        if (isRightPressed && rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool isRightPressedValue) && !isRightPressedValue)
        {
            isRightPressed = false;
        }

        if ((Input.GetKeyDown(KeyCode.A) || (leftController != null && leftController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool leftButtonPressed) && leftButtonPressed && !isLeftPressed))
            && ((!isMovingLateral && currentLane > -1) || (isMovingLateral && targetLane > -1)))
        {
            isLeftPressed = true;
            MoveToLane(targetLane - 1);
        }

        if ((Input.GetKeyDown(KeyCode.D) || (rightController != null && rightController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool rightButtonPressed) && rightButtonPressed && !isRightPressed))
            && ((!isMovingLateral && currentLane < 1) || (isMovingLateral && targetLane < 1)))
        {
            isRightPressed = true;
            MoveToLane(targetLane + 1);
        }

        if ((Input.GetKeyDown(KeyCode.Space) || ((leftController != null &&
                                                  leftController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton,
                                                      out bool leftSecondaryButtonPressed) && leftSecondaryButtonPressed)
                                                 || (rightController != null &&
                                                     rightController.inputDevice.TryGetFeatureValue(CommonUsages.secondaryButton,
                                                         out bool rightSecondaryButtonPressed) && rightSecondaryButtonPressed)))
            && !isJumping)
            {
                Jump();
            }

        if (isMovingLateral)
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
            isMovingLateral = false;
        }
        else if (transform.position.x <= -3)
        {
            transform.position = new Vector3(-3, transform.position.y, transform.position.z);
            currentLane = -1;
            isMovingLateral = false;
        }
        else if (targetLane == 0 && transform.position.x >= -0.1 && transform.position.x <= 0.1)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
            currentLane = 0;
            isMovingLateral = false;
        }
    }

    void MoveToLane(int lane)
    {
        if (isMovingLateral)
            currentLane = targetLane;
        targetLane = lane;
        isMovingLateral = true;
    }
    void Jump()
    {
        animator.SetBool("isJumping", true);
        Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z);
        Rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isJumping = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
            animator.SetBool("isJumping", false);
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            isMovingForward = false;
            animator.SetBool("isHitting", true);
            Rigidbody.velocity = new Vector3(0, 0, 0);
            FindObjectOfType<GameEngine>().EndGame();

            PhotonView.Get(this).RPC("StopPCPlayer", RpcTarget.All);
        }
    }

    [PunRPC]
    private void StopPCPlayer()
    {
        GameObject.Find("Player(Clone)").GetComponent<NavigationPC>().StopMoving();
    }

    public void ResetAll()
    {
        currentLane = 0;
        targetLane = 0;
        isMovingForward = true;
        isJumping = false;
        animator.SetBool("isHitting", false);
        animator.SetBool("isJumping", false);
        animator.Play("Running");
    }
}