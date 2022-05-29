using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPointer : MonoBehaviour
{
    [SerializeField] ParticleSystem pointFX;
    [SerializeField] MeshRenderer rend;
    // Start is called before the first frame update
    void Awake()
    {
        // pointFX = GetComponent<ParticleSystem>();
        // rend = GetComponent<MeshRenderer>();
        
    }

    public void Set(Vector3 newposition)
    {
        rend.enabled=true;
        pointFX.Play();
        transform.position = newposition;
    }

    public void Hide()
    {
        pointFX.Stop();
        rend.enabled=false;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
