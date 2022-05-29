using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolStates
{
    alert,idle
}
public class PatrolVoice : MonoBehaviour
{
    PatrolStates state;
    [SerializeField] AudioClip[] idleBanter;
    [SerializeField] AudioClip[] combatBanter;
    [SerializeField] AudioClip[] hurtBanter;
    [SerializeField] AudioClip[] deathBanter;
    [SerializeField] float minIdleBanterTime = 20f;
    [SerializeField] float maxIdleBanterTime = 200f;
    AudioSource voicebox;
    // Start is called before the first frame update
    void Awake()
    {
        voicebox = GetComponent<AudioSource>();
    }



    public void Alert(bool enabled)
    {
        if(enabled)
        {

            state = PatrolStates.alert;
            CallVoice(combatBanter);
        }
        else 
        {
            state = PatrolStates.idle;
            StartCoroutine(IdleBanter());
        }
    }

    void CallVoice(AudioClip[] clips)
    {
        int nextLine = Random.Range(0,clips.Length-1);
        voicebox.PlayOneShot(clips[nextLine]);
    }

    IEnumerator IdleBanter()
    {
        while(state!=PatrolStates.alert)
        {
            yield return new WaitForSeconds(Random.Range(minIdleBanterTime,maxIdleBanterTime));
            CallVoice(idleBanter);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
