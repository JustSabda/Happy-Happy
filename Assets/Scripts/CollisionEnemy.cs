using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CollisionEnemy : MonoBehaviour
{


    public CharacterController controller;
    bool isGrounded;
    float verticalVel;
    Vector3 moveVector;
    PlayerMovement input;
    bool Captured;
   
    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        input = GetComponent<PlayerMovement>();    
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded)
        {
            verticalVel -= 0;
        }
        else
        {
            verticalVel -= 1;
        }
        moveVector = new Vector3(0, verticalVel * .2f * Time.deltaTime, 0);
        controller.Move(moveVector);

    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.ToString());
        if(other.gameObject.CompareTag("Cap") && Captured == false)
        {
            Capture();
        }
    }
    public void Capture()
    {
        Captured = true;
        input.enabled = true;
        GetComponent<Capturing>().enabled = true;
        GameObject Player = GameObject.FindWithTag("Player");
        Player.GetComponent<Capturing>().Capture();
        //Trigger Convert Animation in ManagerScript
        GameObject Manager = GameObject.Find("Manager");
        Manager.GetComponent<AnimationManager>().CaptureAnimation();    
        this.enabled = false;
    }

}
