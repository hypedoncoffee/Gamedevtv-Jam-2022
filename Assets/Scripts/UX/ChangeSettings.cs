using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
//using UnityEngine.Rendering.PostProcessing;

public class ChangeSettings : MonoBehaviour
{
    //list of settings change options from my last games.
    // Start is called before the first frame update




    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetDifficulty(Slider diffi)
    {
        FindObjectOfType<Difficulty>().NumberOfVips = (int)diffi.value; 
    }

    
    [SerializeField] private TextMeshProUGUI qualityLabel;
    public void ChangeGraphicsPreset(Slider presetDropdown)
    {
        int nextPreset;
        nextPreset = Mathf.RoundToInt(presetDropdown.value);
        //int.TryParse(nextPreset, out preset);
        QualitySettings.SetQualityLevel(nextPreset, true);
        qualityLabel.text = QualitySettings.names[nextPreset];
    }
    [SerializeField] private TextMeshProUGUI windowModeLabel;
    public void ChangeWindowMode(Slider windowDropdown)
    {
        int nextWindow = Mathf.RoundToInt(windowDropdown.value);
        //      int.TryParse(nextWindow, out window);
        switch (nextWindow)
        {
            case 1:
            windowModeLabel.text = "Fullscreen Window";
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 0:
            windowModeLabel.text = "Windowed";
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }

    [SerializeField] private TextMeshProUGUI renderResLabel;
    public void ChangeRenderRes(Slider renderResSlider)
    {
        int renderResToggle = Mathf.RoundToInt(renderResSlider.value);
        switch (renderResToggle)
        {
            //0.25x
            case 0:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.25f;
                renderResLabel.text = "0.25x Resolution";
                break;
            //.5x;
            case 1:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
                renderResLabel.text = "0.50x Resolution";
                break;
            //.75x
            case 2:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.75f;
                renderResLabel.text = "0.75x Resolution";
                break;
            //1x
            case 3:
                QualitySettings.resolutionScalingFixedDPIFactor = 1f;
                renderResLabel.text = "1.00x Resolution";
                break;
            //1.25x
            case 4:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.25f;
                renderResLabel.text = "1.25x Resolution";
                break;
            //1.5x
            case 5:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.5f;
                renderResLabel.text = "1.50x Resolution";
                break;
            //1.75x
            case 6:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.75f;
                renderResLabel.text = "1.75x Resolution";
                break;
            //2x
            case 7:
                QualitySettings.resolutionScalingFixedDPIFactor = 2f;
                renderResLabel.text = "2.00x Resolution";
                break;
        }
    }



    [SerializeField] private TextMeshProUGUI masterVolumeLabel;
    [SerializeField] private TextMeshProUGUI sfxVolumeLabel;
    [SerializeField] private TextMeshProUGUI bgmVolumeLabel;
    [SerializeField] private TextMeshProUGUI dialogueVolumeLabel;
    [SerializeField] private AudioMixer soundMixing;
    public void SetMasterVolume(Slider nextMasterVolume)
    {
        soundMixing.SetFloat("masterVolume", nextMasterVolume.value);
        masterVolumeLabel.text = Mathf.RoundToInt((nextMasterVolume.value+50) /.5f) + " %";
    }
    public void SetSFXVolume(Slider nextSFXVolume)
    {
        soundMixing.SetFloat("sfxVolume", nextSFXVolume.value);
        sfxVolumeLabel.text = Mathf.RoundToInt((nextSFXVolume.value+50) /.5f) + " %";
    }
    public void SetBGMVolume(Slider nextBGMVolume)
    {
        soundMixing.SetFloat("bgmVolume", nextBGMVolume.value);
        bgmVolumeLabel.text = Mathf.RoundToInt((nextBGMVolume.value+50) /.5f) + " %";
    }
    public void SetDialogueVolume(Slider nextDialogueVolume)
    {
        soundMixing.SetFloat("dialogueVolume", nextDialogueVolume.value);
        dialogueVolumeLabel.text = Mathf.RoundToInt(nextDialogueVolume.value * 100f) + " %";
    }
    [SerializeField] TextMeshProUGUI paLabelVolume;
    public void SetPAVolume(Slider nextPAVolume)
    {
        soundMixing.SetFloat("paVolume", nextPAVolume.value);
        paLabelVolume.text = Mathf.RoundToInt(nextPAVolume.value * 100f) + " %";
    }
    public void SetPatrolVoice(Slider nextPatrolVoices)
    {
        //number of voices allowed in voice pool, chatter preferences
    }
    [SerializeField] TextMeshProUGUI patrolLabelVolume;
    public void SetPatrolVolume(Slider nextPatrolVolume)
    {
        soundMixing.SetFloat("patrolVolume", nextPatrolVolume.value);
        patrolLabelVolume.text = Mathf.RoundToInt(nextPatrolVolume.value * 100f) + " %";
        
    }


    /*

        public void ChangeGraphicsPreset(TMP_Dropdown presetDropdown)
        {
            int nextPreset;
            nextPreset = presetDropdown.value;
            //int.TryParse(nextPreset, out preset);
            QualitySettings.SetQualityLevel(nextPreset, true);
        }

    public void ChangeWindowMode(TMP_Dropdown windowDropdown)
    {
        int nextWindow = windowDropdown.value;
        //      int.TryParse(nextWindow, out window);
        switch (nextWindow)
        {
            case 0:
                Screen.fullScreen = Screen.fullScreen;
                break;
            case 1:
                Screen.fullScreen = !Screen.fullScreen;
                //        FullScreenMode = FullScreenMode.FullScreenWindow;
                break;
        }
    }

    public void ChangeAAMode(TMP_Dropdown aaDropdown)
    {
        PostProcessLayer ppl;
        int aa = aaDropdown.value;
        //nextAA = aaDropdown.itemText.text;
        //int.TryParse(nextAA, out aa);
        ppl = GameObject.FindWithTag("MainCamera").GetComponent<PostProcessLayer>();
        PostProcessLayer.Antialiasing SubpixelMorphologicalAntialiasing;
        switch (aa)
        {
            case 0:
                PostProcessLayer.Antialiasing None;
                //nothing
                break;
            case 1:
                PostProcessLayer.Antialiasing FastApproximateAntialiasing;
                //fxaa
                break;
            case 2:
                //smaa
                SubpixelMorphologicalAntialiasing.Quality Medium;
                break;
            case 3:
                //PostProcessLayer.Antialiasing SubpixelMorphologicalAntialiasing;
                SubpixelMorphologicalAntialiasing.Quality High;
                //smaa but swole
                break;
            //taa
            case 4:
                PostProcessLayer.Antialiasing TemporalAntialiasing;
                break;
        }
    }
    //FOV code lifted from Resurface, thankfully only one camera needs to be
    //changed as opposed to whatever the heck that game
    //had going on with its cameras
    //...i miss working on that game
    [SerializeField] private TextMeshProUGUI fovLabel;
    public void AdjustFOV(Slider fovSlider)
    {
        float newExplorationFOV = fovSlider.value;
        //newInteractionFOV = Mathf.RoundToInt(fovSlider.value * 0.8f);
        FindObjectOfType<MouseLook>().gameObject.GetComponent<Camera>().fieldOfView = newExplorationFOV;

        /*      if(this.gameObject.name=="Interact Camera")
              {
                this.gameObject.GetComponent<Camera>().fieldOfView = newInteractionFOV;
              }
          */

          /*
        fovLabel.text = "" + fovSlider.value + "degrees Vert";
        //        fovInteractionLabel.text = "" + newInteractionFOV +"";
        Debug.Log("FOV changed to " + newExplorationFOV + "for Exploration Mode");
    }
    //Controls settings
/*
    [SerializeField] TextMeshProUGUI mouseSpeedLabel;
    public void AdjustMouseSpeed(Slider mouseSlider)
    {
        MouseLook camera = FindObjectOfType<MouseLook>();
        camera.SetMouseSpeed(mouseSlider.value);
        mouseSpeedLabel.text = "" + mouseSlider.value;
        Debug.Log("Mouse speed changed to " + mouseSlider.value);
    }

    [SerializeField] private TextMeshProUGUI renderResLabel;
    public void ChangeRenderRes(Slider renderResSlider)
    {
        int renderResToggle = Mathf.RoundToInt(renderResSlider.value);
        switch (renderResToggle)
        {
            //0.25x
            case 0:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.25f;
                renderResLabel.text = "0.25x Resolution";
                break;
            //.5x;
            case 1:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
                renderResLabel.text = "0.50x Resolution";
                break;
            //.75x
            case 2:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.75f;
                renderResLabel.text = "0.75x Resolution";
                break;
            //1x
            case 3:
                QualitySettings.resolutionScalingFixedDPIFactor = 1f;
                renderResLabel.text = "1.00x Resolution";
                break;
            //1.25x
            case 4:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.25f;
                renderResLabel.text = "1.25x Resolution";
                break;
            //1.5x
            case 5:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.5f;
                renderResLabel.text = "1.50x Resolution";
                break;
            //1.75x
            case 6:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.75f;
                renderResLabel.text = "1.75x Resolution";
                break;
            //2x
            case 7:
                QualitySettings.resolutionScalingFixedDPIFactor = 2f;
                renderResLabel.text = "2.00x Resolution";
                break;
        }
    }

    public void SetVSync(TMP_Dropdown VSyncDropdown)
    {
        int nextVSync;
        nextVSync = VSyncDropdown.value;

        QualitySettings.vSyncCount = nextVSync;

    }

    public void SetTextureLimit(TMP_Dropdown textureLimitDropdown)
    {
        int nextTexLimit = textureLimitDropdown.value;
        QualitySettings.masterTextureLimit = nextTexLimit;

    }

    public void SetShadowDistance(TMP_Dropdown shadowDropdown)
    {
      int nextShadow = Mathf.RoundToInt(shadowDropdown.value);
      switch(nextShadow)
      {
        case 0:
        QualitySettings.shadowDistance = 0;
        break;
        case 1:
        QualitySettings.shadowDistance = 5;
        break;
        case 2:
        QualitySettings.shadowDistance = 10;
        break;
        case 3:
        QualitySettings.shadowDistance = 20;
        break;
        case 4:
        QualitySettings.shadowDistance = 30;
        break;
        case 5:
        QualitySettings.shadowDistance = 50;
        break;
        case 6:
        QualitySettings.shadowDistance = 80;
        break;
      }
    }

    public void InvertX(Toggle invertTrue)
    {
        FindObjectOfType<MouseLook>().InvertX(invertTrue.isOn);
        Debug.Log("Set mouse X to " + invertTrue.isOn);
    }
    public void InvertY(Toggle invertTrue)
    {
        FindObjectOfType<MouseLook>().InvertY(invertTrue.isOn);
        Debug.Log("Set mouse Y to " + invertTrue.isOn);
    }

    //Audio settings
    [SerializeField] private TextMeshProUGUI masterVolumeLabel;
    [SerializeField] private TextMeshProUGUI sfxVolumeLabel;
    [SerializeField] private TextMeshProUGUI bgmVolumeLabel;
    [SerializeField] private TextMeshProUGUI dialogueVolumeLabel;
    [SerializeField] private AudioMixer soundMixing;
    public void SetMasterVolume(Slider nextMasterVolume)
    {
        soundMixing.SetFloat("masterVolume", nextMasterVolume.value);
        masterVolumeLabel.text = Mathf.RoundToInt(nextMasterVolume.value * 100f) + " %";
    }
    public void SetSFXVolume(Slider nextSFXVolume)
    {
        soundMixing.SetFloat("sfxVolume", nextSFXVolume.value);
        sfxVolumeLabel.text = Mathf.RoundToInt(nextSFXVolume.value * 100f) + " %";
    }
    public void SetBGMVolume(Slider nextBGMVolume)
    {
        soundMixing.SetFloat("bgmVolume", nextBGMVolume.value);
        bgmVolumeLabel.text = Mathf.RoundToInt(nextBGMVolume.value * 100f) + " %";
    }
    public void SetDialogueVolume(Slider nextDialogueVolume)
    {
        soundMixing.SetFloat("dialogueVolume", nextDialogueVolume.value);
        dialogueVolumeLabel.text = Mathf.RoundToInt(nextDialogueVolume.value * 100f) + " %";
    }


    /*  public void AudioSet(int state)
      {
        switch(state)
        {
          case 0:
          //briefing
          soundMixing.SetFloat("bgmPitch", 1.0f);
          soundMixing.SetFloat("bgmHighPass",10f);
          soundMixing.SetFloat("bgmLowPass",5500f);
          break;
          case 1:
          soundMixing.SetFloat("bgmPitch",1f);
          soundMixing.SetFloat("bgmHighPass",10f);
          soundMixing.SetFloat("bgmLowPass",22000f);
          break;
          case 2:
          //player died
          soundMixing.SetFloat("bgmPitch",0.45f);
          soundMixing.SetFloat("bgmHighPass",2586f);
          soundMixing.SetFloat("bgmLowPass",7514f);
          break;
          case 3:
          //player won the level
          soundMixing.SetFloat("bgmPitch",0.90f);
          soundMixing.SetFloat("bgmHighPass",1000f);
          soundMixing.SetFloat("bgmLowPass",5000f);
          break;
          }
        }
        */
}
