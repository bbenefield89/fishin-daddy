using UnityEngine;

public class PCMovement : MonoBehaviour
{
    public Transform cameraTransform;
    public float movementSpeed = 5f;

    private CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCharacter();
        RotateCharacter();
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection = cameraTransform.TransformDirection(moveDirection);

        characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
    }

    private void RotateCharacter()
    {
        Quaternion cameraRotation = cameraTransform.rotation;
        Quaternion characterRotation = Quaternion.Euler(0, cameraRotation.eulerAngles.y, 0);
        transform.rotation = characterRotation;
    }
}
