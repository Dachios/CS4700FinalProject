using UnityEngine;

public class MouseLook : MonoBehaviour
{

public float mouseSensitivity = 0.25f;
public CharacterController playerBody;
public Transform playerCam;

private Vector2 mouseInput;
float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {

        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        //float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        //float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseInput.y * mouseSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        playerBody.transform.Rotate(0f, mouseInput.x * mouseSensitivity, 0f);
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        
    }
}
