using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Pathfinding
{
    public class NodeGrid : MonoBehaviour
    {
        public float NodeDiameter => nodeRadius * 2;
        public Vector3Int GridSize => new Vector3Int(Mathf.RoundToInt(gridWorldSize.x / NodeDiameter), Mathf.RoundToInt(gridWorldSize.y / NodeDiameter), Mathf.RoundToInt(gridWorldSize.z / NodeDiameter));
        
        [SerializeField] private Vector3 gridWorldSize;
        [SerializeField] private float nodeRadius;
        [SerializeField] private LayerMask floorMask;
        [SerializeField] private LayerMask obsMask;
        [SerializeField] private LayerMask trapMask;
        [SerializeField] private Node prefab;
        [SerializeField] private List<Node> nodes;
        [SerializeField] private bool previewNodes;
        
        private static NodeGrid _instance;
        private static readonly object _lock = new();
        
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
                        if (Condition(position, out var walkable, out var hasTrap))
                        {
                            var n = Instantiate(prefab, position, Quaternion.identity, transform);
                            n.transform.localScale = Vector3.one * NodeDiameter;
                            n.Init(walkable, hasTrap, $"{x}, {y}, {z}");
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
            Node closestNode = null;
            var closestDistance = Mathf.Infinity;
            var posOffset = position;
            posOffset.y += 0.05f;

            foreach (var node in nodes)
            {
                //var distance = Vector3.Distance(node.transform.position, position);
                var distance = (node.transform.position - position).sqrMagnitude;
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestNode = node;
                }
            }

            return closestNode;
        }

        public void SearchNeightbourds()
        {
            foreach (var node in nodes)
            {
                node.RemoveNeightbours();
                node.GetNeightbours(NodeDiameter);
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

        private bool Condition(Vector3 pos, out bool walkable, out bool hasTrap)
        {
            walkable = !(Physics.OverlapSphere(pos, nodeRadius, obsMask).Length > 0);
            hasTrap = Physics.OverlapSphere(pos, nodeRadius, trapMask).Length > 0;

            RaycastHit hit;
            if (Physics.SphereCast(pos, nodeRadius, Vector3.down, out hit, nodeRadius, floorMask))
            {
                return true;
            }

            return false;

        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, gridWorldSize);

            if (previewNodes)
            {
                for (int x = 0; x < GridSize.x; x++)
                {
                    for (int y = 0; y < GridSize.y; y++)
                    {
                        for (int z = 0; z < GridSize.z; z++)
                        {
                            var position = GetWorldPosition(x, y, z);
                            if (Condition(position, out var walkable, out var hasTrap))
                            {
                                var color = new Color(0, 0, 1, 0.5f);
                                if (!walkable)
                                    color = new Color(.2f, .2f, .2f, 0.5f);
                                else if (hasTrap)
                                    color = new Color(1, 0, 0, 0.5f);
                                Gizmos.color = color;
                                Gizmos.DrawSphere(position, nodeRadius/2);
                            }
                        }
                    }
                }
            }            
            else if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    if (node.Neightbours != null)
                    {
                        var color = new Color(0, 0, 1, 0.5f);
                        if (!node.Walkable)
                            color = new Color(.2f, .2f, .2f, 0.5f);
                        else if (node.IsTrap)
                            color = new Color(1, 0, 0, 0.5f);
                        Gizmos.color = color;
                        Gizmos.DrawSphere(node.MyTransform.position, nodeRadius/2);
                        UnityEditor.Handles.Label(node.MyTransform.position, node.Name);
                        foreach (var neightbour in node.Neightbours)
                        {
                            Gizmos.color = new Color(0.5f, 0, 0.5f, 1);
                            Gizmos.DrawLine(node.MyTransform.position, neightbour.MyTransform.position);
                        }
                    }
                }
            }

        }
#endif
    }
}
