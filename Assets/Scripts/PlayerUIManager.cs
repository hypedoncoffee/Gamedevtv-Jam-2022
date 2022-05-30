using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UX.CharacterInfo;
using GameJam.Attributes;

public class PlayerUIManager : MonoBehaviour
{
    //Essential player meters
    [SerializeField] TextMeshProUGUI healthbar,ammobar,sentencebar,namebar;
    
    //Compass
    [SerializeField] Transform objectiveRef,objectivePointer,compassPointer;

    [SerializeField] string lastname,firstname,crime;

    [SerializeField] Slider reqslider;
    NamePicker names;

    float objectivePointerRate=12;
    [SerializeField] Transform northPoint,objectivePoint;
    string healthcolor="white";
    string lastcrime;
    Transform objectiveObj;
    Health healthComponent;

    // Start is called before the first frame update
    void Awake()
    {
        healthComponent = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        names = FindObjectOfType<NamePicker>();
    }

    void Start()
    {
        SetHealth(healthComponent.GetHealth());
    }

    // Update is called once per frame
    void Update()
    {
        if(objectivePoint!=null&&objectiveObj!=null)
        {
            objectivePoint.position = objectiveObj.position;
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

    public void SetHealth(int health)
    {
        
        healthbar.text="<color=" + healthComponent.nextColor(health) + ">" + health.ToString() + " / " + healthComponent.GetMaxHealthString() + " HP";
    }
    public void SetAmmo(int ammo)
    {
        ammobar.text=ammo.ToString();
    }
    public void Recquisition(int req)
    {
        reqslider.value = req;
    }
}
