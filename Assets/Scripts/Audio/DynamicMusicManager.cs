using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameJam.Control;
public class DynamicMusicManager : MonoBehaviour
{
    [SerializeField] float checkTimer=2.5f;
    [SerializeField] float objectiveHypeThreshold=20;
    [SerializeField] PlayerController player;
    [SerializeField] AudioSource mainAudio,dangerAudio,hypeAudio,deathAudio,finaleAudio;
    [SerializeField] ObjectiveManager objectiveManager;
    [SerializeField] private float fadeRate = 20f;

//    double timeLogged = Time.realtimeSinceStartup;
    bool alive =true;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (player) {
                player = playerObject.GetComponent<PlayerController>();
            }
        }
        if (objectiveManager==null) objectiveManager = FindObjectOfType<ObjectiveManager>();
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
            yield return new WaitForSecondsRealtime(checkTimer);
            if(alive)
            {
                // Check if player wasn't active in scene yet
                if (!player)
                {
                    // Try to get the player
                    player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
                    if (!player)
                    {
                        // No player yet.. try again later
                        yield return new WaitForSecondsRealtime(checkTimer);
                    }
                } else
                {
                    // Have a player, do work
                    HandleDynamicMusicSincePlayerExists();
                }
            }
            else 
            {
                ToggleDangerMusic(false);
                ToggleObjectiveMusic(false);
            }
        }

    }

    private void HandleDynamicMusicSincePlayerExists()
    {
        if (player.IsInCombat())
        {
            ToggleDangerMusic(true);
        }
        else
        {
            ToggleDangerMusic(false);
        }

        //Calculate absolute distance from player to objective
        if (!objectiveManager) { objectiveManager= FindObjectOfType<ObjectiveManager>(); }
        float playerDistance = objectiveManager.distanceValue;
        if (playerDistance < objectiveHypeThreshold)
        {
            ToggleObjectiveMusic(true);
        }
        else
        {
            ToggleObjectiveMusic(false);
        }
    }

    public void PlayerDeathMusic(bool enabled,bool hardStop=false)
    {
        alive=!enabled;
        hypeAudio.volume = 0;
        dangerAudio.volume = 0;
        ToggleMainMusic(!enabled,hardStop);
        if(enabled) deathAudio.Play();
        else deathAudio.Stop();
    }

    public void ToggleObjectiveMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(hypeAudio));
        else StartCoroutine(FadeOut(hypeAudio));
    }

    public void ToggleFinaleMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(finaleAudio));
        else StartCoroutine(FadeOut(finaleAudio));
    }

    public void ToggleMainMusic(bool enabled,bool hardStop=false)
    {
        if(enabled)
        StartCoroutine(FadeIn(mainAudio));
        else StartCoroutine(FadeOut(mainAudio,hardStop));
    }
    public void ToggleDangerMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(dangerAudio));
        else StartCoroutine(FadeOut(dangerAudio));
    }


    IEnumerator FadeIn(AudioSource bgmLayer,bool hardStop = false)
    {
        if(bgmLayer!=null)
        {

        if(!hardStop)
        {

        while(bgmLayer.volume < 1f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume+=(Time.deltaTime*fadeRate);
        }
        }
        bgmLayer.volume=1f;
        }
}

    IEnumerator FadeOut(AudioSource bgmLayer,bool hardStop = false)
    {
        if(bgmLayer!=null)
        {

        if(!hardStop)
        {

        
        while(bgmLayer.volume > 0f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume-=(Time.deltaTime*fadeRate);
        }
        }
        bgmLayer.volume=0f;
        
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
