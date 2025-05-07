// Code from https://github.com/JohnEvans512/QMovement/blob/main/QMovement.cs
// Modified by and annotated by Dachi

// MAJOR BUG THAT I CAN'T SEEM TO SOLVE: If you jump, let go of your directional controls, land, and then try to go anywhere besides the direction you're sliding in, you don't have the control.



using UnityEngine;
using UnityEngine.InputSystem;

public class QMovement : MonoBehaviour
{

	public Camera playerCamera;
	public float moveSpeed = 15.0f;
	public float rotationSpeed = 3.0f;

	// Constants, Some of these overwrite values in the Character Controller fields.
	private const float FRICTION = 5.0f;
	private const float DECC_SPEED = 3.5f;
	private const float ACC_SPEED = 10.0f;
	private const float ACC_SPEED_AIR = 1.5f;
	private const float JUMP_VEL = 5.8f;
	private const float JUMP_ACC = 1.42f;
	private const float GRAVITY_ADD = 17.0f;
	private const float CAMERA_OFFSET = 0.94f;
	private const float PLAYER_HEIGHT = 1.8f;
	
	// The original programmer includes a conditional to account for Mouse Smoothing.


	// Base Character Controller component
	private CharacterController controller;


	// Player-related components (movement, keeping track of surfaces / )
	private Vector3 move_input;
	private Vector3 move_direction;
	private Vector3 move_vector;
	private Vector3 vector_down;
	private Vector3 surface_normal;
	private Vector3 camera_offset;
	private Quaternion player_rotation;

	// Character Controller keeps track of onGround, hit_surface is to keep track of slopes.
	private RaycastHit hit_surface;

	// Keeps track of the change in time ingame, probably to avoid having to write Time.deltaTime all the time (that's a lot of time)
	private float frame_time = 0.0f;

	// Player rotation and camera look
	private float rotation_input;
	private float look_input;
	private float look_y = 0.0f;

	// For Camera bobbing.
	private float bobIntensity;
	private float tiltIntensity;

	// Everything related to movement.
	private float move_speed;
	private float dot; // I believe this is used to store the value of the dot product of all of the movement variables. This is how the original Source Code from Quake 1 handles it.
	private float vel_add;
	private float vel_mul;
	private float speed;
	public float speed_mul;

	// Footstep sound variables 
	[SerializeField] private AudioClip footstepSound;
	[SerializeField] private Transform footstepAudioSourceObject;
	private AudioSource footstepAudioSource;


	void Start () 
	{
		player_rotation = transform.rotation;
		controller = GetComponent<CharacterController>();
		controller.skinWidth = 0.08f;
		controller.height = PLAYER_HEIGHT;
		controller.radius = 0.35f;		
		controller.minMoveDistance = 0; // This is required for CharacterController.isGrounded to always work.
		move_vector = new Vector3(0, -0.5f, 0);
		vector_down = new Vector3(0, -1.0f, 0);
		surface_normal = new Vector3(0, 1.0f, 0);
		move_input = new Vector3(0, 0, 0);
		camera_offset = new Vector3(0, CAMERA_OFFSET, 0);
		

        Cursor.lockState = CursorLockMode.Locked;

		// Initialize footstep audio source
		footstepAudioSource = footstepAudioSourceObject.GetComponent<AudioSource>();
		footstepAudioSource.clip = footstepSound;
	}

