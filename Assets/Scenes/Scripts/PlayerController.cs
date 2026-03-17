using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform cameraPivot;

    [Header("Movement")]
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float sprintSpeed = 9f;
    [SerializeField] float acceleration = 14f;

    [Header("Jumping")]
    [SerializeField] float jumpHeight = 1.6f;
    [SerializeField] float gravity = -20f;

    [Header("Mouse Look")]
    [SerializeField] float mouseSensitivity = 0.12f;
    [SerializeField] float verticalClamp = 80f;

    [Header("Grounding")]
    [SerializeField] LayerMask groundMask;

    CharacterController controller;

    Vector2 moveInput;
    Vector2 lookInput;

    Vector3 velocity;
    float verticalVelocity;

    bool isSprinting;
    bool isGrounded;

    float cameraXRotation;

    void Awake()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        GroundCheck();
        HandleMovement();
        ApplyGravity();
        HandleLook();
    }

    void GroundCheck()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && verticalVelocity < 0)
            verticalVelocity = -2f;
    }

    void HandleMovement()
    {
        Vector3 move =
            transform.right * moveInput.x +
            transform.forward * moveInput.y;

        float speed = isSprinting ? sprintSpeed : walkSpeed;
    
        Vector3 targetVelocity = move * speed;

        velocity = Vector3.Lerp(
            velocity,
            targetVelocity,
            acceleration * Time.deltaTime
        );

        controller.Move(velocity * Time.deltaTime);
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 gravityMove = Vector3.up * verticalVelocity;
        controller.Move(gravityMove * Time.deltaTime);
    }

    void HandleLook()
    {
        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraXRotation -= mouseY;
        cameraXRotation = Mathf.Clamp(cameraXRotation, -verticalClamp, verticalClamp);

        cameraPivot.localRotation = Quaternion.Euler(cameraXRotation, 0, 0);
    }

    void Jump()
    {
        if (!isGrounded) return;

        verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    // INPUT SYSTEM CALLBACKS

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
        Debug.Log("Sprint: " + isSprinting);
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
            Jump();
    }

    [SerializeField] WeaponManager weaponManager;

    void OnAttack(InputValue value)
    {
        if (value.isPressed)
            weaponManager.Fire();
        
    }
    void OnReload(InputValue value)
    {
        if (value.isPressed)
            weaponManager.Reload();
    }
}