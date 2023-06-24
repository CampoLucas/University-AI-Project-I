using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using Game.Entities;
using UnityEngine;

public class WizzardModel : EnemyModel
{
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        
        SetLightAttack(GetComponent<MagicAttack>());
    }
}
