using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTransition : MonoBehaviour
{
    [SerializeField] private UIPanelMask deathScreen;
    [SerializeField] private TextMeshProUGUI textbox;
    PlayerUIManager playerUI;
    // Start is called before the first frame update

    bool typing;


    void Start()
    {
        playerUI = FindObjectOfType<PlayerUIManager>();
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
    public void DisplayNewCharacter(bool success,string fname,string lname,string crime,string years)
    {
        textbox.maxVisibleCharacters=0;
        string resultscolor;
        string resultsmessage;
        string characterText;
        if(success)
        {
            resultscolor = "<color=green>";
            resultsmessage = "Assignee has completed their sentence and is now relieved of duty.\n\n";
        }
        else
        {
            resultscolor = "<color=red>";
            resultsmessage = "Assignee was terminated prematurely and body is beyond repair; specimen cannot be recovered.\nRemaining sentence will be commuted to next of kin.\n\nThis will reflect poorly on your efficiency rating.\n\n";
        }
        characterText = "NAME: "+lname+", "+fname+"\n"+
                                "OFFENSE: "+crime+"\n"+
                                "BAIL OFFERING:"+System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol+BailOffering()+"\n"+
                                "PUNISHMENT: "+ (System.Int32.Parse(years) + Mathf.RoundToInt(Random.Range(0,70)))+" years \n"+
                                "EXPIRATION DATE: "+DeathDate()+"\n"+
                                "REMAINING SENTENCE: "+years+" years\n";
        textbox.text = resultscolor + resultsmessage +"</color>"+ characterText;
        StartCoroutine(AssignCharactersScreen(resultsmessage,characterText,fname,lname,crime,years));
    }

    public string BailOffering()
    {
        int rng = Random.Range(0,50);
        if(rng < 45) return Mathf.RoundToInt(Random.Range(1,900)).ToString()+",000,000.00";
        else return "N/A";
    }
[SerializeField] float textScrollRate = .064f;
[SerializeField] AudioClip textScrollBlip;
[SerializeField] AudioSource textAudio;
    IEnumerator AssignCharactersScreen(string firstmessage,string charactermessage,string fname,string lname,string crime,string years)

    {
        FindObjectOfType<DynamicMusicManager>().PlayerDeathMusic(true);
        deathScreen.Reveal();
        yield return new WaitForSeconds(1f);
        typing=true;
        string nextmsg = firstmessage;
        //scroll text
        while(typing)
        {
            textAudio.PlayOneShot(textScrollBlip);
            yield return new WaitForSeconds(textScrollRate);
            textbox.maxVisibleCharacters++;

            if(textbox.maxVisibleCharacters > nextmsg.Length-1)
                typing=false;
        }
        yield return new WaitForSeconds(1f);
        playerUI.SetName(fname,lname,crime,years);
        typing=true;
        nextmsg = nextmsg + charactermessage;
        //scroll text
        while(typing)
        {
            textAudio.PlayOneShot(textScrollBlip);
            yield return new WaitForSeconds(textScrollRate);
            textbox.maxVisibleCharacters++;

            if(textbox.maxVisibleCharacters > nextmsg.Length)
                typing=false;
        }
        yield return new WaitForSeconds(1f);
        
        deathScreen.Hide();
        FindObjectOfType<DynamicMusicManager>().PlayerDeathMusic(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
