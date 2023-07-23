using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float gravity = -9.81f; // You can adjust this value to change the strength of gravity
    private CharacterController controller;

    private Vector3 velocity; // This will store the current velocity of the player

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Apply gravity. Note that gravity is accumulated over time.
        if (controller.isGrounded && velocity.y < 0)
        {
            // If the player is on the ground, reset the y velocity (this prevents accumulating downward speed while on the ground)
            velocity.y = 0f;
        }

        velocity.y += gravity * Time.deltaTime; // Apply gravity to y velocity

        // Move the controller with the velocity
        controller.Move(velocity * Time.deltaTime);
    }
}
