using System;
using Game.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Pathfinder : MonoBehaviour, IPathfinder
    {
        public List<Vector3> Waypoints { get; private set; } = new();
        public int NextPoint { get; private set; } = 0;
        
        [field: SerializeField] public Transform Target { get; private set; }

        [SerializeField] private float radius;
        [SerializeField] private LayerMask mask;
        [SerializeField] private LayerMask maskObs;
        [SerializeField] private float inViewRadius = 2f;

        private AStar<Node> _aStar = new();
        private Dictionary<Vector3[], List<Node>> _waypointsDictionary = new();
        private Node _startNode;
        private Node _endNode;

        public bool IsTargetInRange()
        {
            var endPos = _endNode.transform.position;
            Logging.LogError("Target is null", () => !Target);
            endPos.y = Target.transform.position.y;
            return Vector3.Distance(Target.position, endPos) <= radius;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        public void Run()
        {
            // is the start and end nodes are null, returns null
            if (!_startNode || !_endNode || !Target)
            {
                // No possible path found
                return;
            }
            
            // checks if the nodes are in the dictionary
            List<Node> nodePath;
            if (InView(_startNode, _endNode))
            {
                nodePath = new List<Node> { _startNode, _endNode };
                LoggingTwo.Log("Return path from: View", LoggingType.Debug);
            }
            else
            {
                nodePath = _aStar.Run(_startNode, Satisfies, GetConections, GetCost, Heuristic);
                LoggingTwo.Log("Return path from: A*", LoggingType.Debug);
            }

            // Convert the node list into a Vector3 list with its position at the start and the target position at the end
            var cleanedPath = new List<Vector3> { transform.position };

            for (var i = 0; i < nodePath.Count; i++)
            {
                cleanedPath.Add(nodePath[i].transform.position);
            }
            cleanedPath.Add(Target.position);

            // Cleans the path
            _aStar.CleanPath(cleanedPath, InView, out cleanedPath);
            
            Waypoints = cleanedPath;
        }

        private bool Satisfies(Node curr)
        {
            return curr == _endNode;
        }

        private List<Node> GetConections(Node curr)
        {
            return curr.Neightbourds;
        }

        private bool InView(Node from, Node to)
        {
            var startPos = from.transform.position;
            var endPos = to.transform.position;
            var dir = startPos - endPos;
            //return !Physics.Linecast(startPos, endPos, maskObs);
            return !Physics.SphereCast(startPos, inViewRadius, dir.normalized, out var hit, dir.magnitude, maskObs);
        }

        private bool InView(Vector3 from, Vector3 to)
        {
            return !Physics.Linecast(from, to, maskObs);
        }

        private float Heuristic(Node curr)
        {
            float multiplierDistance = 2;
            float multiplierTrap = 20f;
            float cost = 0;

            cost += Vector3.Distance(curr.transform.position, _endNode.transform.position) * multiplierDistance;

            if (curr.IsTrap)
                cost += multiplierTrap;
            if (_endNode.IsTrap)
                cost += multiplierTrap;
            
            
            return cost;
        }

        private float GetCost(Node parent, Node son)
        {
            float multiplierDistance = 1f;
            float multiplierTrap = 20f;
            //float multiplierUnwalkable = 80f;
            float cost = 0f;

            cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
            if (son.IsTrap)
                cost += multiplierTrap;
            //if (!son.Walkable)
            //    cost += multiplierUnwalkable;

            return cost;
        }

        private void SetWaypoints(List<Node> newPoints)
        {
            var list = new List<Vector3>();
            Logging.LogError("new points null", () => newPoints == null);
            Logging.LogError("new points is 0 (Node)", () => newPoints.Count == 0);
            for (var i = 0; i < newPoints.Count; i++)
            {
                list.Add(newPoints[i].transform.position);
            }
            SetWaypoints(list);
        }

        private void SetWaypoints(List<Vector3> newPoints)
        {
            NextPoint = 0;
            Logging.LogError("waypoints count is 0 (Vector3)", () => newPoints.Count == 0);
            if (newPoints.Count == 0) return;
            Waypoints = newPoints;
        }

        public void SetNextPoint() => NextPoint++;


        private void SetNode(Vector3 origin, ref Node node)
        {
            node = NodeGrid.GetInstance().GetClosestNode(origin);
        }

        public void SetNodes(Vector3 origin, Vector3 target)
        {
            SetNode(origin, ref _startNode);
            SetNode(target, ref _endNode);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, inViewRadius);
            
            if (_startNode != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_startNode.transform.position, 0.1f);
            }

            if (_endNode != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(_endNode.transform.position, 0.2f);
                Gizmos.DrawWireSphere(_endNode.transform.position, radius);
            }

            if (Waypoints != null && Waypoints.Count > 0)
            {
                if (NextPoint < Waypoints.Count)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawSphere(Waypoints[NextPoint], 0.5f);
                }

                for (var i = 0; i < Waypoints.Count - 1; i++)
                {
                    var point = Waypoints[i];
                    var nextPoint = Waypoints[i + 1];
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(point, nextPoint);
                }
            }
        }
    }
}
