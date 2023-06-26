using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.SO;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "SO/Entities/WizzardStats", order = 2)]

    public class WizzardSO : EnemySO
    {
        public FieldOfViewData AttackFov => attackFov;

        [Header("Attack Cone Vision")] [SerializeField]
        private FieldOfViewData attackFov;

    }
}
