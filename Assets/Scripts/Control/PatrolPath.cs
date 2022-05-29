using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameJam.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float gizmoWaypointRadius = 0.3f;
            //Note - you can access everything through transform as the transform manages the parenting hierarchy
            for (int i = 0; i < transform.childCount; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetWaypoint(i), gizmoWaypointRadius);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(GetNextIndex(i)));
            }
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }


        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        public Vector3 GetRandomWaypoint()
        {
            return transform.GetChild(Random.RandomRange(0, transform.childCount)).position;
        }
    }

}