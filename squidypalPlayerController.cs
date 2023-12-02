using System.Collections;
using TMPro;
using UnityEngine;

public class squidypalPlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public float slideForce = 10f;
    public float crouchHeight = 0.5f;
    public Camera playerCamera;
    private Vector3 moveInput;
    private Vector3 moveVelocity;
    private Rigidbody rb;
    private bool isJumping = false;
    private bool isCrouching = false;
    private CapsuleCollider capCollider;
    private float originalHeight;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
        originalHeight = capCollider.height;
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0;
        cameraRight.y = 0;

        cameraForward.Normalize();
        cameraRight.Normalize();

        moveInput = cameraForward * moveVertical + cameraRight * moveHorizontal;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveVelocity = moveInput * sprintSpeed;
            if (Input.GetKeyDown(KeyCode.C) && IsGrounded())
            {
                StartSlide();
            }
        }
        else
        {
            moveVelocity = moveInput * speed;
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            ToggleCrouch();
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space) && !isCrouching)
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        // Apply horizontal movement based on velocity, respecting physics
        rb.velocity = new Vector3(moveVelocity.x, rb.velocity.y, moveVelocity.z);

        if (isJumping)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isJumping = false;
        }
    }

    private void ToggleCrouch()
    {
        isCrouching = !isCrouching;
        capCollider.height = isCrouching ? crouchHeight : originalHeight;
    }

    private void StartSlide()
    {
        // Apply an initial slide force in the direction of movement
        rb.AddForce(new Vector3(moveVelocity.x, 0, moveVelocity.z) * slideForce, ForceMode.VelocityChange);
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capCollider.bounds.extents.y + 0.1f);
    }
}
