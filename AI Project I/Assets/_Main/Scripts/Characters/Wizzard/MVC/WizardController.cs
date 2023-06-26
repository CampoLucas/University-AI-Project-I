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
        
        idle.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.LightAttack, lightAttack },
            { EnemyStatesEnum.HeavyAttack, heavyAttack },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead },
            { EnemyStatesEnum.FollowRoute, followRoute },
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
        });
        
        lightAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.FollowRoute, followRoute },
            { EnemyStatesEnum.Die, dead},
        });
        
        heavyAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
        });
        
        damage.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Die, dead},
            { EnemyStatesEnum.FollowRoute, followRoute },
        });
        
        followRoute.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
        {
            { EnemyStatesEnum.Idle, idle },
            { EnemyStatesEnum.Pursuit, pursuit },
            { EnemyStatesEnum.Seek, seek },
            { EnemyStatesEnum.Damage, damage },
            { EnemyStatesEnum.Die, dead},
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
        var die = new TreeAction(ActionDie);
        var followRoute = new TreeAction(ActionFollowRoute);

        var willAttack = new TreeQuestion(WillAttack, lightAttack, idle);
        var isInFront = new TreeQuestion(IsPlayerInFront, willAttack, pursuit);
        var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
        var isPlayerOutOfSight = new TreeQuestion(IsPlayerOutOfSight, chase, hasARoute);
        var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInFront, isPlayerOutOfSight);
        var isPlayerAlive = new TreeQuestion(IsPlayerAlive, isPlayerInSight, hasARoute);
        var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
        var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);

        Root = isAlive;
    }

    private bool IsPlayerInFront()
    {
        return GetModel() && GetModel<WizardModel>().IsTargetInFront(Player.transform);
    }

}
