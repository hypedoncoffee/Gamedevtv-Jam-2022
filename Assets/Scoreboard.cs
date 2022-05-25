using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        StartCoroutine(CountTheScore());
    }

    IEnumerator CountTheScore()
    {
        //mission briefing

        //prompt for click
        while(!Input.anyKeyDown)
        yield return null;
        //erase

        FindObjectOfType<Scorekeeper>().CountScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
