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
    
    
   
    Vector3 direction;
    public  bool Return = true;
    public bool Throw = false;

    public GameObject newClone;
     
    private void OnEnable()
    {
       
        DOTween.KillAll(true);
        Return = true;
        Cappy.transform.position = CappySpace.transform.position;
       
        
        
    }
    
    private void Update()
    {
     
        



        if (Input.GetKeyDown(KeyCode.E) && Return == false)
        {
            CapThrow();
            Instantiate(newClone, Cappy.transform.position, Cappy.transform.rotation);
        }
        // Return Animation of Cappy
        if(Return)
        {
            direction = (CappySpace.position - Cappy.transform.position).normalized;
            
            Controller.Move(direction * ReturnSpeed * Time.deltaTime);
            if(Vector3.Distance(CappySpace.position, Cappy.transform.position)< 1.25F  )
            {
                SetParent();
                //Reset Cappys Rotation
                Cappy.transform.rotation = this.transform.rotation * Quaternion.Euler(15,0,5);
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
    public void CapThrow()
    {
        Throw = true;
        //TrailEffectWhite();
        //TrailEffectYellow();
        Cappy.transform.SetParent(null);

        Vector3 Forward = transform.forward;
        Cappy.transform.DOBlendableMoveBy(Forward * ThrowDistance, ThrowTime).OnComplete(ReturnCappy); 

    }
    public void ReturnCappy()
    {

        Return = true;
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
        transform.DOKill();
    }

   
}
