using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    public enum Fruit {Box,Strowberry,Carrot,Coconut }

    public Fruit fruitType = Fruit.Box;

    public float speed;
    public float runSpeed;
    public float rotationSpeed;
    public float jumpHeight;
    public float swimHeight;
    public float gravityMultiplier;

    public float jumpButtonGracePeriod;

    [HideInInspector]public Animator anim1;
    [HideInInspector]public Animator anim2;
    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundTime;
    private float? jumpButtonPressedTime;
    private bool isJumping;
    private bool isGrounded;
    private bool isGliding;
    public bool isSwiming;

    public float glidingMultiplier;
    public float sinkMultiplier;

    float mass = 3.0f;
    Vector3 push = Vector3.zero;


    Vector3 startPos;



    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
        anim1 = transform.GetChild(0).GetComponent<Animator>();
        anim2 = transform.GetChild(1).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if((horizontalInput != 0 || verticalInput != 0 ))
        {

        }
        else
        {

        }


        if(push.magnitude > 0.2f)
        {
            characterController.Move(push * Time.deltaTime);
        }
        push = Vector3.Lerp(push, Vector3.zero, 5 * Time.deltaTime);


        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            magnitude *= runSpeed;
            if (isGrounded)
            {
                anim1.SetBool("Run", true);
                anim2.SetBool("Run", true);
            }
        }
        else
        {
            anim1.SetBool("Run", false);
            anim2.SetBool("Run", false);
        }

        movementDirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;

        ySpeed += gravity * Time.deltaTime;


        //ySpeed = glidingMultiplier;

        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }

        if (Input.GetButton("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            //ySpeed = -0.5f;
            isGrounded = true;
            isJumping = false;
            isGliding = false;
            if (fruitType == Fruit.Carrot)
            {
                anim1.SetBool("Fly", false);
                anim2.SetBool("Fly", false);
            }
            anim1.SetBool("Jump", false);
            anim2.SetBool("Jump", false);

            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                //ySpeed = jumpHeight;
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                isJumping = true;

                anim1.SetBool("Jump", true);
                anim2.SetBool("Jump", true);

                jumpButtonPressedTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            isGrounded = false;
            if (Input.GetKeyDown(KeyCode.Q) && fruitType == Fruit.Carrot)
            {
                isGliding =! isGliding;
                if (isGliding)
                {
                    anim1.SetBool("Fly", true);
                    anim2.SetBool("Fly", true);
                }
                else
                {
                    anim1.SetBool("Fly", false);
                    anim2.SetBool("Fly", false);
                }
            }


        }

        if (isSwiming )
        {
            if (fruitType == Fruit.Coconut)
            {
                anim1.SetBool("Float", true);
                anim2.SetBool("Float", true);

                if (Input.GetButtonDown("Jump"))
                {
                    //ySpeed = Mathf.Sqrt(swimHeight * 3);

                    Vector3 dir = new Vector3(0, 1, 0);
                    dir.Normalize();

                    push += dir.normalized * swimHeight / mass;
                }
                else
                    ySpeed = gravity * sinkMultiplier;
            }
        }
        else
        {
            if (fruitType == Fruit.Coconut)
            {
                anim1.SetBool("Float", false);
                anim2.SetBool("Float", false);
            }
        }

        if (isGliding)
            ySpeed = gravity * glidingMultiplier;
        //transform.Translate(movementDirection * speed * Time.deltaTime);

        Vector3 velocity = movementDirection * magnitude;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;
        //velocity.y = glidingMultiplier;

        characterController.Move(velocity * Time.deltaTime);


        if(movementDirection != Vector3.zero)
        {
            anim1.SetBool("Walk", true);
            anim2.SetBool("Walk", true);
            Quaternion toRotate = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }
        else
        {
            anim1.SetBool("Walk", false);
            anim2.SetBool("Walk", false);
        }

    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if(Physics.Raycast(ray,out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "FallDetector")
        {
            if(GameManager.Instance.isLose == false)
            UIManager.Instance.healthPoint--;

            //Debug.Log("wow");
            //characterController.enabled = false;
            //characterController.transform.position = GameManager.Instance.respawnPoint;
            //characterController.enabled = true;
        }
        

        if(other.tag == "Checkpoint")
        GameManager.Instance.respawnPoint = other.transform.position;
    }
}
