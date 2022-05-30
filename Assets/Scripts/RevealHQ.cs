using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealHQ : MonoBehaviour
{
    [SerializeField] AudioClip revealAudio;
    Animator anim;
    // Start is called before the first frame update
    void Awake()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RevealBase()
    {
        anim.SetBool("reveal",true);
        GetComponent<AudioSource>().PlayOneShot(revealAudio);//play sound?
        StartCoroutine(SetupLoop());
    }

    public void KillPlayer()
    {

    }
    public void HideBase()
    {
        if(anim.GetBool("reveal")==true)
        {

        anim.SetBool("reveal",false);
        GetComponent<AudioSource>().Stop();
        }
    }

    IEnumerator SetupLoop()
    {
        yield return new WaitForSeconds(revealAudio.length-2);
        
        GetComponent<AudioSource>().Play();
    }
}
