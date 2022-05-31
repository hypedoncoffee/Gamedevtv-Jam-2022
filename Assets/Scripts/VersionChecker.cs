using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class VersionChecker : MonoBehaviour
{
    [SerializeField] TMP_Text textbox;
    // Start is called before the first frame update
    void Start()
    {
        textbox.text = Application.version +"|made for gdtv jam 2022";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
