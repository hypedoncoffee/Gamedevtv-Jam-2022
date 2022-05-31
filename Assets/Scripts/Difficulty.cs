using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulty : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       DontDestroyOnLoad(this.gameObject); 
    }
    public int baseGuard;
    public int guardsPerIncrease;
    public int increaseRate;
public int NumberOfVips = 15;
    public void SetDifficulty(int diff)
    {
        NumberOfVips = diff;
        switch(Mathf.RoundToInt(diff/2))
        {
            case 0:
            baseGuard = 3;
            increaseRate = 3;
            guardsPerIncrease = 2;
            break;
            case 1:
            baseGuard = 4;
            increaseRate = 3;
            guardsPerIncrease = 2;
            break;
            case 2:
            baseGuard = 4;
            increaseRate = 3;
            guardsPerIncrease = 3;
            break;
            case 3:
            baseGuard = 4;
            increaseRate = 3;
            guardsPerIncrease = 3;
            break;
            case 4:
            baseGuard = 3;
            increaseRate = 3;
            guardsPerIncrease = 4;
            break;
            case 5:
            baseGuard = 5;
            increaseRate = 3;
            guardsPerIncrease = 3;
            break;
            case 6:
            baseGuard = 5;
            increaseRate = 2;
            guardsPerIncrease = 3;
            break;
            case 7:
            baseGuard = 6;
            increaseRate = 2;
            guardsPerIncrease = 3;
            break;
            case 8:
            baseGuard = 5;
            increaseRate = 2;
            guardsPerIncrease = 4;
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
