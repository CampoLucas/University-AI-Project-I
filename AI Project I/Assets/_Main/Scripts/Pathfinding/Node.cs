using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Pathfinding
{
    public class Node : MonoBehaviour
    {
        public Transform MyTransform { get; private set; }
        public string Name => gameObject.name;
        public List<Node> Neightbourds => neightbours;
        public bool IsTrap => isTrap;

        [SerializeField] private bool isTrap;
        [SerializeField] private List<Node> neightbours;
        [SerializeField] private LayerMask mask;

        public void Init(bool _isTrap, string name)
        {
            isTrap = _isTrap;
            MyTransform = transform;
            gameObject.name = name;
            mask = gameObject.layer;
        }

        public void GetNeightbours(float maxDistnace = 2.2f)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == z || x == -z) continue;
                        var dir = new Vector3(x, y, z);
                        GetNeightbours(dir, maxDistnace);
                    }
                }
            }
        }

        public void RemoveNeightbours()
        {
            neightbours.Clear();
        }

        private void GetNeightbours(Vector3 dir, float maxDistnace = 2.2f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, dir, out hit, maxDistnace))
            {
                var node = hit.collider.GetComponent<Node>();
                if (node != null && node != this)
                    neightbours.Add(node);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!UnityEditor.Selection.gameObjects.Contains(transform.gameObject)) { return; }
            
            var sRadius = 0.2f;
            var color = new Color(0, 0, 1, 0.5f);
            if (isTrap)
            {
                color = new Color(1, 0, 0, 0.5f);
            }
            Gizmos.color = color;
            var position1 = transform.position;
            Gizmos.DrawSphere(position1, sRadius);
            UnityEditor.Handles.Label(position1, Name);

            if (neightbours == null) return;

            foreach (var n in neightbours)
            {
                if (n == null) continue;
                Gizmos.color = new Color(0, 0.5f, 0.5f, 0.5f);
                var position = n.transform.position;
                Gizmos.DrawLine(transform.position, position);
                Gizmos.color = new Color(0, 1, 1, 0.5f);
                Gizmos.DrawSphere(position, sRadius/2);
                UnityEditor.Handles.Label(position, n.Name);
            }
        }
#endif
    }
}
