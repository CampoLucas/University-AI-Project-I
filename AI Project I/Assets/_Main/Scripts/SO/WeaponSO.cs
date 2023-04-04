using UnityEngine;
using Game.Entities.Weapons;

namespace Game.SO
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "SO/Items/Weapons", order = 1)]

    public class WeaponSO : ScriptableObject
    {
        //[field: SerializeField] public AnimationEventSO HandIdle { get; private set; }
        [field: SerializeField] public AnimationEventSO LightAttack01 { get; private set; }
        //[field: SerializeField] public AnimationEventSO LightAttack02 { get; private set; }
        [field: SerializeField] public AnimationEventSO HeavyAttack01 { get; private set; }

        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float HeavyAttackMultiplier { get; private set; }
    }

    [System.Serializable]
    public struct AnimationEvent
    {
        [SerializeField] private string name;
        [SerializeField] private float duration;
    }
}


