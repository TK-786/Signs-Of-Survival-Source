using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("References & Components")]
    public Camera playerCamera;
    public GameObject player;
    public PlayerStats playerStats;          // Make sure this is assigned in the Inspector or via code
    public Animator animator;
    public AudioSource footstepAudioSource;
    public Transform camaraLocation;

    // -------------------------------
    //  Add a reference to your original .inputactions asset here.
    //  (In the Inspector, assign the SAME asset you want your PlayerInput to use.)
    // -------------------------------
    [Header("Input Settings")]
    public PlayerInput playerInput;
    public InputActionAsset originalActionAsset; // Assign this in the Inspector

    private CharacterController controller;

    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 3f;         // In "meters"
    public float gravityForce = 9.8f;
    public float verticalLookLimit = 45f;

    [Header("Crouch Settings")]
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchMovementSpeed = 2f;

    [Header("Camera Settings")]
    public float mouseSensitivity = 1f;
    public float cameraDistance = 0.5f;
    public float cameraMinDistance = 0.1f;
    public LayerMask collisionLayer;

    [Header("State Flags")]
    public bool stealth = false;          // Used for SilencePlayer
    public bool isPaused = false;
    private bool isMovementAllowed = true;

    // -- Private fields for storing input values --
    private Vector2 currentMoveInput;
    private Vector2 currentLookInput;
    private bool isSprinting;
    private bool isCrouchingHeld;
    private bool jumpTriggered;

    // -- Internal state --
    private Vector3 playerVelocity = Vector3.zero;
    private float cameraVerticalRotation = 0;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // If playerStats isn't assigned in Inspector, try to get it:
        if (playerStats == null && player != null)
            playerStats = player.GetComponent<PlayerStats>();

        ConfigureCursor();

        // ------------------------------------
        //  IMPORTANT:
        //  Assign the original .inputactions asset here, not a cloned copy.
        //  This ensures you do NOT get a "(Clone)" at runtime.
        // ------------------------------------
        if (playerInput != null && originalActionAsset != null)
        {
            // Set the PlayerInput to use the original asset.
            playerInput.actions = originalActionAsset;
        }

        // Get the saved mouse sensitivity from PlayerPrefs, if any:
        float savedSensitivity = PlayerPrefs.GetFloat("Sensitivity", 1f);
        mouseSensitivity = (savedSensitivity <= 0f) ? 1f : savedSensitivity;
    }

    void Update()
    {
        if (isPaused)
            return;

        if (isMovementAllowed)
        {
            HandleMovement();
            HandleCameraRotation();
            HandleCrouching();
            MovingSound();
            HandleAnimations();
        }
    }

    // -------------------------------
    //      UNITY EVENTS METHODS
    // -------------------------------
    // These methods will be called automatically if your PlayerInput
    // is set to "Invoke Unity Events" and you hook them up in the Inspector.

    public void OnMove(InputAction.CallbackContext context)
    {
        currentMoveInput = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        currentLookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpTriggered = true;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        isSprinting = context.ReadValue<float>() > 0.1f;
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            isCrouchingHeld = true;
        }
        else if (context.canceled)
        {
            isCrouchingHeld = false;
        }
    }

    // Example of taking damage (you can trigger this method on certain events)
    public void SimulateDamage(float damage)
    {
        Debug.Log("Player will take damage: " + damage);
        if (playerStats != null)
        {
            playerStats.TakeDamage(damage);
        }
    }

    // -------------------------------
    //     MOVEMENT & LOOK LOGIC
    // -------------------------------
    private void HandleMovement()
    {
        Vector3 forwardMovement = transform.TransformDirection(Vector3.forward) * currentMoveInput.y;
        Vector3 sidewaysMovement = transform.TransformDirection(Vector3.right) * currentMoveInput.x;
        float currentMovementSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 movement = (forwardMovement + sidewaysMovement) * currentMovementSpeed;

        if (controller.isGrounded)
        {
            if (playerVelocity.y < 0)
            {
                playerVelocity.y = -2f;
            }

            // If jump was triggered
            if (jumpTriggered)
            {
                // v = sqrt(2 * g * jumpHeight)
                playerVelocity.y = Mathf.Sqrt(2f * -Physics.gravity.y * jumpHeight);
                jumpTriggered = false;
            }
        }

        playerVelocity.y += Physics.gravity.y * Time.deltaTime;
        Vector3 finalMovement = movement + Vector3.up * playerVelocity.y;
        controller.Move(finalMovement * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        cameraVerticalRotation -= currentLookInput.y * mouseSensitivity;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -verticalLookLimit, verticalLookLimit);

        transform.Rotate(Vector3.up * currentLookInput.x * mouseSensitivity);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0, 0);
    }

    private void HandleCrouching()
    {
        if (isCrouchingHeld)
        {
            controller.height = crouchHeight;
            walkSpeed = crouchMovementSpeed;
            sprintSpeed = crouchMovementSpeed;
        }
        else
        {
            controller.height = standingHeight;
            walkSpeed = 6f;
            sprintSpeed = 12f;
        }
    }

    // -------------------------------
    //     AUDIO & ANIMATION
    // -------------------------------
    private void MovingSound()
    {
        if (controller.height == crouchHeight)
        {
            footstepAudioSource.Stop();
            return;
        }

        bool isMoving = currentMoveInput != Vector2.zero;

        if (isMoving && isSprinting)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.pitch = 1.2f;
                footstepAudioSource.Play();
            }
        }
        else if (isMoving && controller.height == standingHeight)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.pitch = 1f;
                footstepAudioSource.Play();
            }
        }
        else
        {
            footstepAudioSource.Stop();
        }
    }

    private void HandleAnimations()
    {
        bool isMoving = currentMoveInput != Vector2.zero;
        animator.SetBool("IsMoving", isMoving);
    }

    // -------------------------------
    //       UTILITY METHODS
    // -------------------------------
    private void ConfigureCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void UpdatePosition(Vector3 newPosition)
    {
        controller.enabled = false;

        if (Physics.Raycast(newPosition + Vector3.up, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            newPosition.y = hit.point.y + controller.height / 2f;
        }
        else
        {
            newPosition.y += controller.height / 2f;
        }

        transform.position = newPosition;

        Transform playerModel = transform.Find("Player Model");
        if (playerModel != null)
        {
            playerModel.localPosition = Vector3.zero;
        }

        controller.enabled = true;
        playerVelocity.y = 0;
        playerCamera.transform.position = newPosition + camaraLocation.position;
    }

    public IEnumerator BoostPlayerStats()
    {
        float originalWalkSpeed = walkSpeed;
        float originalSprintSpeed = sprintSpeed;
        float originalJumpHeight = jumpHeight;

        walkSpeed *= 2;
        sprintSpeed *= 2;
        jumpHeight *= 1.5f;

        yield return new WaitForSeconds(30);

        walkSpeed = originalWalkSpeed;
        sprintSpeed = originalSprintSpeed;
        jumpHeight = originalJumpHeight;
    }

    public IEnumerator SilencePlayer()
    {
        bool wasFootstepAudioPlaying = footstepAudioSource.isPlaying;
        stealth = true;

        footstepAudioSource.mute = true;

        if (wasFootstepAudioPlaying)
        {
            footstepAudioSource.Stop();
        }

        yield return new WaitForSeconds(30f);

        footstepAudioSource.mute = false;
        stealth = false;
    }
}
