using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Enemies.States;
using Game.Entities;
using UnityEngine;
using Game.FSM;
using Game.Sheared;
using Game.Entities.Steering;
using Game.Interfaces;
using Game.Player;
using Game.SO;
using Unity.VisualScripting;

namespace Game.Enemies
{
    public class EnemyController : EntityController<EnemyStatesEnum>
    {
        [field: SerializeField] public PlayerModel Player { get; private set; }

        protected ITreeNode Root;

        protected ISteering Seek;
        protected ISteering Pursuit;
        protected ISteering ObsAvoidance;
        
        private EnemySO _data;

        
        protected virtual void InitSteering()
        {
            var transform1 = transform;
            var transform2 = Player.transform;
            Seek = new Seek(transform1, transform2);
            Pursuit = new Pursuit(transform1, Player, _data.PursuitTime);
            ObsAvoidance = new ObstacleAvoidance(transform1, _data.ObsAngle, _data.ObsRange, _data.MaxObs, _data.ObsMask);
        }

        protected override void InitFSM()
        {
            base.InitFSM();
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

        protected virtual void InitTree()
        {
            var idle = new TreeAction(ActionIdle);
            var chase = new TreeAction(ActionSeek);
            var pursuit = new TreeAction(ActionPursuit);
            var damage = new TreeAction(ActionDamage);
            var lightAttack = new TreeAction(ActionLightAttack);
            var heavyAttack = new TreeAction(ActionHeavyAttack);
            var die = new TreeAction(ActionDie);
            var followRoute = new TreeAction(ActionFollowRoute);

            var isHeavyAttack = new TreeQuestion(IsHeavyAttack, heavyAttack, lightAttack);
            var willAttack = new TreeQuestion(WillAttack, isHeavyAttack, idle);
            var isInAttackRange = new TreeQuestion(IsInAttackingRange, willAttack, pursuit);
            var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
            var isPlayerOutOfSight = new TreeQuestion(IsPlayerOutOfSight, chase, hasARoute);
            var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInAttackRange, isPlayerOutOfSight);
            var isPlayerAlive = new TreeQuestion(IsPlayerAlive, isPlayerInSight, hasARoute);
            var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
            var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);

            Root = isAlive;
        }
        
        protected override void Awake()
        {
            base.Awake();
            _data = GetModel().GetData<EnemySO>();
        }

        protected override void Start()
        {
            InitSteering();
            InitTree();
            base.Start();
            
            //_model.Spawn();
        }

        public ISteering GetSeek() => Seek;
        public ISteering GetPursuit() => Pursuit;
        public ISteering GetObsAvoid() => ObsAvoidance;

        protected bool IsInAttackingRange()
        {
            return GetModel() && GetModel<EnemyModel>().TargetInRange(Player.transform);
        }

        protected bool HasARoute()
        {
            if (GetModel())
                return GetModel<EnemyModel>().HasARoute();
            return false;
        }

        protected bool IsPlayerInSight()
        {
            return GetModel() && GetModel<EnemyModel>().IsTargetInSight(Player.transform);
        }

        protected bool IsPlayerOutOfSight()
        {
            if (GetModel())
                return !GetModel<EnemyModel>().IsTargetInSight(Player.transform) && GetModel<EnemyModel>().IsFollowing();
            return false;
        }

        // ToDo: Usar ruleta con algo que de mas de 2 opciones
        protected bool WillAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 10f },
                { false, 0.5f },
            });
        }

        protected bool IsHeavyAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 1.5f },
                { false, 10f },
            });
        }

        protected bool IsPlayerAlive()
        {
            if (GetModel())
                return GetModel<EnemyModel>().IsPlayerAlive(Player);
            return false;
        }

        protected bool HasTakenDamage()
        {
            if (GetModel())
                return GetModel<EnemyModel>().HasTakenDamage();
            return false;
        }

        protected bool IsAlive()
        {
            if (GetModel())
                return GetModel<EnemyModel>().IsAlive();
            return false;
        }

        protected void ActionSeek() => Fsm.Transitions(EnemyStatesEnum.Seek);
        protected void ActionPursuit() => Fsm.Transitions(EnemyStatesEnum.Pursuit);
        protected void ActionLightAttack() => Fsm.Transitions(EnemyStatesEnum.LightAttack);
        protected void ActionHeavyAttack() => Fsm.Transitions(EnemyStatesEnum.HeavyAttack);
        protected void ActionDamage() => Fsm.Transitions(EnemyStatesEnum.Damage);
        protected void ActionDie() => Fsm.Transitions(EnemyStatesEnum.Die);
        protected void ActionIdle() => Fsm.Transitions(EnemyStatesEnum.Idle);
        protected void ActionFollowRoute() => Fsm.Transitions(EnemyStatesEnum.FollowRoute);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (Root != null)
                Root.Dispose();
            Seek.Dispose();
            Pursuit.Dispose();
            ObsAvoidance.Dispose();
            Player = null;
            Root = null;
            Seek = null;
            Pursuit = null;
            ObsAvoidance = null;
        }
    }
}