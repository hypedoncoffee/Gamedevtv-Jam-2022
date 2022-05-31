using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatrolStates
{
    alert,idle
}
public class PatrolVoice : MonoBehaviour
{
    CharacterTransition deathScreen;
    PatrolStates state;
    [SerializeField] AudioClip[] idleBanter;
    [SerializeField] AudioClip[] combatBanter;
    [SerializeField] AudioClip[] tauntBanter;
    [SerializeField] AudioClip[] hurtBanter;
    [SerializeField] AudioClip[] deathBanter;
    [SerializeField] AudioClip[] susBanter;
    [SerializeField] float minIdleBanterTime = 20f;
    [SerializeField] float maxIdleBanterTime = 200f;
    AudioSource voicebox;
    // Start is called before the first frame update
    void Awake()
    {
        deathScreen = FindObjectOfType<CharacterTransition>();
        voicebox = GetComponent<AudioSource>();
    }

int tauntChance = 10;

    public void Alert(bool enabled,bool isVIP = false)
    {
        if(enabled)
        {
            state = PatrolStates.alert;
            if(isVIP)
                CallVoice(tauntBanter);
            else
            {

            int rng = Random.Range(0,100);
            if(rng < tauntChance) CallVoice(tauntBanter);
            else
            {
                CallVoice(combatBanter);
            }
            }
        }
        else 
        {
            state = PatrolStates.idle;
            StartCoroutine(IdleBanter());
        }
    }

    public void DeathSound()
    {
        CallVoice(deathBanter);
    }
    public void HeardSomething()
    {
        CallVoice(susBanter);
    }

    public void HurtSound()
    {
        CallVoice(hurtBanter);
    }

    void CallVoice(AudioClip[] clips)
    {
        if(!deathScreen.transition)
        {

        int nextLine = Random.Range(0,clips.Length-1);
        voicebox.PlayOneShot(clips[nextLine]);
        }
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
