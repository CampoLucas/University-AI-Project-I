using UnityEngine;
using Game.Entities.Weapons;

namespace Game.SO
{
    [CreateAssetMenu(menuName = "Items/ Weapon Item")]

    public class WeaponSO : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public string HandIdle { get; private set; }
        [field: SerializeField] public string LightAttack01 { get; private set; }
        [field: SerializeField] public string LightAttack02 { get; private set; }
        [field: SerializeField] public string HeavyAttack01 { get; private set; }

        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float HeavyAttackMultiplier { get; private set; }
    }
}


