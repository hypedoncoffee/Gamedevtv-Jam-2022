using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using GameJam.Control;
public class PlayerOrbitalCaller : MonoBehaviour
{

    Vector3 hitPoint;
    [SerializeField] GameObject laserObj,grenadeObj,smokeObj;
    bool canLaser,canSmoke=true,canGrenade;
    [SerializeField]int smokeRechargeTime=15,grenadeRechargeTime=50,laserRechargeTime=120;
    [SerializeField] PlayerController player;
    //UI element for ammo TextMeshProUGUI ammobar;
    
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    public void UnlockLaser()
    {
        canLaser=true;
    }

    public void UnlockGrenade()
    {
        canGrenade=true;
    }

    public void CallLaser()
    {
        if(player.CanFire("orbital")) //remove if tracking on main combat controller
            {
                player.IncreaseRecquisition(-180);
                player.PrepAbility("orbital");
            }

    }

    public void CallSmoke()
    {

        if(player.CanFire("smoke")) //remove if tracking on main combat controller
        {
            player.IncreaseRecquisition(-60);
            player.PrepAbility("smoke");
        }
    }
    public void CallGrenade()
    {
        if(player.CanFire("grenade")) //remove if tracking on main combat controller
        {
            player.IncreaseRecquisition(-120);
            player.PrepAbility("grenade");
        }
    }

    IEnumerator LaserFire()
    {
        canLaser = false;
        hitPoint = Vector3.zero;
        while(hitPoint == Vector3.zero)
            yield return null;
        Instantiate(smokeObj,hitPoint,laserObj.transform.rotation);
        StartCoroutine(LaserRecharge());
    }
    IEnumerator LaserRecharge()
    {
        yield return new WaitForSeconds(laserRechargeTime);
        canLaser =true;
    }
        IEnumerator SmokeRecharge()
    {
        yield return new WaitForSeconds(smokeRechargeTime);
        canSmoke =true;
    }
        IEnumerator GrenadeRecharge()
    {
        yield return new WaitForSeconds(grenadeRechargeTime);
        canGrenade =true;
    }

        IEnumerator SmokeFire()
    {
        hitPoint = Vector3.zero;
        while(hitPoint == Vector3.zero)
            yield return null;
        Instantiate(smokeObj,hitPoint,laserObj.transform.rotation);
        StartCoroutine(SmokeRecharge());
    }

        IEnumerator GrenadeFire()
    {
        hitPoint = Vector3.zero;
        while(hitPoint == Vector3.zero)
            yield return null;
        Instantiate(grenadeObj,hitPoint,laserObj.transform.rotation);
        StartCoroutine(GrenadeRecharge());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }


    // Update is called once per frame
    void OnClick()
    {
        //Raycast code goes here

        //hitPoint = Vector3 of ray hit location
        
    }
}
