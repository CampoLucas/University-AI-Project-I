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
        [SerializeField] private LayerMask nodeLayer;
        [SerializeField] private LayerMask maskObs;
        [SerializeField] private float inViewRadius = 2f;
        [SerializeField] private float closestNodeRange = 15f;
        [SerializeField] private bool hideGizmos;

        private AStar<Node> _aStar = new();
        private Dictionary<Vector3[], List<Node>> _waypointsDictionary = new();
        private Node _startNode;
        private Node _endNode;
        private NodeGrid _grid;

        private void Start()
        {
            _grid = NodeGrid.GetInstance();
        }

        public bool IsTargetInRange()
        {
            if (!_endNode || !Target) return false;
            
            var endPos = _endNode.transform.position;
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
            if (InView(transform.position, Target.position))
            {
                nodePath = new List<Node> { _startNode, _endNode };
                LoggingTwo.Log("Return path from: View", LoggingType.Debug);
            }
            else
            {
                nodePath = _aStar.Run(_startNode, Satisfies, GetConnections, GetCost, Heuristic);
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

        private List<Node> GetConnections(Node curr)
        {
            return curr.Neightbourds;
        }

        private bool InView(Node from, Node to)
        {
            return InView(from.transform.position, to.transform.position);
        }

        private bool InView(Vector3 from, Vector3 to)
        {
            var dir = from - to;
            var right = transform.right;
            var lineRight = !Physics.Linecast(from + right * inViewRadius, to);
            var lineMiddle = !Physics.Linecast(from, to, maskObs);
            var lineLeft = !Physics.Linecast(from + right * -inViewRadius, to);
            
            return lineLeft && lineRight && lineMiddle;
            //return !Physics.SphereCast(from, inViewRadius, dir.normalized, out var hit, dir.magnitude, maskObs);
        }

        private float Heuristic(Node curr)
        {
            const float multiplierDistance = 2f;
            const float multiplierTrap = 20f;
            var cost = 0f;

            cost += Vector3.Distance(curr.transform.position, _endNode.transform.position) * multiplierDistance;

            if (curr.IsTrap)
                cost *= multiplierTrap;
            if (_endNode.IsTrap)
                cost *= multiplierTrap;
            
            
            return cost;
        }

        private float GetCost(Node parent, Node son)
        {
            const float multiplierDistance = 1f;
            const float multiplierTrap = 20f;
            var cost = 0f;

            cost += Vector3.Distance(parent.transform.position, son.transform.position) * multiplierDistance;
            if (son.IsTrap)
                cost *= multiplierTrap;

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


        private bool SetNode(Vector3 origin, ref Node node)
        {
            return _grid && _grid.GetClosestNode(origin, closestNodeRange, out node, nodeLayer); // 11 = Node layer
        }

        public bool SetNodes(Vector3 origin, Vector3 target)
        {
            if (SetNode(target, ref _endNode))
                SetNode(origin, ref _startNode);
            else
                return false;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
#if UNITY_EDITOR
            if (hideGizmos) return;
            
            UnityEditor.Handles.color = new Color(0, 1, 0, 0.1f);
            var transform1 = transform;
            var position = transform1.position;
            var forward = transform1.forward;
            var up = transform1.up;
            UnityEditor.Handles.DrawSolidArc(position, up, forward, 360, inViewRadius);
            UnityEditor.Handles.color = Color.green;
            UnityEditor.Handles.DrawWireArc(position, up, forward, 360, inViewRadius);
            
            
            Gizmos.color = new Color(0.3f, 0.1f, 1, 1);
            Gizmos.DrawWireSphere(position, closestNodeRange);
            
            if (Target)
                Gizmos.DrawWireSphere(Target.transform.position, closestNodeRange);
            
            
#endif
        }

        private void OnDrawGizmos()
        {
            if (hideGizmos) return;

            if (_startNode != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(_startNode.transform.position, 0.1f);
            }

            if (_endNode != null)
            {
                Gizmos.color = Color.green;
                var position = _endNode.transform.position;
                Gizmos.DrawSphere(position, 0.2f);
                Gizmos.DrawWireSphere(position, radius);
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
