using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerOrbitalCaller : MonoBehaviour
{
    [SerializeField] GameObject laserObj;
    private int ammo;
    //UI element for ammo TextMeshProUGUI ammobar;
    public void CallLaser(Vector3 hitPoint)
    {

    if(hasAmmo()) //remove if tracking on main combat controller
    Instantiate(laserObj,hitPoint,laserObj.transform.rotation);

    ammo--;
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
    void Update()
    {
        
    }
}
