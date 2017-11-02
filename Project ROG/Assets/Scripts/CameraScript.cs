using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class CameraScript : NetworkBehaviour
{
    public enum CameraState
    {
        AFK,TARGET,GROUP,LENGTH
    }

    public float lerpFactor = 1f;
    public float heightOffset = 4f;
    public float verticalOffset = .1f;

    private Vector3 groupPosition = Vector3.zero;

    public Transform posTarget;
    public Transform rotTarget;
    public CameraState state;

    private GameObject lastTile;
    public float rotSmooth = 0.1f;
    public float moveSmooth = 0.5f;

    public Transform lastCameraNode;

    // Use this for initialization
    private void Start ()
    {
        posTarget = transform.transform;
        state = CameraState.GROUP;
    }
	
	// Update is called once per frame
	private void Update ()
	{
	    GameObject currentTile = GetCurrentTile();
	    if (currentTile != null && currentTile != lastTile)
	    {
	       // lastCameraNode = currentTile.GetComponent<TileScript>().cameraNode;
            lastTile = currentTile;
            Debug.Log("Group entered another tile");
            StopCoroutine("MoveCamera");
	        StartCoroutine("MoveCamera", currentTile);
	    }

	}

    private void FixedUpdate()
    {
        switch (state)
        {
            case CameraState.GROUP:
                LookAtGroup();
                break;
            default:
                LookAtTarget();
                break;
        }
    }

    private void LookAtTarget()
    {
        //rotate towards target
        Quaternion lookQuat = Quaternion.FromToRotation(Vector3.forward, rotTarget.position - transform.position);
        lookQuat = Quaternion.Euler(lookQuat.eulerAngles.x - verticalOffset, lookQuat.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookQuat, .1f);
    }

    private void LookAtGroup()
    {
        if (GameManager.currentInstance.players.Count <= 0) return;
        //average group position
        groupPosition = Vector3.zero;
        foreach (var p in GameManager.currentInstance.players)
        {
            GameObject playerObject = ClientScene.FindLocalObject(p.netId);
            if (playerObject.GetComponent<Player>().isImportant)
                groupPosition += playerObject.transform.position;
        }
        //groupPosition.z = 0f;
        groupPosition = groupPosition / GameManager.currentInstance.players.Count;

        //rotate towards group
        Quaternion lookQuat = Quaternion.FromToRotation(Vector3.forward, groupPosition - transform.position);
        Debug.DrawRay(transform.position, transform.TransformDirection(lookQuat.eulerAngles));
        lookQuat = Quaternion.Euler(lookQuat.eulerAngles.x - verticalOffset, lookQuat.eulerAngles.y, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, lookQuat, rotSmooth);
    }

    private GameObject GetCurrentTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(groupPosition, -Vector3.up, out hit))
        {
            if (hit.collider.CompareTag("Tile"))
            {
                return hit.collider.gameObject;
            }
        }
        return null;
    }

    private IEnumerator MoveCamera(GameObject target)
    {
        //yield return new WaitForSeconds(2f);
        lastCameraNode = target.GetComponent<TileScript>().cameraNode;
        float currentlerptime = 0;
        float lerptime = 55;
        Vector3 targetPos = new Vector3(lastCameraNode.position.x, lastCameraNode.localPosition.y + heightOffset, lastCameraNode.position.z);
        Debug.Log(targetPos);
        while ((targetPos - transform.position).sqrMagnitude > 0.01f)
        {
            //increment timer once per frame
            currentlerptime += Time.fixedDeltaTime;
            if (currentlerptime > lerptime)
            {
                currentlerptime = lerptime;
            }
            //lerp!
            float perc = currentlerptime / lerptime;
            transform.position = Vector3.Lerp(transform.position, targetPos, perc);
            yield return new WaitForFixedUpdate();
        }
        yield break;
    }
}