	void Update () 
	{
		frame_time = Time.deltaTime;
		move_input.x = Input.GetAxisRaw("Horizontal");
		move_input.z = Input.GetAxisRaw("Vertical");
		rotation_input = Input.GetAxis("Mouse X") * rotationSpeed;
		look_input = Input.GetAxis("Mouse Y") * rotationSpeed * 0.9f; // Make vertical mouse look less sensitive.
		look_y -= look_input;
		look_y = Mathf.Clamp(look_y, -90.0f, 90.0f);
		player_rotation *= Quaternion.Euler(0, rotation_input, 0);
		move_direction = player_rotation * move_input;

		bobIntensity = Mathf.Clamp(((Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.z))/2), 0f, 10f); // Ripped straight outta viewbob and modified to fit for the camera.
        
        
		if (controller == null)
		{
			print("Player is Dead.");
		}
		else
		{
			if (controller.isGrounded)
			{

				//print("ON GROUND");
				if (Physics.Raycast(transform.position, vector_down, out hit_surface, 1.5f))
				{
					surface_normal = hit_surface.normal;
					move_direction = ProjectOnPlane(move_direction, surface_normal); // Stick to the ground on slopes.
				}
				move_direction.Normalize();

				dot = move_vector.x * move_direction.x + move_vector.y * move_direction.y + move_vector.z * move_direction.z;

				speed = Mathf.Sqrt(move_vector.x * move_vector.x + move_vector.z * move_vector.z);

				// Final player speed impacted by speed, friction, and Time.deltaTime
				speed_mul = speed - (speed < DECC_SPEED ? DECC_SPEED : speed) * FRICTION * frame_time;



				// Reformatted the if statements to be blocked-out for better visibility.
				if (speed_mul < 0)
				{
					speed_mul = 0;

				}

				if (speed > 0)
				{
					speed_mul /= speed;
				}

				move_vector *= speed_mul;
				vel_add = move_speed - dot;
				vel_mul = ACC_SPEED * frame_time * move_speed;

				if (vel_mul > vel_add)
				{
					vel_mul = vel_add;
				}

				move_vector += move_direction * vel_mul;
				//if (move_vector.y > -0.5f) move_vector.y = -0.5f; // Make sure there is always a little gravity to keep the character on the ground.
				if (Input.GetButtonDown("Jump"))
				{

					if (surface_normal.y > 0.5f) // Do not jump on high slopes.
					{
						move_vector *= JUMP_ACC;
						move_vector.y = JUMP_VEL;
					}
				}
				playerCamera.transform.localPosition = new Vector3(controller.transform.position.x, (controller.transform.position.y + CAMERA_OFFSET) + (Mathf.Sin(Time.time * 10) * bobIntensity / 50), controller.transform.position.z);
		
				// Play footstep sounds when moving & grounded
				if (speed_mul > 0) {
					if (!footstepAudioSource.isPlaying)
					{
						footstepAudioSource.loop = true;
						footstepAudioSource.Play();
					}
				}
				else // Not moving
				{
					if (footstepAudioSource.isPlaying)
					{
						footstepAudioSource.loop = false;
						footstepAudioSource.Stop();
					}
				}
			}
			else // In Air
			{
				//print("IN AIR");

				// Stop footstep sounds when in air
				if (footstepAudioSource.isPlaying)
				{
					footstepAudioSource.loop = false;
					footstepAudioSource.Stop();
				}

				move_direction.Normalize();
				move_speed = move_direction.magnitude * moveSpeed;
				dot = move_vector.x * move_direction.x + move_vector.y * move_direction.y + move_vector.z * move_direction.z;
				vel_add = move_speed - dot;
				vel_mul = ACC_SPEED_AIR * frame_time * move_speed;

				// Reformatted the if statements to be blocked-out for better visibility.
				if (vel_mul > vel_add)
				{
					vel_mul = vel_add;
				}
				if (vel_mul > 0)
				{
					move_vector += move_direction * vel_mul;
				}
				move_vector.y -= GRAVITY_ADD * frame_time;
				playerCamera.transform.localPosition = new Vector3(controller.transform.position.x, (controller.transform.position.y + CAMERA_OFFSET), controller.transform.position.z);
			}


			controller.Move(move_vector * frame_time);


			tiltIntensity = Input.GetAxis("Horizontal") * 2.5f; // This method is very "snappy" but it gets the job done.

			playerCamera.transform.rotation = player_rotation * Quaternion.Euler(look_y, 0, -tiltIntensity);
		}

	}
    

	Vector3 ProjectOnPlane (Vector3 vector, Vector3 normal)
	{
		return vector - normal * (vector.x * normal.x + vector.y * normal.y + vector.z * normal.z);
	}
}