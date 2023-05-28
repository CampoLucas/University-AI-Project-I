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

        private EnemySO _data;

        private List<EnemyStateBase<EnemyStatesEnum>> _states;

        private ITreeNode _root;

        private ISteering _seek;
        private ISteering _pursuit;
        private ISteering _obsAvoidance;
        
        private void InitSteering()
        {
            var transform1 = transform;
            var transform2 = Player.transform;
            _seek = new Seek(transform1, transform2);
            _pursuit = new Pursuit(transform1, Player, _data.PursuitTime);
            _obsAvoidance = new ObstacleAvoidance(transform1, _data.ObsAngle, _data.ObsRange, _data.MaxObs, _data.ObsMask);
        }

        protected override void InitFSM()
        {
            base.InitFSM();
            _states = new List<EnemyStateBase<EnemyStatesEnum>>();

            var idle = new EnemyStateIdle<EnemyStatesEnum>();
            var seek = new EnemyStateSeek<EnemyStatesEnum>();
            var pursuit = new EnemyStatePursuit<EnemyStatesEnum>();
            var damage = new EnemyStateDamage<EnemyStatesEnum>();
            var lightAttack = new EnemyStateLightAttack<EnemyStatesEnum>();
            var heavyAttack = new EnemyStateHeavyAttack<EnemyStatesEnum>();
            var dead = new EnemyStateDeath<EnemyStatesEnum>();
            var followRoute = new EnemyStateFollowRoute<EnemyStatesEnum>();
            
            _states.Add(idle);
            _states.Add(seek);
            _states.Add(pursuit);
            _states.Add(damage);
            _states.Add(lightAttack);
            _states.Add(heavyAttack);
            _states.Add(dead);
            _states.Add(followRoute);
            
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
            
            followRoute.AddTransition(new FlexibleDictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Pursuit, pursuit },
                { EnemyStatesEnum.Seek, seek },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
            });

            foreach (var state in _states)
            {
                state.Init(GetModel<EnemyModel>(), GetView<EnemyView>(), this, _root);
            }
            _states = null;
            Fsm.SetInit(idle);
        }

        private void InitTree()
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
            var isInAttackRange = new TreeQuestion(IsInAttackingRange, willAttack, chase);
            var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
            var isPlayerOutOfSight = new TreeQuestion(IsPlayerOutOfSight, pursuit, hasARoute);
            var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInAttackRange, isPlayerOutOfSight);
            var isPlayerAlive = new TreeQuestion(IsPlayerAlive, isPlayerInSight, hasARoute);
            var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
            var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);

            _root = isAlive;
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

        public ISteering GetSeek() => _seek;
        public ISteering GetPursuit() => _pursuit;
        public ISteering GetObsAvoid() => _obsAvoidance;

        private bool IsInAttackingRange()
        {
            if (GetModel())
                return GetModel<EnemyModel>().TargetInRange(Player.transform);
            return false;
        }

        private bool HasARoute()
        {
            if (GetModel())
                return GetModel<EnemyModel>().HasARoute();
            return false;
        }

        private bool IsPlayerInSight()
        {
            if (GetModel())
                return GetModel<EnemyModel>().IsTargetInSight(Player.transform);
            return false;
        }

        private bool IsPlayerOutOfSight()
        {
            if (GetModel())
                return !GetModel<EnemyModel>().IsTargetInSight(Player.transform) && GetModel<EnemyModel>().IsFollowing();
            return false;
        }

        // ToDo: Usar ruleta con algo que de mas de 2 opciones
        private bool WillAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 10f },
                { false, 0.5f },
            });
        }

        private bool IsHeavyAttack()
        {
            return MyRandoms.Roulette(new Dictionary<bool, float>
            {
                { true, 1.5f },
                { false, 10f },
            });
        }

        private bool IsPlayerAlive()
        {
            if (GetModel())
                return GetModel<EnemyModel>().IsPlayerAlive(Player);
            return false;
        }

        private bool HasTakenDamage()
        {
            if (GetModel())
                return GetModel<EnemyModel>().HasTakenDamage();
            return false;
        }

        private bool IsAlive()
        {
            if (GetModel())
                return GetModel<EnemyModel>().IsAlive();
            return false;
        }

        private void ActionSeek()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.Seek);
        }

        private void ActionPursuit()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.Pursuit);
        }

        private void ActionLightAttack()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.LightAttack);
        }

        private void ActionHeavyAttack()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.HeavyAttack);
        }

        private void ActionDamage()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.Damage);
        }

        private void ActionDie()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.Die);
        }

        private void ActionIdle()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.Idle);
        }

        private void ActionFollowRoute()
        {
            if (Fsm == null) return;
            Fsm.Transitions(EnemyStatesEnum.FollowRoute);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _root.Dispose();
            _seek.Dispose();
            _pursuit.Dispose();
            _obsAvoidance.Dispose();
            Player = null;
            _states = null;
            _root = null;
            _seek = null;
            _pursuit = null;
            _obsAvoidance = null;
        }
    }
}