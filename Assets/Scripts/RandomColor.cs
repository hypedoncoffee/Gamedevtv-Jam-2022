using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
  //Picks a random color
  //Changes: Added sliders in inspector for color range, was originally hardcoded
  [Header("Hue (l-r = red - violet)")]
  [Range(0,1f)][SerializeField] float hueMin=0f;
  [Range(0,1f)][SerializeField] float hueMax=1f;
  [Header("Saturation (higher is color-y-er)")]
  [Range(0,1f)][SerializeField] float satMin=0f;
  [Range(0,1f)][SerializeField] float satMax=1f;
  [Header("Value (higher is brighter)")]
  [Range(0,1f)][SerializeField] float valMin=0f;
  [Range(0,1f)][SerializeField] float valMax=1f;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.color = Random.ColorHSV(hueMin, hueMax, satMin, satMax, valMin, valMax);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
