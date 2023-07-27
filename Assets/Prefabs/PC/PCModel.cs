using UnityEngine;

public class PCModel : MonoBehaviour
{
    public Transform cameraTransform;
    public float movementSpeed = 5f;
    public float gravity = -9.81f;

    private CharacterController characterController;
    private Vector3 velocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        MoveCharacter();
        RotateCharacter();
        ApplyGravity();
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);

        // Ignore the y-component of the camera's forward direction
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        moveDirection = Quaternion.LookRotation(cameraForward) * moveDirection;

        // Combine movement and gravity into a single call to controller.Move()
        characterController.Move((moveDirection * movementSpeed + velocity) * Time.deltaTime);
    }



    private void RotateCharacter()
    {
        Quaternion cameraRotation = cameraTransform.rotation;
        Quaternion characterRotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        transform.rotation = characterRotation;
    }

    private void ApplyGravity()
    {
        // Apply gravity. Note that gravity is accumulated over time.
        if (characterController.isGrounded && velocity.y < 0)
        {
            // If the player is on the ground, reset the y velocity (this prevents accumulating downward speed while on the ground)
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to y velocity
    }
}
