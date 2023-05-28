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
        public List<Node> Neightbours => neightbours;

        [SerializeField] private bool walkable;
        [SerializeField] private bool isTrap;
        [SerializeField] private List<Node> neightbours;

        public void Init(bool _walkable, bool _isTrap, string name)
        {
            walkable = _walkable;
            isTrap = _isTrap;
            MyTransform = transform;
            gameObject.name = name;
        }

        public void GetNeightbours(float maxDistnace = 2.2f)
        {
            for (int x = -1; x < 1; x++)
            {
                for (int y = -1; y < 1; y++)
                {
                    for (int z = -1; z < 1; z++)
                    {

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
            if (Physics.Raycast(transform.position, dir, out hit, maxDistnace, gameObject.layer))
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
            if (!walkable)
            {
                color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            }
            else if (isTrap)
            {
                color = new Color(1, 0, 0, 0.5f);
            }
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, sRadius);
            UnityEditor.Handles.Label(transform.position, Name);

            if (!walkable) return;
            if (neightbours == null) return;

            foreach (var n in neightbours)
            {
                if (n == null) continue;
                Gizmos.color = new Color(0, 0.5f, 0.5f, 0.5f);
                Gizmos.DrawLine(transform.position, n.transform.position);
                Gizmos.color = new Color(0, 1, 1, 0.5f);
                Gizmos.DrawSphere(n.transform.position, sRadius/2);
                UnityEditor.Handles.Label(n.transform.position, n.Name);
            }
        }
#endif
    }
}
