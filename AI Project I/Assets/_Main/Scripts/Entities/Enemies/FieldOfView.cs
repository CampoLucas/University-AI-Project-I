using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Entities.Enemies
{
    public class FieldOfView : MonoBehaviour
    {
        // public float Range => range;
        // public float Angle => angle;
        [Header("Cone vision")]
        [SerializeField] private float range = 5;
        [SerializeField] private float angle = 120;
        [SerializeField] private LayerMask mask;
        
        public bool CheckRange(Transform target)
        {
            var distance = Vector3.Distance(transform.position, target.position);
            return distance < range;
        }

        public bool CheckAngle(Transform target)
        {
            var forward = transform.forward;
            var dirToTarget = target.position - transform.position;
            var angleToTarget = Vector3.Angle(forward, dirToTarget);
            return angle / 2 > angleToTarget;
        }

        public bool CheckView(Transform target)
        {
            var diff = target.position - transform.position;
            var dirToTarget = diff.normalized;
            var distanceToTarget = diff.magnitude;

            return !Physics.Raycast(transform.position, dirToTarget, out var hit, distanceToTarget, mask);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            var halfFOV = angle / 2f;
            var leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
            var rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            var leftRayDirection = leftRayRotation * forward;
            var rightRayDirection = rightRayRotation * forward;

            
            Gizmos.DrawRay(position, leftRayDirection * range);
            Gizmos.DrawRay(position, rightRayDirection * range);

            Handles.color = new Color(1f, 0f, 0f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.up, leftRayDirection, angle, range);
            Handles.color = Color.red;
            Handles.DrawWireArc(position, Vector3.up, leftRayDirection, angle, range);
        }
    }
    
    // #if UNITY_EDITOR
    // [CustomEditor(typeof(FieldOfView))]
    // public class FieldOfViewEditor : Editor
    // {
    //     private void OnSceneGUI()
    //     {
    //         var fov = target as FieldOfView;
    //         if (fov)
    //         {
    //             var transform = fov.transform;
    //             var position = transform.position;
    //             Handles.color = new Color(1, 0, 0, 0.2f);
    //             //Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
    //             Handles.DrawSolidArc(position, Vector3.up, transform.forward, fov.Angle / 2, fov.Range);
    //             Handles.DrawSolidArc(position, Vector3.up, transform.forward, -fov.Angle / 2, fov.Range);
    //             // Handles.color = Color.red;
    //             // Handles.DrawWireArc(position, Vector3.up, transform.forward, fov.Angle / 2, fov.Range);
    //             // Handles.DrawWireArc(position, Vector3.up, transform.forward, -fov.Angle / 2, fov.Range);
    //             //
    //             // var eulerAngles = fov.transform.eulerAngles;
    //             // var viewAngle01 = DirectionFromAngle(eulerAngles.y, -fov.Angle / 2);
    //             // var viewAngle02 = DirectionFromAngle(eulerAngles.y, fov.Angle / 2);
    //             //
    //             // Handles.DrawLine(position, position + viewAngle01 * fov.Range);
    //             // Handles.DrawLine(position, position + viewAngle02 * fov.Range);
    //         }
    //     }
    //     // private Vector3 DirectionFromAngle(float eulerY, float angleInDegrees)
    //     // {
    //     //     angleInDegrees += eulerY;
    //     //
    //     //     return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    //     // }
    // }    
    // #endif
}