using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerOrbitalCaller : MonoBehaviour
{

    Vector3 hitPoint;
    [SerializeField] GameObject laserObj;
    private int ammo;
    //UI element for ammo TextMeshProUGUI ammobar;
    public void CallLaser()
    {

        if(hasAmmo()) //remove if tracking on main combat controller
        {

            StartCoroutine(LaserFire());
            ammo--;
        }
    }


    IEnumerator LaserFire()
    {
        hitPoint = null;
        while(hitPoint == null)
            yield return null;
        Instantiate(laserObj,hitPoint,laserObj.transform.rotation);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    bool hasAmmo()
    {
        if(ammo>0) return true;
        else return false;
    }

    // Update is called once per frame
    void OnClick()
    {
        //Raycast code goes here

        //hitPoint = Vector3 of ray hit location
        
    }
}
