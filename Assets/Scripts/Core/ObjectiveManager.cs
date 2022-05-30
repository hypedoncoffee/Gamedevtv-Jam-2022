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

    private VIPManager vipManager = null;
    private GameObject closestObjective;
    private float closestObjectiveDistance = Mathf.Infinity;
    private Vector3 playerLastPosition;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        distanceText = GameObject.Find("Objective Label").GetComponent<TextMeshProUGUI>();
        vipManager = GameObject.FindObjectOfType<VIPManager>();
    }
    void Start()
    {
        StartCoroutine(WhatIsThisCodeLol());
    }
    IEnumerator WhatIsThisCodeLol()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("To all future employers, please turn away.");
    }
    

    void LateUpdate()
    {
        FindClosestObjectiveAndSet();
        UpdateTopLeftUiText();
        SetPlayerUiObjective();
    }
    public void ResetObjectiveList()
    {
        objectives = new List<GameObject>();
        vipManager.SpawnVIP();
        closestObjectiveDistance = Mathf.Infinity;
        GameObject[] pickupsInScene = GameObject.FindGameObjectsWithTag("Objective");
        foreach (GameObject objective in pickupsInScene)
        {
            if (!objectives.Contains(objective))
            {
                if (objective.GetComponent<Objective>().GetObjectiveName().Equals("FOB"))
                {
                    objectives.Insert(0, objective);
                }
                else
                {
                    objectives.Add(objective);
                }
                objective.GetComponent<Objective>().objectiveCompleted += handleObjectivePickup;
            }
        }
    }

    /// <summary>
    /// When a player has clearance codes, their primary objective becomes dropping these off at the FOB.
    /// This handles overriding any closest objective with a clearance code in the world scene.
    /// </summary>
    private bool OverrideObjectiveWhenPlayerHasClearanceCodes()
    {
        PlayerController playerController =  player.GetComponent<PlayerController>();
        if (playerController.HasClearanceCode())
        {
            GameObject fob = GameObject.FindGameObjectWithTag("FOB");
            if (!fob)
            {
                playerController.EnableFOB(true);
            }
            else
            {
                objectives = new List<GameObject>() { fob };
                SetClosestObjectiveAndDistance(objectives[0]);
            };
            return true;
        }
        return false;
    }
    /// <summary>
    /// Updates the non world UI with the objective information
    /// </summary>
    private void UpdateTopLeftUiText()
    {
        if (closestObjectiveDistance == Mathf.Infinity)
        {
            distanceText.text = "No objectives";
        }
        else
        {
            distanceValue = (int)(closestObjectiveDistance);
            if (HaveNullObjectives()) { return; }
            if (distanceText != null)
                distanceText.text = String.Format("{0} is your objective. It is {1} m away", closestObjective.GetComponent<Objective>().GetObjectiveName(), closestObjectiveDistance.ToString("N0"));
        }
    }

    private bool HaveNullObjectives()
    {
        if (closestObjective.gameObject == null)
        {
            distanceText.text = "No objectives";
            return true;
        }
        Objective objective = closestObjective.GetComponent<Objective>();
        if (objective.gameObject == null)
        {
            distanceText.text = "No objectives";
            return true;
        }
        return false;
    }

    /// <summary>
    /// Runs through the current objective list and sets the managers closest objective and distance
    /// </summary>
    private void FindClosestObjectiveAndSet()
    {
        HandleEmptyObjectives();
        if (OverrideObjectiveWhenPlayerHasClearanceCodes()) { return; };
        // Eager exit
        if (!PlayedMovedRecently()) { return; }
        foreach (GameObject objective in objectives)
        {
            // TODO - See if vector is ever negative
            SetClosestObjectiveAndDistance(objective);
        }
    }

    private void SetClosestObjectiveAndDistance(GameObject objective)
    {
        if (!objective) { return; }
        float distanceToObjective = Mathf.Abs(Vector3.Distance(player.transform.position, objective.transform.position));
        if (distanceToObjective < closestObjectiveDistance)
        {
            closestObjectiveDistance = distanceToObjective;
            closestObjective = objective;
        }
        else if (closestObjective == objective)
        {
            closestObjectiveDistance = distanceToObjective;
        }
    }

    /// <summary>
    /// Sets the PlayerUIManager objective with the closestobjective transform position
    /// </summary>
    private void SetPlayerUiObjective()
    {
        if (closestObjective != null)
        {
            FindObjectOfType<PlayerUIManager>().SetObjective(closestObjective.transform);
        }
    }
    /// <summary>
    /// Resets closest objective and closest objective distance
    /// </summary>
    private void HandleEmptyObjectives()
    {
        if (objectives.Count == 0)
        {
            closestObjective = null;
            closestObjectiveDistance = Mathf.Infinity;
        }
    }

    /// <summary>
    /// If the player hasn't moved, don't bother iterating objectives.
    /// </summary>
    private bool PlayedMovedRecently()
    {
        float oldX = playerLastPosition.x;
        float oldY = playerLastPosition.y;
        float oldZ = playerLastPosition.z;

        float newX = player.transform.position.x;
        float newY = playerLastPosition.y;
        float newZ = playerLastPosition.z;

        bool didNotMove = Mathf.Approximately(oldX, newX) &&
                        Mathf.Approximately(oldY, newY) &&
                        Mathf.Approximately(oldZ, newZ);
        return !didNotMove;

    }
    /// <summary>
    /// Adds an objective to the list of objectives if it does not already exist
    /// </summary>
    /// <param name="newObjective"></param>
    public void AddObjective(GameObject newObjective)
    {
        if (!objectives.Contains(newObjective))
        {
            objectives.Add(newObjective);
        }
    }
    /// <summary>
    /// Subscribes to event fired by objective. This  happens when the event considers itself "completed".
    /// </summary>
    /// <param name="objectiveThatGotPickedUp"></param>
    public void handleObjectivePickup(GameObject objectiveThatGotPickedUp)
    {
        objectives.Remove(objectiveThatGotPickedUp);
        closestObjectiveDistance = Mathf.Infinity;
    }
    /// <summary>
    /// Components can call this to ensure that there will be an objective list available for them to use
    /// </summary>
    public void GenerateObjectives()
    {
        if (objectives.Count == 0)
        {
            ResetObjectiveList();
        };
    }
}
