using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameStateUIManager : MonoBehaviour
{
    int successfulRuns;
    int totalRuns;
    [SerializeField] TextMeshProUGUI hitList;
    [SerializeField] TextMeshProUGUI assigneeBox;
    [SerializeField] Image stashImg;
    [SerializeField] Sprite holdingBox,noBox;
    CountupTimer timer;
    [SerializeField] TextMeshProUGUI finalHud;
    VIPManager vipMan;
    string[] vips;
    Scorekeeper scoreMan;
    // Start is called before the first frame update
    void Awake()
    {
        scoreMan = FindObjectOfType<Scorekeeper>();
        timer = FindObjectOfType<CountupTimer>();
        ToggleStashDisplay(false);
        vipMan = FindObjectOfType<VIPManager>();

    }


    public void FinalObjective()
    {
        Transform finalHudBG = finalHud.transform.parent;
        finalHudBG.gameObject.SetActive(true);
        hitList.text = "<targeting priorities rerouted>";
        
        finalHud.gameObject.SetActive(true);
        finalHud.text = "ASSIGNMENT: ELIMINATE RESISTANCE LEADER\n"+vipMan.VIPInfo(vipMan.totalVIPS()-1);
        //music manager: final
    }

    public void UpdateHitList()
    {
        string text = string.Empty;
        for(int i=0; i < vipMan.totalVIPS(); i++)
        {
            Debug.Log(vipMan.isVipDead(i));
            bool dead = vipMan.isVipDead(i);
            if(dead) text= text+"<s>";
            text= text+i.ToString()+"|";
            text = text + vipMan.VIPInfo(i);
            if(dead) text = text+ "</s>";
            text= text+"\n";
            hitList.text=text;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleStashDisplay(bool holding)
    {
        if(holding)
        stashImg.sprite = holdingBox;
        else
        stashImg.sprite = noBox;
    }

    public void AssigneeRunEnd(bool successful)
    {
        if(successful)
        {
            successfulRuns++;
        }
        totalRuns++;
        assigneeBox.SetText(successfulRuns+"|"+totalRuns);
        
    }

    public int TotalRuns()
    {
        return totalRuns;
    }
    public int SuccessfulRuns()
    {
        return successfulRuns;
    }

    public void TimeExtension(float newtime)
    {
        //5.03: moved to countup timer, which reports directly to 
    //if you call it here it passes directly on
    timer.TimeExtension(newtime);
    }
}
