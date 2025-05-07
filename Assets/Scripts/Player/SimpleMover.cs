using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleMover : MonoBehaviour
{
    public float speed = 6f;
    public float gravity = 9.81f;

    CharacterController controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // read input in plane space
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 move = transform.right * h + transform.forward * v;

        controller.Move(move * speed * Time.deltaTime);

        // apply gravity so the player stays grounded
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;   // small push keeps grounded state

        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
