using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using UnityEngine.VFX;
using static Unity.Burst.Intrinsics.X86;

public class AnimationManager : MonoBehaviour
{
     
    public GameObject Player;
    [Header("Capture Animation / Dissolve")]
    //public Material DissolveMat;
    //public GameObject PlayerDissolve;
    public float startValue = 1F;
    public float endValue = 2F;
    public float Duration = 1F;
    [Header("Capture Animation / Glow")]
    public float startValueGlow = 1F;
    public float endValueGlow = 2F;
    public float DurationGlow = 1F;
    //public Material GlowMat;
    [Header("Capture Animation / Variable")]
    public GameObject Enemy;
    public Capturing PlayerCapturing;
    public Capturing EnemyCapturing;
    Vector3 EnemyCappySpace;
    private Tween CurrentTween;
    private Tweener StretchTween;
   
    //public ParticleSystem CaptureParticle;
    
    //public VisualEffect Beam;

    [Header("Capture Animation / Curve")]
    public float AnimDuration;
    public float CurveAnim;
    [Header("Capture Animation / Stretch")]
    public float StretchValue = 5F;
    public float Stretchend = 2;
    [Header("Camera")]
    public CinemachineVirtualCamera Cam;
    [Header("EyeColor")]
    public GameObject EnemyEyes;
    public Material NewMaterial;
    Renderer EyeRenderer;
    int Playamount = 1;
    
   
    private void Start()
    {
        EnemyCappySpace = EnemyCapturing.CappySpace.position;
        //EyeRenderer = EnemyEyes.GetComponent<Renderer>();
      
    }
    

    public void CaptureAnimation()
    {

        //Disolve Animation
        //Instantiate(PlayerDissolve, Player.transform.position, Player.transform.rotation);
        //DOTween.To(() => startValue, x => startValue = x, endValue, Duration).OnUpdate(() => DissolveMat.SetFloat("_Duration", startValue));
        // Enemy new Camera Target
        Cam.Follow = Enemy.transform;
        //Cam.LookAt = Enemy.transform;
        Vector3 EndPosition = EnemyCapturing.CappySpace.position + new Vector3(0, 1.5F * StretchValue, 0);
        Player.transform.DOJump(EndPosition, CurveAnim, 1, AnimDuration).SetEase(Ease.Linear); 
        // Stretch Player
        Player.transform.DOScaleY(StretchValue, AnimDuration).SetEase(Ease.Linear); 
        // Rotate
        Vector3 RotateValue = new Vector3(180, 0,0);
       
        CurrentTween = Player.transform.DORotate(RotateValue, AnimDuration).OnUpdate(Convert).SetEase(Ease.Linear);
       

    }
   
    private void Convert()
    {

        StartCoroutine(ShrinkWrap());


         
    }
    private IEnumerator ShrinkWrap()
    {
        Vector3 Pos = transform.position; 
        // Tween Starts after 90 Percent of prevous Tween
        yield return new WaitForSeconds(CurrentTween.Duration()* .96F);

        Vector3 Scale = new Vector3(.02F, .1F, .02F);
        StretchTween =  Player.transform.DOScale(Scale, AnimDuration * .4F).SetEase(Ease.OutQuad);
       
        // Correct Postion Difference due to Pivot/Scale
        Player.transform.DOMoveY(EnemyCapturing.CappySpace.position.y +0.07F * StretchValue , AnimDuration* .4F).OnComplete(SetOff);
       
      

    }
    void SetOff()
    {
        Player.SetActive(false);
       
        StartCoroutine(ParticlePlay());
 
    }
 
    IEnumerator ParticlePlay()
    {
      
        yield return null;
        if (Playamount == 1)
        {
            //CaptureParticle.Play();
            //Beam.enabled = true;
            //Beam.Play();
            Playamount++;
            //Glow Effect

            //DOTween.To(() => startValueGlow, x => startValueGlow = x, endValueGlow, DurationGlow).OnUpdate(() => GlowMat.SetFloat("_GlowDuration", startValue)).OnComplete(() => GlowMat.SetFloat("_GlowDuration", -1)) ;
            //// Change Enemys EyeColor
            //EyeRenderer.material = NewMaterial;

        }
    }
   
 
}


