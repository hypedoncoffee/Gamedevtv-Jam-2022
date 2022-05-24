using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Control;
public class DynamicMusicManager : MonoBehaviour
{
    [SerializeField] float checkTimer=2.5f;
    [SerializeField] float objectiveHypeThreshold=20;
    [SerializeField] PlayerController player;
    [SerializeField] AudioSource mainAudio,dangerAudio,hypeAudio;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] private float fadeRate = 20f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MusicChecker());
        //for debug ToggleIntenseMusic(true);
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
            ToggleIntenseMusic(true);
        }
        else
        {
            ToggleIntenseMusic(false);
        }
        }
    }

    public void ToggleIntenseMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(hypeAudio));
        else StartCoroutine(FadeOut(hypeAudio));
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
