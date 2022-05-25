using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroAutoDespawn : MonoBehaviour
{
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private float destroyTime=10f,moveSpeed=3f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject,destroyTime);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(moveSpeed*Time.deltaTime*direction);
    }
}
