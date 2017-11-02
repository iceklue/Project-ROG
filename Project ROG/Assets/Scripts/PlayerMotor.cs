using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Player player;

    private Rigidbody playerRb;
    private Camera cam;
    private CameraScript camScript;

    public Transform headTest;


    // Use this for initialization
    void Start () {
        player = GetComponent<Player>();
        playerRb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
	    camScript = Camera.main.GetComponent<CameraScript>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Jump()
    {
        playerRb.AddForce(player.movement.jumpForce * transform.up, ForceMode.VelocityChange);
        playerRb.AddForce(player.movement.jumpForce * playerRb.velocity.normalized, ForceMode.VelocityChange);
    }


    public Vector3 CalculateMovement(Vector2 input)
    {
        // Calculate how fast we should be moving
        Vector3 targetVelocity = new Vector3(input.x, 0, input.y).normalized;
        targetVelocity = transform.TransformDirection(targetVelocity);//camScript.lastCameraNode.TransformDirection(targetVelocity);
        Debug.Log(Vector3.Dot(targetVelocity, headTest.forward));
        if (Vector3.Dot(targetVelocity, headTest.forward) > 0.3f)
        {
        Debug.Log("forward");
        targetVelocity *= player.movement.moveSpeedForwards;
        }
         else if (Vector3.Dot(targetVelocity, headTest.forward) < -0.3f)
        {
        Debug.Log("backwards");
        targetVelocity *= player.movement.moveSpeedBackwards;
        }
         else
        {
            targetVelocity *= player.movement.strafeSpeed;
        }

        if(player.movement.isRunning)
        {
            return targetVelocity * player.movement.moveSpeedMultiplier * 1.4f;
        }
         else
        {
            return targetVelocity * player.movement.moveSpeedMultiplier;
        }
        



        /* old way
         if (input.y > 0)
         {
             return (transform.forward * input.y * player.movement.moveSpeedForwards
                     + transform.right * input.x * player.movement.strafeSpeed)
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         else if(input.y < 0)
         {
             return (transform.forward * input.y * player.movement.moveSpeedBackwards
                     + transform.right * input.x * player.movement.strafeSpeed)
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         else
         {
             return  transform.right * input.x * player.movement.strafeSpeed
                     * player.movement.moveSpeedMultiplier * Time.fixedDeltaTime;
         }
         */

    }


    public void DoMovement(Vector3 _targetVelocity)
    {
        // Apply a force that attempts to reach our target velocity
        Vector3 velocity = playerRb.velocity;
        Vector3 velocityChange = (_targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -player.movement.maxVelocityChange, player.movement.maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -player.movement.maxVelocityChange, player.movement.maxVelocityChange);
        velocityChange.y = 0;

        playerRb.AddForce(velocityChange, ForceMode.VelocityChange);

    }

    public void DoRotation(Vector3 target)
    { 
        Quaternion look =  Quaternion.FromToRotation(Vector3.forward, (target - headTest.position));
        transform.rotation = Quaternion.Euler(look.eulerAngles.x, look.eulerAngles.y,0);
    }
}
