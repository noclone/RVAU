using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

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

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (isMovingForward)
            MoveForward();

        if (Input.GetKeyDown(KeyCode.A) && ((!isMovingLateral && currentLane > -1) || (isMovingLateral && targetLane > -1)))
        {
            MoveToLane(targetLane - 1);
        }

        if (Input.GetKeyDown(KeyCode.D) && ((!isMovingLateral && currentLane < 1) || (isMovingLateral && targetLane < 1)))
        {
            MoveToLane(targetLane + 1);
        }

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            // To be removed
            GameObject.Find("GameEngine").GetComponent<GameEngine>().LoadColorMiniGame();



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
        Rigidbody.velocity = new Vector3(0, jumpForce, 0);
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
        }
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