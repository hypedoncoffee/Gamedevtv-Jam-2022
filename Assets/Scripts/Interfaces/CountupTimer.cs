using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountupTimer : MonoBehaviour
{
    private float timeSinceStart;
    Text timerText;

    void Awake()
    {
        timeSinceStart = 0;
        timerText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceStart += Time.deltaTime;
        timerText.text = String.Format("{0:0} seconds", timeSinceStart);
    }
}
