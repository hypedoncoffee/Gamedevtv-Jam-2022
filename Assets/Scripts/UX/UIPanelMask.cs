using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPanelMask : MonoBehaviour
{
  [SerializeField] private bool hiddenByDefault;
  [SerializeField] private float maskRate;
  //[SerializeField] private bool useXAxis,useYAxis;
  [SerializeField] AudioSource foley;
[SerializeField]  AudioClip swishUp,swishDown;
    // Start is called before the first frame update
    
    void Awake()
    {
      foley = GetComponent<AudioSource>();

    }
    void Start()
    {
      if(foley==null) foley = this.gameObject.AddComponent<AudioSource>();
      
          
      if(hiddenByDefault)
      {
        this.transform.localScale = new Vector3(0,0f,1f);
      }
    }

    public void Reveal()
    {
      foley.PlayOneShot(swishUp);
      StartCoroutine(MaskShow());
    }
    public void Hide()
    {
      foley.PlayOneShot(swishDown);
      StartCoroutine(MaskHide());
    }

    // Update is called once per frame
    IEnumerator MaskShow()
    {
      for(var i = 0f; i < 1f; i+=maskRate)
      {
        yield return new WaitForSecondsRealtime(0.016f);
        this.transform.localScale=new Vector3(transform.localScale.x+maskRate,transform.localScale.y+maskRate,1f);
      }
        this.transform.localScale = new Vector3(1f,1f,1f);
    }

    IEnumerator MaskHide()
    {
      for(var i = 1f; i > 0f; i-=maskRate)
        {
          yield return new WaitForSecondsRealtime(0.016f);
          this.transform.localScale=new Vector3(transform.localScale.x-maskRate,transform.localScale.y-maskRate,1f);
        }
          this.transform.localScale = new Vector3(0,0f,1f);
    }
}
