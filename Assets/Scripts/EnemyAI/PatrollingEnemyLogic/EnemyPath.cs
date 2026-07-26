using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace EnemyAI
{
    public class EnemyPath : MonoBehaviour
    {
        public List<Transform> waypoints = new();
        public int closestwaypoint;
        [Header("Debug")]
        public bool drawPath;
        public bool showNumbers;
        public bool drawAsLoop;
        [Space(.2f)]
        public Color debugColor = Color.white;

        public int GetClosestWaypoint()
        {
            int newWaypoint = 0;
            float closestDistance = Vector3.Distance(waypoints[newWaypoint].position, transform.position);

            for (int i = 0; i < waypoints.Count; i++)
            {
                float checkDistance = Vector3.Distance(waypoints[i].position, transform.position);
                if (checkDistance < closestDistance)
                {
                    newWaypoint = i;
                    closestDistance = checkDistance;
                }      
            }
            return newWaypoint;
        }

# if UNITY_EDITOR
        private void DrawPath()
        {
            for (int i = 0; i < waypoints.Count; i++)
            {
                GUIStyle labelStyle = new();
                labelStyle.fontSize = 15;
                labelStyle.normal.textColor = debugColor;
                if (showNumbers)
                    Handles.Label(waypoints[i].position, i.ToString(), labelStyle);

                // Draw lines between paths
                if (i > 0)
                {
                    Gizmos.color = debugColor;
                    Gizmos.DrawLine(waypoints[i - 1].position, waypoints[i].position);
                }

                if (drawAsLoop)
                    Gizmos.DrawLine(waypoints[^1].position, waypoints[0].position);
            }
        }

        private void OnDrawGizmos()
        {
            if (drawPath)
                DrawPath();
        }
#endif
    }
}
