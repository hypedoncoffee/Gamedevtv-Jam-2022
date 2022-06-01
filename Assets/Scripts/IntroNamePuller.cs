using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UX.CharacterInfo;
public class IntroNamePuller : MonoBehaviour
{
    [SerializeField] int minSentence=20,maxSentence=300;
    TMP_Text characterText;
    NamePicker names;
    Scorekeeper score;
    [SerializeField] bool outro;
    // Start is called before the first frame update
    void Awake()
    {
        characterText = GetComponent<TMP_Text>();
        names = FindObjectOfType<NamePicker>();
         if(outro) score =  FindObjectOfType<Scorekeeper>();
        Generate();
    }

    void Start(){

    }   

    void Generate()
    {
        if(!outro)
        {

        string lastName= names.ReadList("lastname");
        string firstName = names.ReadList("firstname");
        string crime = names.ReadList("crime");
        int years = Mathf.RoundToInt(Random.Range(minSentence,maxSentence));
        characterText.text = "NAME: "+lastName+", "+firstName+"\n"+
                        "OFFENSE: "+crime+"\n"+
                        "BAIL OFFERING: "+System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol+BailOffering()+"\n"+
                        "PUNISHMENT: "+ (years + Mathf.RoundToInt(Random.Range(0,70)))+" years \n"+
                        "EXPIRATION DATE: "+DeathDate()+"\n"+
                        "REMAINING SENTENCE: "+years+" years\n";
        }
        else
        {
            string[] data = score.GiveName().Split(',');
        characterText.text = "NAME: "+data[0]+", "+data[1]+"\n"+
                        "OFFENSE: "+data[2]+"\n"+
                        "BAIL OFFERING: "+System.Globalization.NumberFormatInfo.CurrentInfo.CurrencySymbol+BailOffering()+"\n"+
                        "PUNISHMENT: "+ (data[3])+" years \n"+
                        "EXPIRATION DATE: "+"8/28/2081"+"\n"+
                        "REMAINING SENTENCE: "+(data[3])+" years\n";
            
        }
    }

    string DeathDate()
    {
        int m = Mathf.RoundToInt(Random.Range(1,12));
        int d = Mathf.RoundToInt(Random.Range(1,30));
        if(m == 2) Mathf.Clamp(d,1,28);
        int y = Mathf.RoundToInt(Random.Range(2070,2081));
        return m.ToString()+"/"+d.ToString()+"/"+y.ToString();
        
        
    }



    public string BailOffering()
    {
        int rng = Random.Range(0,50);
        if(rng < 45) return Mathf.RoundToInt(Random.Range(1,900)).ToString()+",000,000.00";
        else return "N/A";
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
