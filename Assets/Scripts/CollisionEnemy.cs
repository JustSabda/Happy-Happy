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
    public bool Captured;

    Capturing capturing;
   
    // Start is called before the first frame update
    void Start()
    {
        controller = this.GetComponent<CharacterController>();
        input = GetComponent<PlayerMovement>();
        capturing = this.GetComponent<Capturing>();
    }

    // Update is called once per frame
    void Update()
    {
        capturing.isNPC = true;

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

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.E) || (Input.GetKeyDown(KeyCode.F)))
        {
            capturing.Cappy = null;
            capturing.Cappy = GameObject.FindWithTag("Cap");
        }
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

        gameObject.tag = "Player";

        //Trigger Convert Animation in ManagerScript
        GameObject Manager = GameObject.Find("Manager");
        Manager.GetComponent<AnimationManager>().CaptureAnimation();    
        this.enabled = false;
    }

}
