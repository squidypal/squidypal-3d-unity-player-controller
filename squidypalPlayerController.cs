using System.Collections;
using Mirror;
using TMPro;
using UnityEngine;

public class squidypalPlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float sprintSpeed = 8f;
    public float jumpForce = 5f;
    public LayerMask groundLayer;
    public Camera playerCamera; // Reference to the player's camera

    public float mouseSensitivity = 100.0f; // Mouse movement sensitivity
    private float cameraRotationX; // Current vertical rotation of the camera
    
    private Rigidbody rb;
    private bool isJumping;
    private CapsuleCollider capCollider;
    private GameObject instantiatedObject;
    private bool cooldown; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        capCollider = GetComponent<CapsuleCollider>(); // Get the CapsuleCollider component
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the direction of movement relative to the camera's rotation
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical);
        direction = playerCamera.transform.rotation * direction;
        direction.y = 0;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            direction *= sprintSpeed * Time.deltaTime;
        }
        else
        {
            direction *= speed * Time.deltaTime;
        }

        rb.MovePosition(transform.position + direction);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space))
        {
            isJumping = true;
        }

        float rotationY = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

        cameraRotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        cameraRotationX = Mathf.Clamp(cameraRotationX, -90, 90);

        playerCamera.transform.localEulerAngles = new Vector3(cameraRotationX, 0, 0);
        transform.Rotate(0, rotationY, 0);
    }
    private void FixedUpdate()
    {
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
