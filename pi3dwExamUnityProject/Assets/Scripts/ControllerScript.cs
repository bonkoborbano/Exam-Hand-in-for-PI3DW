using System;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed;
    public float jumpForce;
    public float gravity;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 3.0f;
    public float verticalLookLimitUp = 80f;
    public float verticalLookLimitDown = 80f;

    private CharacterController characterController;
    private float verticalVelocity;
    private float cameraPitch = 0.0f;
    private Transform playerCamera;

    Rigidbody [] ragdollRigidbodies;

    private void Awake()
    {
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        DisableRagdoll();
    }

    void Start()
    {
        // Cache references
        characterController = GetComponent<CharacterController>();
        
        // Try to find the camera if it's a child of the player
        playerCamera = GetComponentInChildren<Camera>().transform;
        
        // Lock cursor for FPS-style gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
    }

    void Update()
    {
        // If there's no camera, return early
        if (playerCamera == null) 
            return;

        // Handle mouse rotation
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate character left-right
        transform.Rotate(Vector3.up * mouseX);

        // Pitch the camera up-down and clamp
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalLookLimitDown, verticalLookLimitUp);
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);

        // Check if grounded
        if (characterController.isGrounded)
        {
            verticalVelocity = -1f;  // Small negative to keep grounded

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Apply gravity when not grounded
            verticalVelocity -= gravity * Time.deltaTime;
        }

        // Movement input
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical")   * moveSpeed;

        // Combine movement
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        move.y = verticalVelocity;

        // Move character
        characterController.Move(move * Time.deltaTime);
        
    }
    
    public void DisableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = true;
        }
    }

    public void EnableRagdoll()
    {
        foreach (var rigidbody in ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Example check: If we collide with something tagged "Obstacle," do two things:
        // 1) Move the camera offset
        // 2) Trigger ragdoll
        if (hit.gameObject.CompareTag("Enemy"))
        {
            EnableRagdoll();
        }
    }
}