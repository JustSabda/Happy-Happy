using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Capturing : MonoBehaviour
{
    public GameObject Cappy;
    public Transform CappySpace;
    public float ThrowTime = 1F;
    public float ReturnSpeed = 1F;
    public float ThrowDistance =3F;
    public float RotationSpeed=2F;
    public CharacterController Controller;

    [Header("CircleTrail Effect/ White")]
    public TrailRenderer TrailWhite;
    public Vector3 Radial;
    [Header("CircleTrail Effect/ Yellow")]
    public TrailRenderer TrailYellow;
    public float Height = 1F;
    [Header("Cappy Floating Animation")]
    public float speed = 2F;
    public float height = 1F;

    [HideInInspector] public bool isNPC = false;
    
   
    Vector3 direction;
    public  bool Return = true;
    public bool Throw = false;

    public GameObject newClone;

    private CharacterController characterController;
     
    private void OnEnable()
    {
       
        DOTween.KillAll(true);
        Return = true;
        Cappy.transform.position = CappySpace.transform.position;
        
        
    }

    private void Start()
    {
        //GameManager.Instance.Cam.Follow = gameObject.transform;
        Cappy = GameObject.Find("Cap");
        Controller = Cappy.GetComponent<CharacterController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E) && Return == false)
        {
            CapThrow();
            
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Instantiate(newClone, Cappy.transform.position + new Vector3(1, 0, 0), transform.rotation);

            OutBody();
        }
        // Return Animation of Cappy
        if (Return)
        {

            direction = (CappySpace.position - Cappy.transform.position).normalized;
            
            Controller.Move(direction * ReturnSpeed * Time.deltaTime);
            if(Vector3.Distance(CappySpace.position, Cappy.transform.position)< 1.25F  )
            {
                SetParent();
                //Reset Cappys Rotation
                Cappy.transform.rotation = this.transform.rotation * Quaternion.Euler(0,0,0);
            }
        }
        //Spin Rotation
        if(Vector3.Distance(CappySpace.position, Cappy.transform.position) > 1F)
        {
           
            Vector3 Spin = new Vector3 (0, 1, 0);    
            Cappy.transform.Rotate(Spin * (RotationSpeed*100) * Time.deltaTime);
        }

        //Cappy Floating Animation
        if (Vector3.Distance(CappySpace.position, Cappy.transform.position) < 1F && Throw == false)
        {
            float Y = Mathf.Sin(Time.time * speed) * (height/1000) + Cappy.transform.position.y;
            Cappy.transform.position = new Vector3(Cappy.transform.position.x,Y, Cappy.transform.position.z);
            
        }
        




    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "FallDetector" && GameManager.Instance.isLose == false)
        {
            Instantiate(newClone, GameManager.Instance.respawnPoint, transform.rotation);

            OutBody();

            NPCManager.Instance.backToSpot();
        }
    }
    public void OutBody()
    {
        

        if (!isNPC)
        {

            gameObject.SetActive(false);
            transform.DOKill();

        }
        else
        {

            Cappy.SetActive(false);
            var player = gameObject.GetComponent<PlayerMovement>();


            player.anim1.SetBool("Walk", false);
            player.anim2.SetBool("Walk", false);
            player.anim1.SetBool("Run", false);
            player.anim2.SetBool("Run", false);

            if (player.fruitType == Fruit.Carrot)
            {
                player.anim1.SetBool("Fly", false);
                player.anim2.SetBool("Fly", false);
            }

            if(player.fruitType == Fruit.Coconut)
            {
                player.anim1.SetBool("Float", false);
                player.anim2.SetBool("Float", false);
            }

            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<CollisionEnemy>().enabled = true;
            GetComponent<CollisionEnemy>().Captured = false;

            Return = true;
            this.enabled = false;
            gameObject.tag = "Untagged";
            transform.DOKill();

        }
    }
    public void CapThrow()
    {
        var player = gameObject.GetComponent<PlayerMovement>();

        player.anim1.SetTrigger("ThrowCap");
        player.anim2.SetTrigger("ThrowCap");
        Throw = true;
        //TrailEffectWhite();
        //TrailEffectYellow();
        Cappy.transform.SetParent(null);
        Cappy.tag = "Cap";
        Vector3 Forward = transform.forward;
        Cappy.transform.DOBlendableMoveBy(Forward * ThrowDistance, ThrowTime).OnComplete(ReturnCappy); 

    }
    public void ReturnCappy()
    {

        Return = true;
        Cappy.tag = "Untagged";
    }
    public void SetParent()
    {
        Throw = false;
        Return = false;
        //TrailWhite.enabled = false;
       
        //TrailYellow.enabled = false;
        Cappy.transform.position = CappySpace.position;
        Cappy.transform.SetParent(this.transform);
       
        
    }
    public void TrailEffectWhite()
    {
        TrailWhite.enabled = true;
        TrailWhite.transform.RotateAround(Cappy.transform.position,Radial,5);

        TrailWhite.transform.position = Vector3.Slerp(TrailWhite.transform.position,Cappy.transform.position,.1F * Time.deltaTime);

    }
    public void TrailEffectYellow()
    {
     
        TrailYellow.enabled = true;
        TrailYellow.transform.RotateAround(Cappy.transform.position, Radial, 10);

        TrailYellow.transform.position = Vector3.Slerp(TrailWhite.transform.position, Cappy.transform.position, .15F * Time.deltaTime);

        TrailYellow.transform.Translate(0,Height,0);
    }
     
    public void Capture()
    {
       
        GetComponent<PlayerMovement>().enabled = false;
        this.enabled = false;

        if (!isNPC)
        {
            gameObject.SetActive(false);

        }
        else
        {
            GetComponent<CollisionEnemy>().enabled = true;
            GetComponent<CollisionEnemy>().Captured = false;
            gameObject.tag = "Untagged";
        }
        

        transform.DOKill();
    }

   
}
