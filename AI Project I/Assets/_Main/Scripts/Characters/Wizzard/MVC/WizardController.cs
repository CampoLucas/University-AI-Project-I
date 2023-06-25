using System.Collections;
using System.Collections.Generic;
using Game;
using Game.DecisionTree;
using Game.Enemies;
using Game.Enemies.States;
using Game.Entities.Steering;
using Game.FSM;
using Game.Interfaces;
using UnityEngine;

public class WizardController : EnemyController
{
    private ISteering _flee;

    protected override void InitSteering()
    {
        base.InitSteering();
        _flee = new Flee(transform, Player.transform);
    }

    protected override void InitFSM()
    {
        Fsm = new FSM<EnemyStatesEnum>();
        
        var states = new List<EnemyStateBase<EnemyStatesEnum>>();

        var idle = new EnemyStateIdle<EnemyStatesEnum>();
        var seek = new EnemyStateSeek<EnemyStatesEnum>(Seek, ObsAvoidance);
        var pursuit = new EnemyStatePursuit<EnemyStatesEnum>(Pursuit, ObsAvoidance);
        var flee = new EnemyStatePursuit<EnemyStatesEnum>(_flee, ObsAvoidance);
        var damage = new EnemyStateDamage<EnemyStatesEnum>();
        var lightAttack = new EnemyStateLightAttack<EnemyStatesEnum>();
        var heavyAttack = new EnemyStateHeavyAttack<EnemyStatesEnum>();
        var dead = new EnemyStateDeath<EnemyStatesEnum>();
        var followRoute = new EnemyStateFollowRoute<EnemyStatesEnum>();
        
        states.Add(idle);
        states.Add(seek);
        states.Add(pursuit);
        states.Add(damage);
        states.Add(lightAttack);
        states.Add(heavyAttack);
        states.Add(dead);
        states.Add(followRoute);
        states.Add(flee);
        
        idle.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.LightAttack, lightAttack },
            { EnemyStatesEnum.HeavyAttack, heavyAttack },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead },
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Flee, flee},
        });
        
        seek.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.LightAttack, lightAttack },
            { EnemyStatesEnum.HeavyAttack, heavyAttack },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Flee, flee},
        });
        
        pursuit.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.LightAttack, lightAttack },
            { EnemyStatesEnum.HeavyAttack, heavyAttack },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Flee, flee},
        });
        
        lightAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.Flee, flee},
        });
        
        heavyAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Flee, flee},
        });
        
        damage.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Flee, flee},
        });
        
        followRoute.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.Flee, flee},
        });
        
        flee.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute},
        });
        
        foreach (var state in states)
        {
            state.Init(GetModel<EnemyModel>(), GetView<EnemyView>(), this, Root);
        }
        Fsm.SetInit(idle);
    }
    
    protected override void InitTree()
    {
        var idle = new TreeAction(ActionIdle);
        var chase = new TreeAction(ActionSeek);
        var pursuit = new TreeAction(ActionPursuit);
        var damage = new TreeAction(ActionDamage);
        var lightAttack = new TreeAction(ActionLightAttack);
        var heavyAttack = new TreeAction(ActionHeavyAttack);
        var die = new TreeAction(ActionDie);
        var followRoute = new TreeAction(ActionFollowRoute);
        var flee = new TreeAction(ActionFlee);

        var isHeavyAttack = new TreeQuestion(IsHeavyAttack, heavyAttack, lightAttack);
        var willAttack = new TreeQuestion(WillAttack, isHeavyAttack, idle);
        var isInFront = new TreeQuestion(IsPlayerInFront, willAttack, pursuit);
        var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
        var isPlayerOutOfSight = new TreeQuestion(IsPlayerOutOfSight, chase, hasARoute);
        var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInFront, isPlayerOutOfSight);
        var isTooClose = new TreeQuestion(IsInAttackingRange, flee, isPlayerInSight);
        var isPlayerAlive = new TreeQuestion(IsPlayerAlive, isTooClose, hasARoute);
        var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
        var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);

        Root = isAlive;
    }

    private bool IsPlayerInFront()
    {
        return GetModel() && GetModel<WizardModel>().IsTargetInFront(Player.transform);
    }

    private void ActionFlee() => Fsm.Transitions(EnemyStatesEnum.Flee);
}
