using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using NameReader;
public class PlayerUIManager : MonoBehaviour
{
    //Essential player meters
    [SerializeField] TextMeshProUGUI healthbar,ammobar,sentencebar,namebar;
    
    //Compass
    [SerializeField] Transform objectivePointer,compassPointer;

    [SerializeField] string lastname,firstname,crime;

    NamePicker names;

    float objectivePointerRate=12;
    [SerializeField] Transform northPoint,objectivePoint;
    string healthcolor="white";
    Transform objectiveObj;

    // Start is called before the first frame update
    void Start()
    {
        names = FindObjectOfType<NamePicker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(objectivePoint!=null)
        {
            objectivePoint.LookAt(objectivePoint);
            objectivePointer.rotation = Quaternion.Slerp(objectivePointer.rotation,objectivePoint.rotation,objectivePointerRate*Time.deltaTime);
        }
    }

    public void SetObjective(Transform objective)
    {
        //destroy old objective point?
        //get objective location
        objectiveObj = objective;
        objectivePoint = new GameObject("Waypoint").transform;
        objectivePoint.position = objective.position;
        objectivePoint.rotation = objective.rotation;
    }

    public void SetName()
    {
        namebar.text = names.ReadList("firstname")+", "+names.ReadList("firstname");
    }

    public void SetHealth(int health)
    {

    }
}
