using GameJam.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] List<GameObject> objectives;
    [SerializeField] public int distanceValue;
    //[SerializeField] Text distanceText;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject player;

    // TODO: Unused, maybe for directional arrow later ?
    private GameObject closestObjective;
    private float closestObjectiveDistance = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        // Allow time for pickups to spawn
        Invoke("GenerateInitialPickups", 1f);
    }
    private void GenerateInitialPickups()
    {
        if (objectives.Count == 0)
        {
            objectives = new List<GameObject>();
            GameObject[] pickupsInScene = GameObject.FindGameObjectsWithTag("Objective");
            foreach (GameObject objective in pickupsInScene)
            {
                if (!objectives.Contains(objective))
                {
                    objectives.Add(objective);
                    objective.GetComponent<ItemPickup>().pickedUp += handleObjectivePickup;
                }
            }
        };
    }

    private void handleObjectivePickup(GameObject objectiveThatGotPickedUp)
    {
        objectives.Remove(objectiveThatGotPickedUp);
        closestObjectiveDistance = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject objective in objectives)
        {
            // TODO - See if vector is ever negative
            float distanceToObjective = Mathf.Abs(Vector3.Distance(player.transform.position, objective.transform.position));
            if (distanceToObjective < closestObjectiveDistance)
            {
                closestObjectiveDistance = distanceToObjective;
                closestObjective = objective;
            }
        }
        if (closestObjectiveDistance == Mathf.Infinity)
        {
            distanceText.text = "No objectives";
        } else
        {
            distanceValue = (int)(closestObjectiveDistance);
            if(distanceText!=null)
                distanceText.text = String.Format("{0} m", closestObjectiveDistance.ToString("N0"));
        }
        //Give player ui the objective reference
        if(closestObjective!=null)
        FindObjectOfType<PlayerUIManager>().SetObjective(closestObjective.transform);
        //Todo: Having no objectives results in the objective never going above 0m distance
    }
}
