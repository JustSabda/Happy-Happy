using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
       var player = other.GetComponent<PlayerMovement>();
        if (player != null)
            player.isSwiming = true;
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PlayerMovement>();
        if (player != null)
            player.isSwiming = false;
    }
}
