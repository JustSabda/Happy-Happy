using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject catOn_1;
    public GameObject catOff_1;
    public GameObject catOn_2;
    public GameObject catOff_2;

    private void Start()
    {
        catOn_1.SetActive(false);
        catOff_1.SetActive(true);
        catOn_2.SetActive(false);
        catOff_2.SetActive(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            catOn_1.SetActive(true);
            catOff_1.SetActive(false);
            catOn_2.SetActive(true);
            catOff_2.SetActive(false);
        }
    }

}
