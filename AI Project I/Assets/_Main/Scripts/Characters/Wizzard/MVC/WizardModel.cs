using System.Collections;
using System.Collections.Generic;
using Game.Enemies;
using Game.Entities;
using Game.SO;
using UnityEngine;

public class WizardModel : EnemyModel
{

    private FieldOfView _attackFOV;
    
    protected override void Awake()
    {
        base.Awake();
        
        SetLightAttack(GetComponent<MagicAttack>());
        _attackFOV = new FieldOfView(GetData<WizzardSO>().AttackFov, transform);
    }

    public bool IsTargetInFront(Transform target) => _attackFOV.CheckRange(target) && _attackFOV.CheckAngle(target) &&
                                                     _attackFOV.CheckView(target);
    
    
    #if UNITY_EDITOR

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        
        #region FOV

        GetData<WizzardSO>().AttackFov.DebugGizmos(transform, Color.yellow);

        #endregion
        
    }

#endif
}
