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

        private AStar<Node> _aStar = new();
        private Dictionary<Node[], List<Node>> _waypointsDictionary = new();
        private Node _startNode;
        private Node _endNode;

        public bool IsTargetInRange()
        {
            var endPos = _endNode.transform.position;
            Logging.LogError("Target is null", () => !Target);
            endPos.y = Target.transform.position.y;
            return Vector3.Distance(Target.position, _endNode.transform.position) < radius;
        }

        public void SetTarget(Transform target)
        {
            Target = target;
        }

        public void Run()
        {
            if (!_startNode || !_endNode) return;

            var startEnd = new Node[] { _startNode, _endNode };
            if (_waypointsDictionary.TryGetValue(startEnd, out var path))
            {
                Logging.Log("Path from dictionary");
                SetWaypoints(path);
                return;
            }

            var startPos = _startNode.transform.position;
            var endPos = _endNode.transform.position;
            var endDir = endPos - startPos;

            //if (InView(_startNode, _endNode))
            //{
            //    path = new() { _startNode, _endNode };
            //    Logging.Log("Path from sphere cast");
            //}
            //else
            //{
            //    path = _aStar.Run(_startNode, Satisfies, GetConections, GetCost, Heuristic, 500);
            //    Logging.Log("Path from a star");
            //}

            path = _aStar.Run(_startNode, Satisfies, GetConections, GetCost, Heuristic, 500);
            Logging.LogError("Path is 0", () => path.Count == 0);
            Logging.Log("Path from a star");

            SetWaypoints(path);
            _waypointsDictionary[startEnd] = path;
            Logging.Log(startEnd);
        }

        //public IEnumerator<Vector3> Run()
        //{
        //    if (!_startNode || !_endNode) yield break; 

        //    var startEnd = new Path(_startNode, _endNode);
        //    if (_waypointsDictionary.TryGetValue(startEnd, out var path))
        //    {
        //        SetWaypoints(path);
        //        yield break; 
        //    }

        //    var startPos = _startNode.transform.position;
        //    var endPos = _endNode.transform.position;
        //    var endDir = endPos - startPos;

        //    if (Physics.SphereCast(startPos, 1.5f, endDir, out var hit, endDir.magnitude, maskObs))
        //    {
        //        path = new() { startPos, endPos };
        //    }
        //    else
        //    {
        //        var aStarPath = new List<Node>(_aStar.Run(_startNode, Satisfies, GetConections, GetCost, Heuristic, 500));

        //        for (var i = 0; i < aStarPath.Count; i++)
        //        {
        //            path.Add(aStarPath[i].transform.position);
        //            yield return aStarPath[i].transform.position;
        //        }


        //    }

        //    SetWaypoints(path);
        //    _waypointsDictionary[startEnd] = path;
        //}

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
            return !Physics.Linecast(startPos, endPos, maskObs);
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

    public class Path
    {
        public Node Start;
        public Node End;

        public Path(Node start, Node end)
        {
            Start = start;
            End = end;
        }

        public override string ToString()
        {
            return "(" + Start.Name + " + " + End.Name + ")";
        }
    }
}
