using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UX.CharacterInfo;
using UnityEngine.UI;
public class Scorekeeper : MonoBehaviour
{
    int successfulAssigneeCount,totalAssigneeCount;
    int timeLeft;
    int vipsRouted;
    int stashesFound;
    int stashesTotal;
    int enemiesRouted;
    [System.Serializable]
    public struct NewAssignee
    {
        public string lname;
        public string fname;
        public string crime;
        public string years;
    }

    NewAssignee[] casualties;

    // Start is called before the first frame update
    void Awake()
    {
        casualties = new NewAssignee[256];
        stashesTotal = FindObjectOfType<VIPManager>().totalVIPS();
        DontDestroyOnLoad(this);
    }

    void Start()
    {
       // GameEnd();
    }

    public void GameEnd()
    {
        //Game manager will call this when either time runs out or the last stash is collected... or you run out of cadavers.
        //Get value of: Stashes collected, time elapsed, time remaining, cadavers used, cadavers pardoned, and enemies routed.
        timeLeft =FindObjectOfType<CountupTimer>().timeLeft();
        SceneManager.LoadScene(2);
    }

    public void AssigneeRunEnd(bool reachedObjective, string lname)
    {
        if(reachedObjective) 
        {
            stashesFound++;
            successfulAssigneeCount++;
        }
        else
        {
            LogName(lname,"default","Inefficient Conduct, Family Level");
        }
        totalAssigneeCount++;
    }


    public void LogName(string lname,string fname="default",string crime = "Undisclosed", string years = "default")
    {
        for (int i = 0; i < casualties.Length-1;i++)
        {
            if(casualties[i].lname==string.Empty)
            {
                casualties[i].lname = lname;
                if(fname=="default") casualties[i].fname = FindObjectOfType<NamePicker>().ReadList("firstname");
                else casualties[i].fname = fname;
                if(crime=="default") casualties[i].crime = FindObjectOfType<NamePicker>().ReadList("crimes");
                else casualties[i].crime = crime;
                casualties[i].years = Mathf.RoundToInt(Random.Range(1,150)).ToString(); 
            }
        }
    }
    public string GiveName()
    {
        int searching = 0;
        while(searching < 10000)
        {
        int rng = Random.Range(0,casualties.Length-1);
        if(casualties[rng].lname!=string.Empty)
        {
            return casualties[rng].lname.ToString()+","+casualties[rng].fname.ToString()+","+casualties[rng].crime+","+casualties[rng].years;
        }
        searching++;
        }
        Debug.LogError("name check failed");
        return string.Empty;

    }
    public void KillVIP()
    {
        vipsRouted++;
    }

    public int SentencesServed()
    {
        return successfulAssigneeCount;
    }
    public int TotalCadavers()
    {
        return totalAssigneeCount;
    }

    public int Efficiency()
    {
        return successfulAssigneeCount/totalAssigneeCount*stashesFound/stashesTotal;
    }

    public int EnemiesKilled()
    {
        return enemiesRouted;
    }
    public int VIPKill()
    {
        return vipsRouted;
    }
    public int StashesRecovered()
    {
        return stashesFound;
    }

    public int TimeLeft()
    {
        return timeLeft;
    }

    public int CountScore()
    {
        int finalScore = 0;
        //for the scoreboard scene to call on.
        return finalScore;
    }
}
