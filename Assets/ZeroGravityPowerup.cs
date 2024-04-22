using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeroGravityPowerup : MonoBehaviour
{
    public float powerupTime;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.GetComponent<Player>().ZeroGravity(powerupTime);
        }
    }
}
