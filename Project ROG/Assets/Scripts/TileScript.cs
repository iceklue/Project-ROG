using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Networking;

public class TileScript : NetworkBehaviour
{
    public Transform cameraNode;
	// Use this for initialization
	void Start()
	{
        GetComponent<Renderer>().material.color = new Color(Random.Range(0f,.4f), 0.45f, 0.05f);
	    cameraNode = gameObject.FindComponentInChildWithTag<Transform>("Camera Node");
        Debug.Log(cameraNode);
	    cameraNode.transform.localRotation = Quaternion.Euler(0, Quaternion.FromToRotation(cameraNode.forward, (transform.position- cameraNode.position)).eulerAngles.y,0);
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
