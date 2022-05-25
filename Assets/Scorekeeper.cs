using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
public class Scorekeeper : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnd()
    {
        //Game manager will call this when either time runs out or the last stash is collected... or you run out of cadavers.
        //Get value of: Stashes collected, time elapsed, time remaining, cadavers used, cadavers pardoned, and enemies routed.

        SceneManager.LoadScene(2);
    }

    public int CountScore()
    {
        TextMeshProUGUI scoreboardtext = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        int finalScore = 0;
        //for the scoreboard scene to call on.
        return finalScore;
    }
}
