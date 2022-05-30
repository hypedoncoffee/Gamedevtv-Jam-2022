using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingTime : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(HackTime());        
    }

    // Update is called once per frame
    IEnumerator HackTime()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale=1;        
    }
}
