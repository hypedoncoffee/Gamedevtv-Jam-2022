using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountupTimer : MonoBehaviour
{
    CharacterTransition deathui;
    private float timeSinceStart;
    TextMeshProUGUI timerText;
    [SerializeField] bool countdown;
    [SerializeField] int maxTime;
    float timeRemaining;
    void Awake()
    {
        deathui = FindObjectOfType<CharacterTransition>();
        if(countdown)
            timeRemaining = maxTime;
        timeSinceStart = 0;
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public void TimeExtension(float newtime)
    {
        timeRemaining+=newtime;
    }

    public int timeLeft()
    {
        return (int)timeRemaining;
    }
    

    // Update is called once per frame
    void Update()
    {
        if(!deathui.transition)
        {

        timeSinceStart += Time.deltaTime;
        if(!countdown)
        timerText.text = String.Format("{0:0} seconds", timeSinceStart);

        else 
        {
            timeRemaining -=Time.deltaTime;
            timerText.text = String.Format("{0:0} until blackout ends", timeRemaining);
            if(timeRemaining<0) FindObjectOfType<Scorekeeper>().GameEnd();
        }
        }
    }
}
