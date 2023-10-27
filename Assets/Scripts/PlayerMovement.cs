using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController))]

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpHeight;
    public float swimHeight;
    public float gravityMultiplier;

    public float jumpButtonGracePeriod;

    private Animator anim;
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

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");



        if(push.magnitude > 0.2f)
        {
            characterController.Move(push * Time.deltaTime);
        }
        push = Vector3.Lerp(push, Vector3.zero, 5 * Time.deltaTime);


        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput);
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        float gravity = Physics.gravity.y * gravityMultiplier;

        ySpeed += gravity * Time.deltaTime;


        //ySpeed = glidingMultiplier;

        if (characterController.isGrounded)
        {
            lastGroundTime = Time.time;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundTime <= jumpButtonGracePeriod)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
            isGrounded = true;
            isJumping = false;
            isGliding = false;
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                //ySpeed = jumpHeight;
                ySpeed = Mathf.Sqrt(jumpHeight * -3 * gravity);
                isJumping = true;
                jumpButtonPressedTime = null;
                lastGroundTime = null;
            }
        }
        else
        {
            characterController.stepOffset = 0;
            isGrounded = false;
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isGliding =! isGliding;
            }


        }

        if (isSwiming)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //ySpeed = Mathf.Sqrt(swimHeight * 3);

                Vector3 dir = new Vector3(0,1,0);
                dir.Normalize();

                push += dir.normalized * swimHeight / mass;
            }
            else
            ySpeed = gravity * sinkMultiplier;

        }

        if (isGliding)
            ySpeed = gravity * glidingMultiplier;
        //transform.Translate(movementDirection * speed * Time.deltaTime);

        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;
        //velocity.y = glidingMultiplier;

        characterController.Move(velocity * Time.deltaTime);


        if(movementDirection != Vector3.zero)
        {
            Quaternion toRotate = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotate, rotationSpeed * Time.deltaTime);
        }
    }
}
