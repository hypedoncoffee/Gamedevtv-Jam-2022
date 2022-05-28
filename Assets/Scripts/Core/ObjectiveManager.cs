using GameJam.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GameJam.Control;

public class ObjectiveManager : MonoBehaviour
{

    [SerializeField] List<GameObject> objectives;
    [SerializeField] public int distanceValue;
    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] GameObject player;

    // TODO: Unused, maybe for directional arrow later ?
    private GameObject closestObjective;
    private float closestObjectiveDistance = Mathf.Infinity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distanceText = GameObject.Find("Objective Label").GetComponent<TextMeshProUGUI>();
    }
    public void GenerateObjectives()
    {
        if (objectives.Count == 0)
        {
            GenerateObjectiveList();
        };
    }

    private void GenerateObjectiveList()
    {
        objectives = new List<GameObject>();
        GameObject[] pickupsInScene = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject objective in pickupsInScene)
        {
            if (!objectives.Contains(objective))
            {
                if (objective.name.Equals("eccFOB_Base-final"))
                {
                    objectives.Insert(0, objective);
                }
                else
                {
                    objectives.Add(objective);
                }
                objective.GetComponent<Objective>().pickedUp += handleObjectivePickup;
            }
        }
    }
    
    public void AddObjective(GameObject newObjective)
    {
        if (!objectives.Contains(newObjective))
        {
            objectives.Add(newObjective);
        }
    }

    public void handleObjectivePickup(GameObject objectiveThatGotPickedUp)
    {
        objectives.Remove(objectiveThatGotPickedUp);
        closestObjectiveDistance = Mathf.Infinity;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.GetComponent<PlayerController>().HasClearanceCode())
        {
            GenerateObjectiveList();
            foreach (GameObject objective in objectives)
            {
                if(objective.GetComponent<Objective>() && objective.GetComponent<Objective>().Equals("FOB"))
                {
                    closestObjective = objective.gameObject;
                }
            }
        }
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
                distanceText.text = String.Format("{0} is your objective. It is {1} m away", closestObjective.GetComponent<Objective>().GetObjectiveName(), closestObjectiveDistance.ToString("N0"));
        }
        //Give player ui the objective reference
        if(closestObjective!=null)
        FindObjectOfType<PlayerUIManager>().SetObjective(closestObjective.transform);
        //Todo: Having no objectives results in the objective never going above 0m distance
    }
}
