using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveForce : MonoBehaviour
{
    public GameObject[] Forces;
    private bool isActive;


    [SerializeField] private float timeReset;
    [SerializeField] private float remainTime;
    private void Start()
    {
        int childCount = transform.childCount;
        Forces = new GameObject[childCount];
        for (int i = 0; i < childCount; i++)
        {
            Forces[i] = transform.GetChild(i).gameObject;
        }

    }

    private void Update()
    {
        if (remainTime > 0)
        {
            remainTime -= Time.deltaTime;
        }
        else
        {
            remainTime = timeReset;

            isActive = !isActive;
        }

        for (int i = 0; i < Forces.Length; i++)
        {
            if (isActive)
                Forces[i].SetActive(true);
            else
                Forces[i].SetActive(false);
        }
    }

}
