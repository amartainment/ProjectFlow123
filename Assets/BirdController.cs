using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    //player parameters
    public float skill_accuracy = 0f; //how close to the center of the target is the player
    public float skill_dexterity = 0f; //how many obstacles has the player hit
    public float previousAccuracy;
    public float currentAccuracy;
    public float startingAccuracy = 3f;
    //flight controls
    public float gravityFactor;
    public float thrustFactor;
    public float wingFlapStrength;
    public float horizontalSensitivity;
    public float verticalSensitivity;
    private float yaw = 0f;
    private float pitch = 0f;

    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        currentAccuracy = startingAccuracy;
    }

    // Update is called once per frame
    void Update()
    {
        naturalForces();
        checkPlayerInput();
        
    }

    void checkPlayerInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            flapWings();
        }
    }

    void naturalForces()
    {
        //NEED to add provision to change flight parameters (speed) based on player skill 

        //gravity, forward thrust
      //  transform.localPosition += Vector3.down * gravityFactor * Time.deltaTime;
        transform.localPosition += transform.forward * thrustFactor * Time.deltaTime;

        //yaw and pitch from mouseInput (Later to be head tracking from VR)
        yaw += horizontalSensitivity * Input.GetAxis("Mouse X");
        pitch += -verticalSensitivity * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    void flapWings()
    {
        transform.localPosition += transform.up * wingFlapStrength * Time.deltaTime;
    }
}
