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
public int NumberOfVips = 15;
    public void SetDifficulty(int diff)
    {
        NumberOfVips = diff;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
