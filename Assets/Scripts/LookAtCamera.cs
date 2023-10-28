using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Camera cameraOnly;
    private void Start()
    {
        cameraOnly = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(cameraOnly.transform);
    }
}
