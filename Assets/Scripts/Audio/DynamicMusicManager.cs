using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Control;
public class DynamicMusicManager : MonoBehaviour
{
    [SerializeField] float checkTimer=2.5f;
    [SerializeField] float objectiveHypeThreshold=20;
    [SerializeField] PlayerController player;
    [SerializeField] AudioSource mainAudio,dangerAudio,hypeAudio,deathAudio;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] private float fadeRate = 20f;
    bool alive =true;
    // Start is called before the first frame update
    void Start()
    {
        if(player==null) player = FindObjectOfType<PlayerController>();
        if(objectiveManager==null) objectiveManager = FindObjectOfType<ObjectiveManager>();
        StartCoroutine(MusicChecker());
        //for debug ToggleObjectiveMusic(true);
    }

    IEnumerator MusicChecker()
    {   //sorry not sorry
        while(true)
        {
            if(checkTimer==0)
            {
                Debug.LogError("That's a nice infinite loop you got there.  Set Music Manager's check timer.");
                checkTimer = 2;
            } 
            yield return new WaitForSeconds(checkTimer);
            if(alive)
            {

                if(player.IsInCombat())
                {
                    ToggleDangerMusic(true);
                }
                else
                {
                    ToggleDangerMusic(false);
                }
                
                //Calculate absolute distance from player to objective
                float playerDistance = objectiveManager.distanceValue;
                if(playerDistance<objectiveHypeThreshold)
                {
                    ToggleObjectiveMusic(true);
                }
                else
                {
                    ToggleObjectiveMusic(false);
                }
            }
        }

    }
    public void PlayerDeathMusic(bool enabled)
    {
        alive=!enabled;
        hypeAudio.volume = 0;
        dangerAudio.volume = 0;
        ToggleMainMusic(!enabled);
        if(enabled) deathAudio.Play();
        else deathAudio.Stop();
    }

    public void ToggleObjectiveMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(hypeAudio));
        else StartCoroutine(FadeOut(hypeAudio));
    }
    public void ToggleMainMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(mainAudio));
        else StartCoroutine(FadeOut(mainAudio));
    }
    public void ToggleDangerMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(dangerAudio));
        else StartCoroutine(FadeOut(dangerAudio));
    }


    IEnumerator FadeIn(AudioSource bgmLayer)
    {
        while(bgmLayer.volume < 1f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume+=(Time.deltaTime*fadeRate);
        }
        bgmLayer.volume=1f;
}

    IEnumerator FadeOut(AudioSource bgmLayer)
    {
        while(bgmLayer.volume > 0f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume-=(Time.deltaTime*fadeRate);
        }
        bgmLayer.volume=0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
