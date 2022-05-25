using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UX.CharacterInfo;
public class PlayerUIManager : MonoBehaviour
{
    //Essential player meters
    [SerializeField] TextMeshProUGUI healthbar,ammobar,sentencebar,namebar;
    
    //Compass
    [SerializeField] Transform objectiveRef,objectivePointer,compassPointer;

    [SerializeField] string lastname,firstname,crime;

    NamePicker names;

    float objectivePointerRate=12;
    [SerializeField] Transform northPoint,objectivePoint;
    string healthcolor="white";
    string lastcrime;
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
            objectiveRef.LookAt(objectivePoint);
            objectivePointer.rotation = Quaternion.Slerp(objectivePointer.rotation,objectiveRef.rotation,objectivePointerRate*Time.deltaTime);
        }
    }

    public void SetObjective(Transform objective)
    {
        if(objective!=objectiveObj)
        {
        //destroy old objective point?

        //get objective location
        objectiveObj = objective;
        objectivePoint = new GameObject("Waypoint").transform;
        objectivePoint.position = objective.position;
        }
    }

    public void SetName(string fname,string lname,string crime,string years)
    {
        namebar.text = lname+", "+fname;
        sentencebar.text = crime+"\n<size=150%>"+years + " yrs.</size>";
    }

    public void SetYears(string years)
    {
        sentencebar.text = lastcrime + "\n<size=150%>"+years+ "yrs.</size>";
    }

    public string nextColor(int nextHealth)
    {
        if (nextHealth > 67)
            return "green";
        
        else if (nextHealth > 33 && nextHealth <= 67)
            return "yellow";
            
            else if (nextHealth <= 33)
            return "red";
            else
                return  "red";
        
    }


    public void SetHealth(int health)
    {
        
        healthbar.text="<color="+nextColor(health)+">"+health.ToString();
    }
    public void SetAmmo(int ammo)
    {
        ammobar.text=ammo.ToString();
    }
}
