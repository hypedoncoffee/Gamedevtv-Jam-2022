using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealHQ : MonoBehaviour
{
    [SerializeField] AudioClip revealAudio;
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        RevealBase();
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

    IEnumerator SetupLoop()
    {
        yield return new WaitForSeconds(revealAudio.length-1);
        
        GetComponent<AudioSource>().Play();
    }
}
