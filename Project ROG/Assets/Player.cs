using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class Player : NetworkBehaviour
{
    public struct PlayerInfo
    {
        public string name;
        public float timeAdded;
        public NetworkInstanceId netId;
        public override string ToString()
        {
            return name + " (" + netId + ")";
        }
    };

    public static int playerCount = 0;

    [System.Serializable]
    public class Movement
    {
        public float moveSpeedForwards = 10.0f;
        public float moveSpeedBackwards = 5.0f;
        public float strafeSpeed = 3.0f;
        public float moveSpeedMultiplier = 1.0f;
        public float maxVelocityChange = 1.0f;

        public float jumpForce = 1.0f;

        public bool isRunning = false;
    }


    [SyncVar] public float health = 1;
    public float healthMax = 50;
    public float healthRegenPerSecond = 2;

    [SyncVar] public bool isImportant = true;
    [SyncVar] public bool isDead = false;
    [SyncVar] public bool isInvincible = false;

    public Behaviour[] disableWhenDead;
    public GameObject[] disableGameObjectsWhenDead;
    public bool[] wasEnabled;

    public PlayerInfo info;
    public Movement movement;

    private float distToGround;

    

    public override void OnStartLocalPlayer()
    {
        GetComponentInChildren<MeshRenderer>().material.color = Color.blue;
        distToGround = GetComponent<Collider>().bounds.extents.y;
        info.netId = GetComponent<NetworkIdentity>().netId;
    }

    public override void OnStartServer()
    {
        info.netId = GetComponent<NetworkIdentity>().netId;
        GameManager.currentInstance.CmdAddThisPlayerToList(info.netId);

    }

    // Update is called once per frame
    void Update()
    {

    }



    public void Die() //Setup for death
    {

    }

    public bool isGrounded() //Check if player is grounded
    {
        return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
    }


    [ClientRpc]
    public void RpcAddHealth(int amount) //Add health to health pool
    {
        if (health + amount > healthMax)
        {
            health = healthMax;
        }
        else
        {
            health += amount;
        }
    }

    [ClientRpc]
    public void RpcRemoveHealth(int amount) //Remove health from health pool
    {
        if (!isDead && !isInvincible)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= amount;
        }
    }
}

