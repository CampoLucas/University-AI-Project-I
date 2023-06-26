using UnityEngine;

namespace Game.Data
{
    [System.Serializable]
    public class FieldOfViewData
    {
        public float Range => range;
        public float Angle => angle;
        public LayerMask Mask => mask;

        [SerializeField] private float range = 0.25f;
        [SerializeField] private float angle = 120;
        [SerializeField] private LayerMask mask;

        #if UNITY_EDITOR
        public void DebugGizmos(Transform origin, Color color)
        {
            var forward = origin.forward;
            var position = origin.position;
            
            Gizmos.color = color;
            var halfFOV = Angle / 2f;
            var leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

            
            var leftRayDirection = leftRayRotation * forward;
            var rightRayDirection = rightRayRotation * forward;

            
            Gizmos.DrawRay(position, leftRayDirection * Range);
            Gizmos.DrawRay(position, rightRayDirection * Range);

            UnityEditor.Handles.color = color - new Color(0, 0, 0, 0.9f);
            UnityEditor.Handles.DrawSolidArc(position, Vector3.up, leftRayDirection, Angle, Range);
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawWireArc(position, Vector3.up, leftRayDirection, Angle, Range);

        }
        #endif
    }
}