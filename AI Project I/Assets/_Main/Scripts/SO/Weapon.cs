using UnityEngine;

namespace Game.Shared.SO
{
    [CreateAssetMenu(menuName = "Items/ Weapon Item")]

    public class Weapon : ScriptableObject
    {
        [field: SerializeField] public GameObject Prefab { get; private set; }

        [field: SerializeField] public WeaponAnimEvent HandIdle { get; private set; }
        [field: SerializeField] public WeaponAnimEvent LightAttack01 { get; private set; }
        [field: SerializeField] public WeaponAnimEvent LightAttack02 { get; private set; }
        [field: SerializeField] public WeaponAnimEvent HeavyAttack01 { get; private set; }

        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public float HeavyAttackMultiplier { get; private set; }
    }
}


