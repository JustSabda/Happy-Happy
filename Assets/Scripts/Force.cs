using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Force : MonoBehaviour
{
    public float moveSpeed;
    private void OnTriggerStay(Collider other)
    {

        if (other.tag == "Player")
        {
            var cc = other.GetComponent<CharacterController>();
            var offset = transform.position - other.transform.position;

            if(offset.magnitude > .1f)
            {
                offset = offset.normalized * moveSpeed;

                cc.Move(offset * Time.deltaTime);
            }
        }
    }

   
}