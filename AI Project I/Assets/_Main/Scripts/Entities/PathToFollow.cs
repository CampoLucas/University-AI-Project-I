using System;
using System.Collections;
using System.Collections.Generic;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class PathToFollow : MonoBehaviour
    {
        [SerializeField] private PathSO path;
        private int _index;
        private int _dir = 1;

        public Vector3 GetCurrentPoint()
        {
            return path.Waypoints[_index] + path.WorldOffset;
        }

        public Vector3 GetNextWaypoint()
        {
            return path.Waypoints[GetNextIndex()] + path.WorldOffset;
        }

        private int GetNextIndex()
        {
            var currentIndex = _index;
            if (path.IsCircular)
            {
                currentIndex = path.IsReversed ? currentIndex - 1 : currentIndex + 1;
                
                if (currentIndex < 0)
                {
                    currentIndex = path.Waypoints.Count - 1;
                }
                else if (currentIndex > path.Waypoints.Count - 1)
                {
                    currentIndex = 0;
                }
                
            }
            else
            {
                if (_dir == 1)
                {
                    currentIndex++;
                    if (currentIndex  > path.Waypoints.Count - 1)
                    {
                        _dir = -1;
                        currentIndex = 0;
                    }
                }
                else
                {
                    currentIndex--;
                    if (currentIndex < 0)
                    {
                        _dir = 1;
                        currentIndex = path.Waypoints.Count - 1;
                    }
                }
                
            }

            return currentIndex;
        }

        public bool ReachedWaypoint()
        {
            return Vector3.Distance(transform.position, GetNextWaypoint()) < path.Threshold;
        }

        public Vector3 GetWaypointDirection()
        {
            return (GetNextWaypoint() - transform.position).normalized;
        }

        public void ChangeWaypoint()
        {
            _index = GetNextIndex();
        }

        // private void OnDrawGizmos()
        // {
        //     // Draw the path in the editor
        //     if (path != null)
        //     {
        //         
        //         for (int i = 0; i < path.Waypoints.Count; i++)
        //         {
        //             if (path.Waypoints[i] == path.Waypoints[_index])
        //             {
        //                 Gizmos.color = Color.magenta;
        //             }
        //             else if (path.Waypoints[i] == path.Waypoints[GetNextIndex()])
        //             {
        //                 Gizmos.color = Color.red;
        //             }
        //             else if (path.Waypoints[i] == path.Waypoints[0])
        //             {
        //                 Gizmos.color = Color.green;
        //             }
        //             else
        //             {
        //                 Gizmos.color = Color.yellow;
        //             }
        //             Gizmos.DrawSphere(path.Waypoints[i] + path.WorldOffset, 0.2f);
        //             Gizmos.DrawWireSphere(path.Waypoints[i] + path.WorldOffset, path.Threshold);
        //             Gizmos.color = Color.yellow;
        //             if (i > 0)
        //             {
        //                 Gizmos.DrawLine(path.Waypoints[i - 1] + path.WorldOffset, path.Waypoints[i] + path.WorldOffset);
        //             }
        //         }
        //         if (path.IsCircular && path.Waypoints.Count > 1)
        //         {
        //             Gizmos.DrawLine(path.Waypoints[path.Waypoints.Count - 1] + path.WorldOffset, path.Waypoints[0] + path.WorldOffset);
        //         }
        //     }
        // }
        
        private void OnDrawGizmos()
        {
            if (path == null) return;

            for (int i = 0; i < path.Waypoints.Count; i++)
            {
                var waypoint = path.Waypoints[i];
                var waypointOffset = waypoint + path.WorldOffset;
                if (waypointOffset.y != 0)
                {
                    if (waypointOffset.y > 0)
                    {
                        Gizmos.color = new Color(0, 0, 1, 0.5f);
                    }
                    else
                    {
                        Gizmos.color = new Color(1, 0.5f, 0, 0.5f);
                    }
                    Gizmos.DrawLine(new Vector3(waypointOffset.x, 0, waypointOffset.z), waypointOffset);
                }
                
                if (waypoint == path.Waypoints[_index])
                {
                    Gizmos.color = Color.magenta;
                }
                else if (waypoint == path.Waypoints[GetNextIndex()])
                {
                    Gizmos.color = Color.red;
                }
                else if (waypoint == path.Waypoints[0])
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                Gizmos.DrawSphere(waypointOffset, 0.2f);
                Gizmos.DrawWireSphere(waypointOffset, path.Threshold);

                if (i > 0)
                {
                    Gizmos.DrawLine(path.Waypoints[i - 1] + path.WorldOffset, waypointOffset);
                }
            }

            if (path.IsCircular && path.Waypoints.Count > 1)
            {
                Gizmos.DrawLine(path.Waypoints[path.Waypoints.Count - 1] + path.WorldOffset, path.Waypoints[0] + path.WorldOffset);
            }
        }
    }
}
