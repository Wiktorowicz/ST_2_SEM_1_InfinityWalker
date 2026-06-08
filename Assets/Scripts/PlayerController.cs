using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;

    [Header("Slide")]
    public float slideHeightMultiplier = 0.5f;

    [Header("Fast Fall")]
    public float fastFallForce = -25f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private bool isSliding;

    private float normalHeight;
    private Vector3 normalCenter;
    private float startZ;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        normalHeight = controller.height;
        normalCenter = controller.center;
        startZ = transform.position.z;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = 0f;

        if (Keyboard.current.aKey.isPressed)
            moveX = -1f;

        if (Keyboard.current.dKey.isPressed)
            moveX = 1f;

        Vector3 move = new Vector3(moveX, 0, 0);
        controller.Move(move * moveSpeed * Time.deltaTime);

        bool jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
        bool slideHeld = Keyboard.current.leftCtrlKey.isPressed;
        bool slidePressed = Keyboard.current.leftCtrlKey.wasPressedThisFrame;

        if (jumpPressed)
        {
            if (isSliding)
                StopSlide();

            if (isGrounded)
                velocity.y = jumpForce;
        }

        if (slidePressed && !isGrounded)
        {
            velocity.y = fastFallForce;
        }

        if (slideHeld && isGrounded)
        {
            StartSlide();
        }
        else
        {
            StopSlide();
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        float clampedX = Mathf.Clamp(transform.position.x, -3f, 3f);

        transform.position = new Vector3(
            clampedX,
            transform.position.y,
            startZ
        );
    }

    private void StartSlide()
    {
        if (isSliding)
            return;

        isSliding = true;

        float slideHeight = normalHeight * slideHeightMultiplier;

        controller.height = slideHeight;
        controller.center = new Vector3(
            normalCenter.x,
            normalCenter.y - (normalHeight - slideHeight) / 2f,
            normalCenter.z
        );

        transform.rotation = Quaternion.Euler(90f, 0f, 0f);
    }

    private void StopSlide()
    {
        if (!isSliding)
            return;

        isSliding = false;

        transform.rotation = Quaternion.identity;

        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            startZ
        );

        controller.height = normalHeight;
        controller.center = normalCenter;
    }
}