using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class CameraScript : NetworkBehaviour {
    public float lerpFactor = 1f;
    public float cameraAngele = 45f;

    Vector3 groupPosition = Vector3.zero;

    // Use this for initialization
    private void Start () {
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
	    if (GameManager.currentInstance.players.Count > 0)
	    {
	        //average group position
	        groupPosition = Vector3.zero;
	        foreach (var p in GameManager.currentInstance.players)
	        {
	            GameObject playerObject = ClientScene.FindLocalObject(p.netId);
	            if (!playerObject.GetComponent<Player>().isImportant)
	                groupPosition += playerObject.transform.position;
	        }
	        groupPosition.z = 0f;
	        groupPosition = groupPosition / GameManager.currentInstance.players.Count;

	        //rotate towards group
	        transform.rotation = Quaternion.Lerp(transform.rotation,
	                                            Quaternion.FromToRotation(Vector3.forward, groupPosition - transform.position),
	                                            1);
	    }
	}
}
