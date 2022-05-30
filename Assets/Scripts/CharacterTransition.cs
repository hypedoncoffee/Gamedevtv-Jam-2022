using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameJam.Combat;

public class CharacterTransition : MonoBehaviour
{   
    //Audio
    [SerializeField] AudioSource paAudio;
    [SerializeField] AudioClip[] successVoice,failureVoice;
    [SerializeField] AudioClip firstRunVoice;

    public bool transition;
    //Main
    [SerializeField] private UIPanelMask deathScreen;
    [SerializeField] private TextMeshProUGUI textbox;
    [SerializeField] private bool firstRun = true;
    [SerializeField] int charsEraseRate = 8;
    [SerializeField] PlayerUIManager playerUI;
    [SerializeField] float textScrollRate = .015f;
    [SerializeField] AudioClip textScrollBlip,eraseBlip;
    [SerializeField] AudioSource textAudio;
    // Start is called before the first frame update

    bool typing;
    GameObject playerObject;


    private void Awake()
    {
        textbox.maxVisibleCharacters=0;
    }


    string DeathDate()
    {
        int m = Mathf.RoundToInt(Random.Range(1,12));
        int d = Mathf.RoundToInt(Random.Range(1,30));
        if(m == 2) Mathf.Clamp(d,1,28);
        int y = Mathf.RoundToInt(Random.Range(2070,2082));
        return m.ToString()+"/"+d.ToString()+"/"+y.ToString();
    }

    public void DisplayNewCharacter(bool reachedObjective,string fname,string lname,string crime,string years)
    {
        transition=true;
        foreach (Suspicion sus in FindObjectsOfType<Suspicion>()) sus.Ignorance(true);
        Debug.Log("Changing Characters Now.");
        textbox.maxVisibleCharacters=0;
        string resultscolor;
        string resultsmessage;
        string characterText;
        if(firstRun)
        {
            resultscolor = "\t";
            resultsmessage = "New Handler:\n\nThe following district is in a state of chaotic rebellion and quick action must be taken to minimize long-term repercussions to the greater state.\n\n"+
            "You will be assigned formerly deceased prisoners that will act accordingly in exchange for a removal of their remaining sentence.\n\n"+
            "<color=green>You will be rewarded based on your efficient usage of the resources provided.</color>\n\n"+
            "<color=red>Failure to preserve the asignees until their sentence has been fully amended will be met with consequences on your final record.\n\n"+
            "<color=grey>Press any key to acknowledge your understanding of the assignment.\n\n";
           
        }
        else 
        {
            // Died with clearance codes
            if(reachedObjective)
            {
                resultscolor = "<color=green>";
                resultsmessage = "Assignee has completed their sentence and is now relieved of duty.\n\n";
            }
            else
            {
                // Died without getting clearance codes
                resultscolor = "<color=red>";
                resultsmessage = "Assignee was terminated prematurely and body is beyond repair; specimen cannot be recovered.\nRemaining sentence will be commuted to next of kin.\n\nThis will reflect poorly on your efficiency rating.\n\n";
            }
        }
        characterText = "NAME: "+lname+", "+fname+"\n"+
                        "OFFENSE: "+crime+"\n"+
                        "BAIL OFFERING:"+System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol+BailOffering()+"\n"+
                        "PUNISHMENT: "+ (System.Int32.Parse(years) + Mathf.RoundToInt(Random.Range(0,70)))+" years \n"+
                        "EXPIRATION DATE: "+DeathDate()+"\n"+
                        "REMAINING SENTENCE: "+years+" years\n";
        textbox.text = resultscolor + resultsmessage +"</color>"+ characterText;
        StartCoroutine(AssignCharactersScreen(resultsmessage,characterText,fname,lname,crime,years,reachedObjective));
    }

    public string BailOffering()
    {
        int rng = Random.Range(0,50);
        if(rng < 45) return Mathf.RoundToInt(Random.Range(1,900)).ToString()+",000,000.00";
        else return "N/A";
    }
    IEnumerator AssignCharactersScreen(string firstmessage,string charactermessage,string fname,string lname,string crime,string years,bool reachedObjective)

    {
        //Pause game (Disable this since it stops videos)
        //Time.timeScale = 0;
        
        //
        float multiplier = 1;
        int reducechars = 4;
        int charsScrollRate = 3;
        if(firstRun)
        {
            //paAudio.PlayOneShot(firstRunVoice);
            reducechars += 44;
            multiplier = 5f;
            charsScrollRate = 5;
        }
        if(!firstRun&&reachedObjective)
        {
            paAudio.PlayOneShot(successVoice[Random.Range(0,successVoice.Length-1)]);
        }
        else paAudio.PlayOneShot(failureVoice[Random.Range(0,failureVoice.Length-1)]);
        FindObjectOfType<DynamicMusicManager>().PlayerDeathMusic(true,true);
        deathScreen.Reveal();
        yield return new WaitForSecondsRealtime(1f);
        typing=true;
        string nextmsg = firstmessage;
        //scroll text


        //Success failure prompt
        while(typing)
        {
            textAudio.PlayOneShot(textScrollBlip);
            yield return new WaitForSecondsRealtime(textScrollRate/multiplier);
            textbox.maxVisibleCharacters+=charsScrollRate;

            if(textbox.maxVisibleCharacters > nextmsg.Length-reducechars)
                typing=false;
        }
        if(!firstRun)
        yield return new WaitForSecondsRealtime(1f);

        //Get new character name
        // if (!playerUI)
        // {
        //     playerUI = FindObjectOfType<PlayerUIManager>().SetName(fname, lname, crime, years);
        // }

        //Startup screen erase
        typing =true;
        nextmsg = nextmsg + charactermessage;
        if(firstRun)
        {
            while(!Input.anyKeyDown)
            {
                yield return null;
            }
            while(textbox.maxVisibleCharacters>0)
            {
                yield return new WaitForSecondsRealtime(textScrollRate/10f);
                textbox.maxVisibleCharacters-=charsEraseRate;
                textAudio.PlayOneShot(eraseBlip);
            }
            textbox.text = charactermessage;
            firstRun=false;
            nextmsg = charactermessage;
        }
        //scroll text

        int nextLength = nextmsg.Length-1;
        if (firstRun) nextLength = charactermessage.Length-1;
        while(typing)
        {
            textAudio.PlayOneShot(textScrollBlip);
            yield return new WaitForSecondsRealtime(textScrollRate);
            textbox.maxVisibleCharacters++;

            if(textbox.maxVisibleCharacters > nextmsg.Length)
                typing=false;
        }
        yield return new WaitForSecondsRealtime(4f);
    
    
        Time.timeScale = 1;
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().SetBool("alive",true);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>().ResetTrigger("death");
        foreach (Suspicion sus in FindObjectsOfType<Suspicion>()) sus.Ignorance(true,5f);
        deathScreen.Hide();
        transition=false;
        FindObjectOfType<DynamicMusicManager>().PlayerDeathMusic(false);
    }

    public void SetPlayerobject(GameObject gameObject)
    {
        playerObject = gameObject;
    }
}
