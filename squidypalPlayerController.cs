using System.Collections;
using TMPro;
using UnityEngine;

public class squidypalPlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public Camera playerCamera;
    private Vector3 direction;
    private Rigidbody rb;
    private bool isJumping = false;
    private CapsuleCollider capCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera.transform.forward;
        Vector3 cameraRight = playerCamera.transform.right;

        cameraForward.y = 0; // ignore vertical direction of the camera for movement
        cameraRight.y = 0; 

        cameraForward.Normalize();
        cameraRight.Normalize();

        direction = cameraForward * moveVertical + cameraRight * moveHorizontal;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction *= sprintSpeed;
        }
        else
        {
            direction *= speed;
        }

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * Time.deltaTime);

        if (isJumping)
        {
            rb.AddForce(new Vector3(0, jumpForce, 0), ForceMode.Impulse);
            isJumping = false;
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, capCollider.bounds.extents.y + 0.1f);
    }
}
