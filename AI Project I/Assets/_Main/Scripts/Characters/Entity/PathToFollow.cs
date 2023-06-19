using System;
using System.Collections;
using System.Collections.Generic;
using Game.SO;
using UnityEngine;

namespace Game.Entities
{
    public class PathToFollow : MonoBehaviour
    {
        [field: SerializeField] public PathSO Path { get; private set; }
        private int _index;
        private int _dir = 1;

        public Vector3 GetCurrentPoint()
        {
            if (Path)
                return Path.Waypoints[_index] + Path.WorldOffset;
            return default;
        }

        public Vector3 GetNextWaypoint()
        {
            if (Path)
                return Path.Waypoints[GetNextIndex()] + Path.WorldOffset;
            return default;
        }

        private int GetNextIndex()
        {
            var currentIndex = _index;
            if (Path.IsCircular)
            {
                currentIndex = Path.IsReversed ? currentIndex - 1 : currentIndex + 1;
                
                if (currentIndex < 0)
                {
                    currentIndex = Path.Waypoints.Count - 1;
                }
                else if (currentIndex > Path.Waypoints.Count - 1)
                {
                    currentIndex = 0;
                }
                
            }
            else
            {
                if (_dir == 1)
                {
                    currentIndex++;
                    if (currentIndex  > Path.Waypoints.Count - 1)
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
                        currentIndex = Path.Waypoints.Count - 1;
                    }
                }
                
            }

            return currentIndex;
        }

        public bool ReachedWaypoint()
        {
            if (Path)
                return Vector3.Distance(transform.position, GetNextWaypoint()) < Path.Threshold;
            return default;
        }

        public Vector3 GetWaypointDirection()
        {
            return (GetNextWaypoint() - transform.position).normalized;
        }

        public void ChangeWaypoint()
        {
            _index = GetNextIndex();
        }
        
        private void OnDrawGizmos()
        {
            if (Path == null) return;

            for (int i = 0; i < Path.Waypoints.Count; i++)
            {
                var waypoint = Path.Waypoints[i];
                var waypointOffset = waypoint + Path.WorldOffset;
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
                
                if (waypoint == Path.Waypoints[_index])
                {
                    Gizmos.color = Color.magenta;
                }
                else if (waypoint == Path.Waypoints[GetNextIndex()])
                {
                    Gizmos.color = Color.red;
                }
                else if (waypoint == Path.Waypoints[0])
                {
                    Gizmos.color = Color.green;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }

                Gizmos.DrawSphere(waypointOffset, 0.2f);
                Gizmos.DrawWireSphere(waypointOffset, Path.Threshold);

                if (i > 0)
                {
                    Gizmos.DrawLine(Path.Waypoints[i - 1] + Path.WorldOffset, waypointOffset);
                }
            }

            if (Path.IsCircular && Path.Waypoints.Count > 1)
            {
                Gizmos.DrawLine(Path.Waypoints[Path.Waypoints.Count - 1] + Path.WorldOffset, Path.Waypoints[0] + Path.WorldOffset);
            }
        }
    }
}
