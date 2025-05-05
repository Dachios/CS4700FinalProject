// Code by Dachi
// Originally intended as a supplemental to an existing camera script. Now the Quake movement handles the camera, so this is just for weapon bobbing.

using UnityEngine;


public class ViewBob : MonoBehaviour
{
    public GameObject weapon;
    public Camera playerCam;
    public CharacterController playerBody;

    private Vector3 playerSpeed;

    

    float bobIntensity;
    float sideOffset;
    float vertOffset;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Constantly gets the velocity of the rigid body
        //playerSpeed = playerBody.GetComponent<> 
        if (playerBody == null)
        {
            //print("Player is Dead.");
        }
        else
        {
            playerSpeed = playerBody.GetComponent<CharacterController>().velocity;

            // Uses the average of the X and Z speed to calculate the forward and back bobbing of the weapon model.

            bobIntensity = Mathf.Clamp(((Mathf.Abs(playerSpeed.x) + Mathf.Abs(playerSpeed.z)) / 2), 0f, 10f);


            // Notes: Figure out how to stop it from "snapping" into the desired location.
            //        Clamp the bob and offset intensity so the gun doesn't fly off at high speeds.
            sideOffset = Mathf.PerlinNoise1D(Time.time) * Input.GetAxis("Horizontal") / 10;
            vertOffset = Mathf.PerlinNoise1D(Time.time) * Input.GetAxis("Vertical") / 25;
            weapon.transform.localPosition = new Vector3(sideOffset, vertOffset * -1, Mathf.Sin(Time.time * 10) * bobIntensity / 50);
        }
        




    }
}
