using System.Collections.Generic;
using UnityEngine;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Path", menuName = "SO/Path", order = 3)]
    public class PathSO : ScriptableObject
    {
        [field: SerializeField] public bool IsCircular { get; private set; }
        [field: SerializeField] public bool IsReversed { get; private set; }
        [field: SerializeField] public float Threshold { get; private set; }
        [field: SerializeField] public Vector3 WorldOffset { get; private set; }
        [field: SerializeField] public List<Vector3> Waypoints { get; private set; }
    }
}