using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    AudioSource voicebox;

    [SerializeField] AudioClip[] idleBanter;
    [SerializeField] AudioClip[] combatBanter;
    [SerializeField] AudioClip[] tauntBanter;
    [SerializeField] AudioClip[] hurtBanter;
    [SerializeField] AudioClip[] deathBanter;
    [SerializeField] AudioClip[] stashFoundAnnouncement;
    [SerializeField] AudioClip[] stashDropAnnouncement;

    [Space(5)]
    [SerializeField] AudioClip[] playerDeathAnnouncement;
    [SerializeField] AudioClip[] playerObjectiveAnnouncement;
    // Start is called before the first frame update
    void Awake()
    {
        voicebox = GetComponent<AudioSource>();
    }

    public void PlayDeathSound(bool reachedObjective)
    {
        if(reachedObjective) CallVoice(playerObjectiveAnnouncement);
        else CallVoice(playerDeathAnnouncement);
    }

    public void FoundStash()
    {
        CallVoice(stashFoundAnnouncement);
    }

    void CallVoice(AudioClip[] clips)
    {
        int nextLine = Random.Range(0,clips.Length-1);
        if (!voicebox)
        {
            voicebox = GetComponent<AudioSource>();
            voicebox.PlayOneShot(clips[nextLine]);
        }
    }
}
