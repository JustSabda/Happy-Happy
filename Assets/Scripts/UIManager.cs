using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [HideInInspector]
    public bool isPaused;

    public int healthPoint;

    [SerializeField]
    private GameObject healthImage;
    [SerializeField]
    private GameObject healthLossImage;

    [SerializeField]
    private GameObject panelWin;

    public GameObject panelLose;

    public GameObject pausePanel;

    private GameObject settingPanel;

    // Start is called before the first frame update
    [Header("MusicBtn")]
    public GameObject musicBtnOn;
    public GameObject musicBtnOff;

    [Header("SFXBtn")]
    public GameObject SFXBtnOn;
    public GameObject SFXBtnOff;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
    private void Start()
    {
        if(panelWin != null)
        {
            panelWin.SetActive(false);
        }
        if (panelLose != null)
        {
            panelLose.SetActive(false);
        }
        
    }



    // Update is called once per frame
    private void Update()
    {



        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }

        //if (!AudioManager.Instance.musicSource.mute)
        //{
        //    musicBtnOn.SetActive(true);
        //    musicBtnOff.SetActive(false);
        //}
        //else
        //{
        //    musicBtnOn.SetActive(false);
        //    musicBtnOff.SetActive(true);
        //}

        //if (!AudioManager.Instance.sfxSource.mute)
        //{
        //    SFXBtnOn.SetActive(true);
        //    SFXBtnOff.SetActive(false);
        //}
        //else
        //{
        //    SFXBtnOn.SetActive(false);
        //    SFXBtnOff.SetActive(true);
        //}


        HealthPoint();

        if (GameManager.Instance.isWin == true && panelWin != null)
        {
            panelWin.SetActive(true);
        }

        if (GameManager.Instance.isLose == true && panelLose != null)
        {
            panelLose.SetActive(true);
        }

    }

    public void HealthPoint()
    {
        if (healthImage != null)
        {
            switch (healthPoint)
            {
                case 0:
                    healthImage.transform.GetChild(0).gameObject.SetActive(false);
                    healthImage.transform.GetChild(1).gameObject.SetActive(false);
                    healthImage.transform.GetChild(2).gameObject.SetActive(false);

                    healthLossImage.transform.GetChild(0).gameObject.SetActive(true);
                    healthLossImage.transform.GetChild(1).gameObject.SetActive(true);
                    healthLossImage.transform.GetChild(2).gameObject.SetActive(true);

                    //Lose
                    panelLose.SetActive(true);
                    GameManager.Instance.isLose = true;
                    break;
                case 1:
                    healthImage.transform.GetChild(0).gameObject.SetActive(true);
                    healthImage.transform.GetChild(1).gameObject.SetActive(false);
                    healthImage.transform.GetChild(2).gameObject.SetActive(false);

                    healthLossImage.transform.GetChild(0).gameObject.SetActive(false);
                    healthLossImage.transform.GetChild(1).gameObject.SetActive(true);
                    healthLossImage.transform.GetChild(2).gameObject.SetActive(true);
                    break;
                case 2:
                    healthImage.transform.GetChild(0).gameObject.SetActive(true);
                    healthImage.transform.GetChild(1).gameObject.SetActive(true);
                    healthImage.transform.GetChild(2).gameObject.SetActive(false);

                    healthLossImage.transform.GetChild(0).gameObject.SetActive(false);
                    healthLossImage.transform.GetChild(1).gameObject.SetActive(false);
                    healthLossImage.transform.GetChild(2).gameObject.SetActive(true);
                    break;
                case 3:
                    healthImage.transform.GetChild(0).gameObject.SetActive(true);
                    healthImage.transform.GetChild(1).gameObject.SetActive(true);
                    healthImage.transform.GetChild(2).gameObject.SetActive(true);

                    healthLossImage.transform.GetChild(0).gameObject.SetActive(false);
                    healthLossImage.transform.GetChild(1).gameObject.SetActive(false);
                    healthLossImage.transform.GetChild(2).gameObject.SetActive(false);
                    break;
            }
        }
    }

    public void musicBtn()
    {
       
        AudioManager.Instance.ToogleMusic();

    }

    public void SFXBtn()
    {
       
        AudioManager.Instance.ToogleSFX();

    }

    public void SettingPanel()
    {
        pausePanel.SetActive(false);
        settingPanel.SetActive(true);
    }



    public void Pause()
    {
        isPaused = true;
        pausePanel.SetActive(true);
        settingPanel.SetActive(false);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        isPaused = false;
        pausePanel.SetActive(false);
        settingPanel.SetActive(false);
        Time.timeScale = 1f;

    }


}
