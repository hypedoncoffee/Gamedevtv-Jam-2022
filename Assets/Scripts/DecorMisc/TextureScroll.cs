using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextureScroll : MonoBehaviour
{
  MeshRenderer rend;
  RawImage rendraw;
  [SerializeField] float xScrollSpeed,yScrollSpeed;
  [SerializeField] bool raw = false;
    // Start is called before the first frame update
    void Start()
    {
//todo: support raw image for ui

        if(!raw)rend = GetComponent<MeshRenderer>();
        else rendraw = GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
      float xOffset = Time.time * xScrollSpeed;
      float yOffset = Time.time * yScrollSpeed;
      if(!raw)rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
      else rendraw.material.SetTextureOffset("_MainTex",new Vector2(xOffset,yOffset));
    }
}
