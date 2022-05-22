using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicMusicManager : MonoBehaviour
{
    [SerializeField] AudioSource mainAudio,dangerAudio,hypeAudio;

    [SerializeField] private float fadeRate = 20f;
    // Start is called before the first frame update
    void Start()
    {
        //for debug ToggleIntenseMusic(true);
    }

    public void ToggleIntenseMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(hypeAudio));
        else StartCoroutine(FadeOut(hypeAudio));
    }

    public void ToggleDangerMusic(bool enabled)
    {
        if(enabled)
        StartCoroutine(FadeIn(dangerAudio));
        else StartCoroutine(FadeOut(dangerAudio));
    }


    IEnumerator FadeIn(AudioSource bgmLayer)
    {
        while(bgmLayer.volume < 1f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume+=(Time.deltaTime*fadeRate);
        }
        bgmLayer.volume=1f;
}

    IEnumerator FadeOut(AudioSource bgmLayer)
    {
        while(bgmLayer.volume > 0f)
        {
            yield return new WaitForSecondsRealtime(Time.deltaTime);
            bgmLayer.volume-=(Time.deltaTime*fadeRate);
        }
        bgmLayer.volume=0f;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
