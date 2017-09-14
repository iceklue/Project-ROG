using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TileScript : MonoBehaviour
{
    public Transform cameraNode;
	// Use this for initialization
	void Start ()
	{
	    cameraNode = gameObject.FindComponentInChildWithTag<Transform>("Camera Node");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}
