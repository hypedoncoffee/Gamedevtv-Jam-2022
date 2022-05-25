using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputIntroListener : MonoBehaviour
{
    [SerializeField] AudioSource audio;
    [SerializeField] AudioSource sfxAudio;
    private bool skip;
    [SerializeField] float audioSkipTo = 38.1f;

    public void PlaySFX(AudioClip nextClip)
    {
        sfxAudio.PlayOneShot(nextClip);
    }

    public void IntroFinish()
    {
        skip=true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKeyDown&&!skip)
        {
            IntroFinish();
            GetComponent<Animator>().SetBool("input",true);
             audio.time = audioSkipTo;
        }
    }
}
