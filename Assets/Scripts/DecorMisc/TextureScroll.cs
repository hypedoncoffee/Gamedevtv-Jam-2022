using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureScroll : MonoBehaviour
{
  MeshRenderer rend;
  [SerializeField] float xScrollSpeed,yScrollSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
      float xOffset = Time.time * xScrollSpeed;
      float yOffset = Time.time * yScrollSpeed;
      rend.material.SetTextureOffset("_MainTex", new Vector2(xOffset, yOffset));
    }
}
