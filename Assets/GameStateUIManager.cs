using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameStateUIManager : MonoBehaviour
{
    int successfulRuns;
    int totalRuns;
    [SerializeField] TextMeshProUGUI assigneeBox;
    [SerializeField] Image stashImg;
    [SerializeField] Sprite holdingBox,noBox;
    CountupTimer timer;
    // Start is called before the first frame update
    void Awake()
    {
        
        timer = FindObjectOfType<CountupTimer>();
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
        assigneeBox.text = successfulRuns+"/"+totalRuns;
        
    }

    public int TotalRuns()
    {
        return totalRuns;
    }
    public int SuccessfulRuns()
    {
        return successfulRuns;
    }

    public void TimeExtension()
    {
    }
}
