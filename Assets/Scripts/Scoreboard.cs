
using UX.CharacterInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
        textbox.text = "The mission has concluded and District 122 has regained access to communications.\nLast night's events are considered an unknown anomaly and no reports have been made by living witnesses.\n.Your debriefing will follow shortly.\n(Press any key.)";
        
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
        {

            yield return new WaitForSecondsRealtime(0.01f);
            textbox.maxVisibleCharacters-=5;
        }
        scoreboard.CountScore();
        textbox.text = "You destroyed "+scoreboard.VIPKill()+" and completed "+scoreboard.StashesRecovered()+"/10 objectives."+
                        "You used "+scoreboard.SentencesServed()+" assignees.  "+scoreboard.TotalCadavers()+" resolved their pending sentences."+
                        "Efficiency rating: "+scoreboard.Efficiency()+"."+
                        "Operation ended "+scoreboard.TimeLeft()+" before security grid re-enabled."+
                        "Final Score: "+scoreboard.CountScore()+
                        "...this scoreboard didn't get functionally completed in time for jam.  We'll get around to it some time after post. Sorry about that.";
       while (textbox.maxVisibleCharacters < textbox.text.Length)
        {
            yield return new WaitForSecondsRealtime(0.009f);
            textbox.maxVisibleCharacters+=3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BackToSart()
    {
        Destroy(FindObjectOfType<Difficulty>().gameObject);
        Destroy(FindObjectOfType<Scorekeeper>().gameObject);
        Destroy(FindObjectOfType<NamePicker>().gameObject);
    SceneManager.LoadScene(0);
    }
}
