using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;
    public float jumpHeight = 10f;
    public float gravityForce = 25f;
    public float mouseSensitivity = 1f;
    public float verticalLookLimit = 45f;
    public float standingHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchMovementSpeed = 2f;

    private Vector3 playerVelocity = Vector3.zero;
    private float cameraVerticalRotation = 0;
    private CharacterController controller;
    private bool isMovementAllowed = true;

    public float cameraDistance = 0.5f;
    public float cameraMinDistance = 0.1f;
    public LayerMask collisionLayer;
    public Transform camaraLocation;

    public PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction lookAction;
    private InputAction crouchAction;

    public AudioSource footstepAudioSource;

    public GameObject player;

    public PlayerStats playerStats;
    public bool stealth = false;
    public bool isPaused = false;


    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerStats = player.GetComponent<PlayerStats>();
        ConfigureCursor();

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        sprintAction = playerInput.actions["Sprint"];
        lookAction = playerInput.actions["Look"];
        crouchAction = playerInput.actions["Crouch"];
    }

    void Update()
    {
        if (isPaused)
        {
            return;
        }

        if (isMovementAllowed)
        {
            HandleMovement();
            HandleCameraRotation();
            HandleCrouching();
            MovingSound();
        }
    }

    // Example of taking damage (you can trigger this method on certain events)
    public void SimulateDamage(float damage)
    {
        Debug.Log("player will take demage");
        playerStats.TakeDamage(damage);
    }


    private void MovingSound()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        bool isSprinting = sprintAction.ReadValue<float>() > 0;

        if (controller.height == crouchHeight)
        {
            footstepAudioSource.Stop();
            return;
        }

        if (isSprinting && input != Vector2.zero)
        {
            if (!footstepAudioSource.isPlaying)
            {
                footstepAudioSource.pitch = 1.2f;
                footstepAudioSource.Play();
            }
        }

        else if (controller.height == standingHeight && input != Vector2.zero)
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



    private void ConfigureCursor()
    {
        // Locks the cursor to the center of the screen and makes it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleMovement()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 forwardMovement = transform.TransformDirection(Vector3.forward) * input.y;
        Vector3 sidewaysMovement = transform.TransformDirection(Vector3.right) * input.x;

        bool isSprinting = sprintAction.ReadValue<float>() > 0; // Detect sprint action
        float currentMovementSpeed = isSprinting ? sprintSpeed : walkSpeed;

        Vector3 movement = (forwardMovement + sidewaysMovement) * currentMovementSpeed;

        if (controller.isGrounded)
        {
            playerVelocity.y = 0;
            if (jumpAction.triggered)
            {
                playerVelocity.y = Mathf.Sqrt(jumpHeight * 2f * gravityForce);
            }
        }
        else
        {
            playerVelocity.y -= gravityForce * Time.deltaTime;
        }

        Vector3 finalMovement = movement + Vector3.up * playerVelocity.y;
        controller.Move(finalMovement * Time.deltaTime);
    }

    private void HandleCameraRotation()
    {
        Vector2 lookInput = lookAction.ReadValue<Vector2>();

        cameraVerticalRotation -= lookInput.y * mouseSensitivity;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -verticalLookLimit, verticalLookLimit);

        transform.Rotate(Vector3.up * lookInput.x * mouseSensitivity);

        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0, 0);
    }

    private void HandleCrouching()
    {
        bool isCrouching = crouchAction.ReadValue<float>() > 0;

        if (isCrouching)
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


    public void UpdatePosition(Vector3 newPosition)
    {
        // Updates the player's position, adjusting for collisions and terrain height
        controller.enabled = false;

        RaycastHit hit;
        if (Physics.Raycast(newPosition + Vector3.up, Vector3.down, out hit, Mathf.Infinity))
        {
            newPosition.y = hit.point.y + controller.height / 2f;
        }
        else
        {
            newPosition.y += controller.height / 2f;
        }

        transform.position = newPosition;

        // Resets player model position if it exists
        Transform playerModel = transform.Find("Player Model");
        if (playerModel != null)
        {
            playerModel.localPosition = Vector3.zero;
        }

        controller.enabled = true;

        playerVelocity.y = 0;

        // Adjusts the camera position based on the new player position
        Vector3 cameraOffset = transform.forward;
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
