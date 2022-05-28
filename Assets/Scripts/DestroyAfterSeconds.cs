using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    [Header("Destroy after...")]
    [SerializeField] float timeInSeconds;
    
    [SerializeField] bool onAwake;
    void Awake()
    {
        if(onAwake)
        Destroy(this.gameObject,timeInSeconds);
    }

    public void Destroy()
    {
        Destroy(this.gameObject,timeInSeconds);
    }

    
}
