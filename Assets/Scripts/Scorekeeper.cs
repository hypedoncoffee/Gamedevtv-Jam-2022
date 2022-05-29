using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Scorekeeper : MonoBehaviour
{
    int successfulAssigneeCount,totalAssigneeCount;
    int timeLeft;
    int vipsRouted;
    int stashesFound;
    int stashesTotal = 10;
    int enemiesRouted;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void GameEnd()
    {
        //Game manager will call this when either time runs out or the last stash is collected... or you run out of cadavers.
        //Get value of: Stashes collected, time elapsed, time remaining, cadavers used, cadavers pardoned, and enemies routed.

        SceneManager.LoadScene(2);
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
        TextMeshProUGUI scoreboardtext = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        int finalScore = 0;
        //for the scoreboard scene to call on.
        return finalScore;
    }
}
