using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountupTimer : MonoBehaviour
{
    private float timeSinceStart;
    TextMeshProUGUI timerText;
    [SerializeField] bool countdown;
    [SerializeField] int maxTime;
    float timeRemaining;
    void Awake()
    {
        if(countdown)
            timeRemaining = maxTime;
        timeSinceStart = 0;
        timerText = GetComponent<TextMeshProUGUI>();
    }

    public int timeLeft()
    {
        return (int)timeRemaining;
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        if(!countdown)
        timerText.text = String.Format("{0:0} seconds", timeSinceStart);

        else 
        {
            timeRemaining -=Time.deltaTime;
            timerText.text = String.Format("{0:0} until blackout ends", timeRemaining);
        }
    }
}
