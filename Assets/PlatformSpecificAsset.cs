using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSpecificAsset : MonoBehaviour
{
    [SerializeField] bool windows,osx,linux,webgl,android;
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_ANDROID
        if(!android)
        Destroy(this.gameObject);
        #elif UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
        Debug.Log("Did this run?");
        if(!linux)
        Destroy(this.gameObject);
        #elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if(!windows)Destroy(this.gameObject);
        #elif UNITY_WEBGL
        if(!webgl) Destroy(this.gameObject);
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
