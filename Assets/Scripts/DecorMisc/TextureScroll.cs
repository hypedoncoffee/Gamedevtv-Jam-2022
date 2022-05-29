using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextureScroll : MonoBehaviour
{
  MeshRenderer rend;
  double timeLog;
  RawImage rendraw;
  [SerializeField] float xScrollSpeed,yScrollSpeed;
  [SerializeField] bool raw = false;
  [SerializeField] bool isImg;
  [SerializeField] bool isURP = false;
  Image rendimg;
  private float yOffset,xOffset;
  string mapping; 
    // Start is called before the first frame update
    void Awake()
    {
//todo: support raw image for ui
      if(isURP)
        mapping = "_BaseMap";
      else
        mapping = "_MainTex";
      //timeLog = timeLog.realtimeSinceStartup;
        if(!raw&&!isImg)rend = GetComponent<MeshRenderer>();
        else if (isImg) rendimg = GetComponent<Image>();
        else rendraw = GetComponent<RawImage>();
      if(raw)StartCoroutine(TextureRaw());
    }
    void OnEnable()
    {
    if(raw)StartCoroutine(TextureRaw());
    if(isImg) StartCoroutine(Texture());
      
    }

    IEnumerator TextureRaw()
    {
      while(true)
      {
      xOffset +=(.0167f*xScrollSpeed);
      yOffset += (.0167f*xScrollSpeed);
        yield return new WaitForSecondsRealtime(0.0167f);
      rendraw.material.SetTextureOffset(mapping,new Vector2(xOffset,yOffset));
      }
    }
    IEnumerator Texture()
    {
      while(true)
      {
      xOffset +=(.0167f*xScrollSpeed);
      yOffset += (.0167f*xScrollSpeed);
        yield return new WaitForSecondsRealtime(0.0167f);
      rendimg.material.SetTextureOffset(mapping,new Vector2(xOffset,yOffset));
      }
    }

    // Update is called once per frame
    void Update()
    {
      float timeNow = Time.realtimeSinceStartup;
      xOffset = timeNow * xScrollSpeed;
      yOffset = timeNow * yScrollSpeed;
      if(!raw&&!isImg)rend.material.SetTextureOffset(mapping, new Vector2(xOffset, yOffset));
    }
}
