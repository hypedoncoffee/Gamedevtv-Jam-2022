using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textbox;
    Scorekeeper scoreboard;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        scoreboard= FindObjectOfType<Scorekeeper>();
        textbox.maxVisibleCharacters = 0;
        StartCoroutine(CountTheScore());
    }

    IEnumerator CountTheScore()
    {
        //mission briefing
        textbox.text = "The mission has concluded and Sector 40 has returned to standard procedure.  Your debriefing will follow shortly.";
        
        while (textbox.maxVisibleCharacters < textbox.text.Length)
        {
            yield return new WaitForSecondsRealtime(0.009f);
            textbox.maxVisibleCharacters+=3;
        }
        
        //prompt for click
        while(!Input.anyKeyDown)
        yield return null;
        //erase
        while(textbox.maxVisibleCharacters>0)
            yield return new WaitForSecondsRealtime(0.01f);
            textbox.maxVisibleCharacters-=5;
        scoreboard.CountScore();
        textbox.text = "You destroyed "+scoreboard.VIPKill()+" and completed "+scoreboard.StashesRecovered()+"/10 objectives."+
                        "You used "+scoreboard.SentencesServed()+" assignees.  "+scoreboard.TotalCadavers()+" resolved their pending sentences."+
                        "Efficiency rating: "+scoreboard.Efficiency()+"."+
                        "Operation ended "+scoreboard.TimeLeft()+" before security grid re-enabled."+
                        "Final Score: "+scoreboard.CountScore();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
