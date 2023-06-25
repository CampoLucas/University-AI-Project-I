using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class NodeGrid : MonoBehaviour
    {
        public float NodeDiameter => nodeRadius * 2;
        public Vector3Int GridSize => new Vector3Int(Mathf.RoundToInt(gridWorldSize.x / NodeDiameter), Mathf.RoundToInt(gridWorldSize.y / NodeDiameter), Mathf.RoundToInt(gridWorldSize.z / NodeDiameter));
        
        
        [Header("Position Settings")]
        [SerializeField] private Vector3 gridWorldSize;
        [SerializeField] private Vector3 gridOffset;
        [SerializeField] private bool clampToFloor;
        [SerializeField] private float clampOffset;
        
        [Header("Masks Settings")]
        [SerializeField] private LayerMask floorMask;
        [SerializeField] private LayerMask obsMask;
        [SerializeField] private LayerMask trapMask;
        
        [Header("Node Settings")]
        [SerializeField] private Node prefab;
        [SerializeField] private float nodeRadius;
        [SerializeField] private List<Node> nodes;
        
        private Collider[] _closestNodes;
        private static NodeGrid _instance;
        private static readonly object _lock = new();
        
#if UNITY_EDITOR
        public bool HideGizmos => hideGizmos;
        public int NodeCount => nodes != null ? nodes.Count : 0;
        [Header("Gizmos Settings")]
        [SerializeField] private bool hideGizmos;
        [SerializeField] private bool hideNodes;
        [SerializeField] private bool hideLabels;
        [SerializeField] private bool hideNeighbours;
        [SerializeField] private bool onSelected = true;
        [SerializeField] private bool previewNodes;
#endif
        
        private void Awake()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = this;
                    }
                }
            }
            
            _closestNodes = new Collider[15];
        }

        public static NodeGrid GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        var instance = FindObjectOfType<NodeGrid>();
                        if (instance == null)
                        {
                            var gameObject = new GameObject("NodeGrid");
                            instance = gameObject.AddComponent<NodeGrid>();
                        }
                        _instance = instance;
                    }
                }
            }
            return _instance;
        }

        public void CreateGrid()
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    for (int z = 0; z < GridSize.z; z++)
                    {
                        var position = GetWorldPosition(x, y, z);
                        if (Condition(position, out var trap))
                        {
                            if (clampToFloor && Physics.Raycast(position, Vector3.down, out var hit, NodeDiameter * 2, floorMask))
                            {
                                var pos = hit.point;
                                pos.y += nodeRadius + clampOffset;
                                position = pos;
                            }
                            
                            
                            var n = Instantiate(prefab, position, Quaternion.identity, transform);
                            n.transform.localScale = Vector3.one * NodeDiameter;
                            n.Init(trap, $"{x}, {y}, {z}");
                            nodes.Add(n);
                        }
                    }
                }
            }
        }

        public void EmptyGrid()
        {
            for (var i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                DestroyImmediate(node.gameObject);
            }
            nodes.Clear();
        }

        public Node GetClosestNode(Vector3 position)
        {
            LoggingTwo.Log("There is a better method that gets the closest nodes from a group of nodes in the given position", LoggingType.Warning);
            const float multiplierTrap = 5f;

            Node closestNode = null;
            var closestDistance = Mathf.Infinity;

            var posOffset = position;
            posOffset.y += 0.05f;

            foreach (var node in nodes)
            {
                //var distance = Vector3.Distance(node.transform.position, position);
                var distance = (node.transform.position - position).sqrMagnitude;
                if (node.IsTrap)
                    distance += multiplierTrap;

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }
        
        public bool GetClosestNode(Vector3 position, float range, out Node closestNode, LayerMask layer, float yOffset = .05f)
        {
            const float multiplierTrap = 5f;

            closestNode = null;
            var overlaps = Physics.OverlapSphereNonAlloc(position, range, _closestNodes, layer);

            if (overlaps <= 0) return false;
            var closestDistance = Mathf.Infinity;
            var pos = position;
            pos.y = yOffset;

            
            for (var i = 0; i < overlaps; i++)
            {
                var node = _closestNodes[i].gameObject.GetComponent<Node>();
                
                var distance = Vector3.Distance(node.transform.position, pos);
                
                if (node.IsTrap) distance += multiplierTrap;
                
                if (!(distance < closestDistance)) continue;
                closestDistance = distance;
                closestNode = node;
            }

            return true;
        }

        public void SearchNeightbourds()
        {
            foreach (var node in nodes)
            {
                node.RemoveNeightbours();
                node.GetNeightbours(NodeDiameter * 2);
            }
        }

        private Vector3 GetWorldPosition(float x, float y, float z)
        {
            float xOffset = (GridSize.x - 1) * NodeDiameter * 0.5f;
            float yOffset = (GridSize.y - 1) * NodeDiameter * 0.5f;
            float zOffset = (GridSize.z - 1) * NodeDiameter * 0.5f;

            var position = new Vector3(
                x * NodeDiameter - xOffset,
                y * NodeDiameter - yOffset,
                z * NodeDiameter - zOffset
            );
            position += transform.position;
            return position;
        }

        // private bool Condition(Vector3 pos, out bool walkable, out bool hasTrap)
        // {
        //     walkable = !(Physics.OverlapSphere(pos, nodeRadius, obsMask).Length > 0);
        //     hasTrap = Physics.OverlapSphere(pos, nodeRadius, trapMask).Length > 0;
        //
        //     RaycastHit hit;
        //     if (Physics.SphereCast(pos, nodeRadius, Vector3.down, out hit, nodeRadius, floorMask))
        //     {
        //         return true;
        //     }
        //
        //     return false;
        //
        // }
        
        private bool Condition(Vector3 pos, out bool hasTrap)
        {
            //hasTrap = Physics.OverlapSphere(pos, nodeRadius, trapMask).Length > 0;
            hasTrap = Physics.OverlapBox(pos, Vector3.one * nodeRadius, Quaternion.identity, trapMask).Length > 0;

            var e = new Collider[1];
            RaycastHit[] hits = new RaycastHit[1];

            RaycastHit hit;

            var nRadius = nodeRadius / 2;
            var forwardPos = pos + Vector3.forward * nRadius;
            var backwardPos = pos + Vector3.back * nRadius;
            var rightPos = pos + Vector3.right * nRadius;
            var leftPos = pos + Vector3.left * nRadius;
            var maxDistance = NodeDiameter * 2;
            var forward = Physics.Raycast(forwardPos, Vector3.down, maxDistance, floorMask);
            var backward = Physics.Raycast(backwardPos, Vector3.down, maxDistance, floorMask);
            var right = Physics.Raycast(rightPos, Vector3.down, maxDistance, floorMask);
            var left = Physics.Raycast(leftPos, Vector3.down, maxDistance, floorMask);


            var insideFloor = Physics.OverlapSphereNonAlloc(pos, nodeRadius, e, floorMask) > 0;
            var insideUnWalkable = Physics.OverlapSphereNonAlloc(pos, nodeRadius, e, obsMask) > 0;
            if (!forward || !backward || !right || !left || insideFloor || insideUnWalkable) return false;
            var overlapUp = Physics.SphereCastNonAlloc(pos, nodeRadius, Vector3.up, hits, NodeDiameter, floorMask) > 0;
            if (overlapUp) return false;
            var overlapDown = Physics.SphereCastNonAlloc(pos, nodeRadius, Vector3.down, hits, NodeDiameter, floorMask) > 0;
            return overlapDown;
        }
        
        private bool Condition(Vector3 pos)
        {
            var e = new Collider[1];
            RaycastHit[] hits = new RaycastHit[1];

            RaycastHit hit;

            var nRadius = nodeRadius / 2;
            var forwardPos = pos + Vector3.forward * nRadius;
            var backwardPos = pos + Vector3.back * nRadius;
            var rightPos = pos + Vector3.right * nRadius;
            var leftPos = pos + Vector3.left * nRadius;
            var maxDistance = NodeDiameter * 2;
            var forward = Physics.Raycast(forwardPos, Vector3.down, maxDistance, floorMask);
            var backward = Physics.Raycast(backwardPos, Vector3.down, maxDistance, floorMask);
            var right = Physics.Raycast(rightPos, Vector3.down, maxDistance, floorMask);
            var left = Physics.Raycast(leftPos, Vector3.down, maxDistance, floorMask);


            var insideFloor = Physics.OverlapSphereNonAlloc(pos, nodeRadius, e, floorMask) > 0;
            var insideUnwalkable = Physics.OverlapSphereNonAlloc(pos, nodeRadius, e, obsMask) > 0;
            if (forward && backward && right && left && !insideFloor && !insideUnwalkable)
            {
                var overlapUp = Physics.SphereCastNonAlloc(pos, nodeRadius, Vector3.up, hits, NodeDiameter, floorMask) > 0;
                if (!overlapUp)
                {
                    var overlapDown = Physics.SphereCastNonAlloc(pos, nodeRadius, Vector3.down, hits, NodeDiameter, floorMask) > 0;
                    if (overlapDown)
                    {
                        return true;
                    }
                }
            }

            return false;

        }

#if UNITY_EDITOR
        private void DebugGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, gridWorldSize);

            if (previewNodes)
            {
                for (var x = 0; x < GridSize.x; x++)
                {
                    for (var y = 0; y < GridSize.y; y++)
                    {
                        for (var z = 0; z < GridSize.z; z++)
                        {
                            var position = GetWorldPosition(x, y, z);
                            if (!Condition(position, out var hasTrap)) continue;
                            var color = new Color(0, 0, 1, 0.5f);
                            if (hasTrap)
                                color = new Color(1, 0, 0, 0.5f);
                                
                            if (clampToFloor && Physics.Raycast(position, Vector3.down, out var hit, NodeDiameter * 2, floorMask))
                            {
                                var pos = hit.point;
                                pos.y += nodeRadius + clampOffset;
                                position = pos;
                            }
                                
                            Gizmos.color = color;
                            Gizmos.DrawSphere(position, nodeRadius/2);
                        }
                    }
                }
            }
            else if (nodes != null)
            {
                foreach (var node in nodes)
                {

                    var color = new Color(0, 0, 1, 0.5f);
                    if (node.IsTrap)
                        color = new Color(1, 0, 0, 0.5f);
                    Gizmos.color = color;
                    var position = node.transform.position;
                    
                    if (!hideNodes) Gizmos.DrawSphere(position, nodeRadius/2);

                    if (!hideLabels)
                    {
                        var style = new GUIStyle(GUI.skin.label)
                            { alignment = TextAnchor.MiddleCenter, fontSize = (int)(20 * (nodeRadius * 0.5f)) };
                        style.normal.textColor = Color.white - color;
                        UnityEditor.Handles.Label(position, node.Name, style);
                    }
                    
                    if (node.Neightbourds == null || hideNeighbours) continue;
                    foreach (var neighbour in node.Neightbourds)
                    {
                        Gizmos.color = new Color(0.5f, 0, 0.5f, 1);
                        Gizmos.DrawLine(node.transform.position, neighbour.transform.position);
                    }
                }
            }
        }

        private void OnDrawGizmos()
        {
            if (hideGizmos || onSelected) return;
            DebugGizmos();
        }

        private void OnDrawGizmosSelected()
        {
            if (hideGizmos || !onSelected) return;
            DebugGizmos();
        }
#endif
    }
}
