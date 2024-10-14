using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        ConfigureCursor();
    }

    void Update()
    {
        if (isMovementAllowed)
        {
            HandleMovement();
            HandleCameraRotation();
            HandleCrouching();
        }
    }

    private void ConfigureCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void HandleMovement()
    {
        Vector3 forwardMovement = transform.TransformDirection(Vector3.forward);
        Vector3 sidewaysMovement = transform.TransformDirection(Vector3.right);

        bool isSprinting = Input.GetKey(KeyCode.LeftShift);
        float currentMovementSpeed = isSprinting ? sprintSpeed : walkSpeed;

        float movementForward = Input.GetAxis("Vertical") * currentMovementSpeed;
        float movementSideways = Input.GetAxis("Horizontal") * currentMovementSpeed;

        Vector3 movement = forwardMovement * movementForward + sidewaysMovement * movementSideways;

        if (controller.isGrounded)
        {
            playerVelocity.y = 0;
            if (Input.GetButton("Jump"))
            {
                playerVelocity.y = jumpHeight;
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
        cameraVerticalRotation -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -verticalLookLimit, verticalLookLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(cameraVerticalRotation, 0, 0);

        float cameraHorizontalRotation = Input.GetAxis("Mouse X") * mouseSensitivity;
        transform.Rotate(0, cameraHorizontalRotation, 0);
    }

    private void HandleCrouching()
    {
        if (Input.GetKey(KeyCode.LeftControl))
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
}
