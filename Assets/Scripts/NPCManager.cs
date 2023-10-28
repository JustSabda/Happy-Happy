using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.Serialization;

public class NPCManager : MonoBehaviour
{
    public static NPCManager Instance { get; private set; }

    public GameObject[] NPCs;
    public Vector3[] NPCPos;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        int childCount = transform.childCount;
        NPCs = new GameObject[childCount];
        NPCPos = new Vector3[childCount];
        for (int i = 0; i < childCount; i++)
        {
            NPCs[i] = transform.GetChild(i).gameObject;
            NPCPos[i] = transform.GetChild(i).transform.position;
        }
    }

    [Button("Spawn Object")]
    public void backToSpot()
    {
        for (int i = 0; i < NPCs.Length; i++)
        {
            NPCs[i].GetComponent<CharacterController>().enabled = false;
            NPCs[i].GetComponent<CharacterController>().transform.position = NPCPos[i];
            NPCs[i].GetComponent<CharacterController>().enabled = true;
        }
    }
}
