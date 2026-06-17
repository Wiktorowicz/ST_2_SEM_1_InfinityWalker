using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float jumpForce = 10f;
    public float gravity = -20f;

    [Header("Slide")]
    public float slideHeightMultiplier = 0.3f;

    [Header("Fast Fall")]
    public float fastFallForce = -25f;

    private CharacterController controller;
    private CapsuleCollider capsule;
    private Animator animator;

    private Vector3 velocity;
    private bool isGrounded;
    private bool isSliding;

    public bool IsGrounded => isGrounded;

    private float normalControllerHeight;
    private Vector3 normalControllerCenter;

    private float normalCapsuleHeight;
    private Vector3 normalCapsuleCenter;

    private float startZ;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        capsule = GetComponent<CapsuleCollider>();
        animator = GetComponentInChildren<Animator>();

        normalControllerHeight = controller.height;
        normalControllerCenter = controller.center;

        normalCapsuleHeight = capsule.height;
        normalCapsuleCenter = capsule.center;

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

        bool jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.wKey.wasPressedThisFrame;
        bool slideHeld = Keyboard.current.leftCtrlKey.isPressed || Keyboard.current.sKey.isPressed;
        bool slidePressed = Keyboard.current.leftCtrlKey.wasPressedThisFrame || Keyboard.current.sKey.wasPressedThisFrame;

        if (isGrounded)
            animator.SetBool("IsJumping", false);

        if (jumpPressed)
        {
            animator.SetBool("IsJumping", true);

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
        animator.SetBool("IsSliding", true);

        float controllerSlideHeight = normalControllerHeight * slideHeightMultiplier;
        controller.height = controllerSlideHeight;
        controller.center = new Vector3(
            normalControllerCenter.x,
            normalControllerCenter.y - (normalControllerHeight - controllerSlideHeight) / 2f,
            normalControllerCenter.z
        );

        float capsuleSlideHeight = normalCapsuleHeight * slideHeightMultiplier;
        capsule.height = capsuleSlideHeight;
        capsule.center = new Vector3(
            normalCapsuleCenter.x,
            normalCapsuleCenter.y - (normalCapsuleHeight - capsuleSlideHeight) / 2f,
            normalCapsuleCenter.z
        );
    }

    private void StopSlide()
    {
        if (!isSliding)
            return;

        isSliding = false;
        animator.SetBool("IsSliding", false);

        controller.height = normalControllerHeight;
        controller.center = normalControllerCenter;

        capsule.height = normalCapsuleHeight;
        capsule.center = normalCapsuleCenter;
    }
}