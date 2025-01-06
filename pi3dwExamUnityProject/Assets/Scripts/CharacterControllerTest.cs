using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ControllerScriptTest : MonoBehaviour
{
    // Movement settings
    public float moveSpeed = 11.5f;
    public float jumpForce = 11f;
    public float gravity = 30f;
    public float mouseSensitivity = 14f;
    public float verticalLookLimitUp = 30f;
    public float verticalLookLimitDown = 40f;
    private float cameraPitch = 0.0f;
    
    private bool wasMovingOnGround = false;
    private float verticalVelocity;
    private bool canMovePlayer = true;
    
    // Bone to attach camera to when ragdoll is enabled
    public Transform ragdollCameraPosition;
    // Time spent following ragdoll
    public float ragdollCameraFollowTime = 7f;
    private bool isRagdoll = false;
    
    // Position & rotation for static camera view
    public Vector3 cameraStaticPosition = new Vector3(0f, 4f, -5f);
    public Vector3 cameraStaticRotation = new Vector3(40f, 0f, 0f);
    
    // Camera settings
    // Cached reference to the camera transform
    private Transform playerCamera;
    // Store the camera's original parent & local transform
    private Transform originalCameraParent;
    // Whether camera rotation is allowed
    private bool canMoveCamera = true;
    
    // Components
    private CharacterController characterController;
    private Rigidbody[] ragdollRigidbodies;
    private GameObject gameOverObject;
    
    private void Awake()
    {
        // Get components
        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        gameOverObject = GameObject.Find("GameOver");
        playerCamera = GetComponentInChildren<Camera>().transform;
        
        
    }

    void Start()
    {
        DisableRagdoll();
        
        // Hide UI canvas
        gameOverObject.SetActive(false);
        
        AudioManager.instance.PlayMusic();

        // Set variable to store the original parent of the camera
        originalCameraParent = playerCamera.parent;

        // Lock cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Starts timer
        TimerController.instance.BeginTimer();
        
    }

    void Update()
    {
        // Can only move camera when not in ragdoll mode
        if (canMoveCamera)
        {
            HandleMouseLook();
        }

        // Can only move when not in ragdoll mode
        if (canMovePlayer)
        {
            HandleMovement();
        }
    }
    // Handles collision with enemy
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Hit by enemy and not in ragdoll mode
        if (hit.gameObject.CompareTag("Enemy") && !isRagdoll)
        {
            // Enable ragdoll
            EnableRagdoll();

            // Disable player and camera movement
            canMovePlayer = false;
            canMoveCamera = false;
            
            // Show game over UI and enable interaction with it
            gameOverObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            // Stop timer
            TimerController.instance.EndTimer();
            
            // Handle sfx and music upon collision
            AudioManager.instance.StopSFX4();
            AudioManager.instance.PlaySFX1(0);
            AudioManager.instance.PlaySFX2(1);
            AudioManager.instance.PlaySFX3(2);
            AudioManager.instance.musicSource.volume = 0.15f;
            
            // Delay camera before going back to original parent
            StartCoroutine(FollowRagdollForSeconds(ragdollCameraFollowTime));
        }
    }

    // Handles visual orientation of player
    private void HandleMouseLook()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate player left/right
        transform.Rotate(Vector3.up, mouseX);

        // Tilt the camera up/down within a certain range
        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -verticalLookLimitDown, verticalLookLimitUp);
        playerCamera.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }
    
    // Handles player movement
    private void HandleMovement()
    {
        // Get movement input
        float moveX = Input.GetAxis("Horizontal") * moveSpeed;
        float moveZ = Input.GetAxis("Vertical")   * moveSpeed;
        
        // Move the player across the X and Z axes
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        // Move the player across the Y axis
        move.y = verticalVelocity;
        // Apply movement
        characterController.Move(move * Time.deltaTime);
        
        // If the player is touching the ground
        if (characterController.isGrounded)
        {
            // Set vertical velocity when grounded
            verticalVelocity = -1f;

            // Jump when the player presses the jump key
            if (Input.GetButtonDown("Jump"))
            {
                // Set vertical velocity to jump force
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // Apply gravity if the player is in the air
            verticalVelocity -= gravity * Time.deltaTime;
        }

        
        // Check if the player is moving
        bool isMoving = (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0);
        // Check if player is grounded
        bool isGrounded = characterController.isGrounded;
        // Check if player is moving and grounded
        bool isMovingOnGround = (isMoving && isGrounded);
        
        // First movement on ground
        if (isMovingOnGround && !wasMovingOnGround)
        {
            // Play footsteps
            AudioManager.instance.PlaySFX4(3);
        }
        // Movement stopped on ground or player is not grounded
        else if ((!isMovingOnGround && wasMovingOnGround) || !isGrounded)
        {
            // Pause footsteps
            AudioManager.instance.PauseSFX4();
        }
        // Update previous movement state
        wasMovingOnGround = isMovingOnGround;
    }
    
    // Disables ragdoll mode
    public void DisableRagdoll()
    {
        // Switch every ragdoll rigidbody back to kinematic
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true;
        }
        // Set to false to use in other methods
        isRagdoll = false;
    }

    // Enables ragdoll mode
    public void EnableRagdoll()
    {
        // Switch every ragdoll rigidbody to non-kinematic
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = false;
        }
        // Set to true to use in other methods
        isRagdoll = true;
    }

    // Follow ragdoll for a certain time before returning camera to original parent
    private IEnumerator FollowRagdollForSeconds(float duration)
    {

        // Parent camera to the ragdoll head
        playerCamera.SetParent(ragdollCameraPosition);

        // Wait before executing the rest of the method
        yield return new WaitForSeconds(duration);

        // Move camera to an offset
        OffsetCamera();
    }
    
    // Method to handle camera offset
    private void OffsetCamera()
    {
        // Return camera to the original parent
        playerCamera.SetParent(originalCameraParent);

        // Adjust position & rotation of camera
        playerCamera.localPosition   = cameraStaticPosition;
        playerCamera.localEulerAngles = cameraStaticRotation;
    }
}