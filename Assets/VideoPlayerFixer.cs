using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Video;
public enum AsciiVideo
{
    SKULL_LOGO,ASSIGNING_TEXT,
    DIRECTIVE_FOB,DIRECTIVE_VIP,DIRECTIVE_STASH,
    REVIVAL_M,REVIVAL_F

}
public class VideoPlayerFixer : MonoBehaviour
{
    [SerializeField] AsciiVideo displayVideo;
    


     [System.Serializable] public struct VideoFile
    {
         public AsciiVideo videoFile;
         public string filename;
    } 
    
    
    [SerializeField]VideoFile[] videoOptions;
    string pathToVideo;
    string extension;
    string currentPlatform;
    VideoPlayer vp;
    // Start is called before the first frame update
    void Awake()
    {
        vp = GetComponent<VideoPlayer>();


        #if UNITY_EDITOR_LINUX || UNITY_LINUX
        /*#elif UNITY_STANDALONE_WIN
        vp.url = pathToMyWmvFile;
        */
        currentPlatform = "Linux";
        extension = ".webm";
        #elif UNITY_ANDROID
        currentPlatform = "Android";
        extension = ".webm";
        #elif UNITY_EDITOR_WINDOWS || UNITY_STANDALONE_WIN
        currentPlatform = "Windows";
        extension = ".mp4";
        #elif UNITY_WEBGL
        currentPlatform = "WebGL";
        extension = ".mp4";
        #endif
        Debug.Log(Path.DirectorySeparatorChar.ToString());
        pathToVideo = (Application.streamingAssetsPath
                        +Path.DirectorySeparatorChar.ToString() 
                    +"Video" + Path.DirectorySeparatorChar.ToString()
                    +currentPlatform 
                        +Path.DirectorySeparatorChar.ToString() 
                    +videoOptions[(int)(displayVideo)].filename.ToString()
                    +extension);
        vp.url = pathToVideo;
        vp.Play();


    }

    // Update is called once per frame
    void Update()
    {
        
    }





}
