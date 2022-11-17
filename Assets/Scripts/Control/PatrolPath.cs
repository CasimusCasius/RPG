using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            const float WAYPOINT_GIZMO_RADIUS = 0.3f;
            for (int i = 0; i < transform.childCount ; i++)
            {
                int j = GetNextIndex(i, transform.childCount);

                Gizmos.DrawSphere(GetWaypoint(i), WAYPOINT_GIZMO_RADIUS);
                Gizmos.DrawLine(GetWaypoint(i), GetWaypoint(j));
            }
        }

        public int GetNextIndex(int i, int loopLenght)
        {
            if (i < loopLenght - 1)
                return i + 1;
            else
                return 0;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }

        
    }
}