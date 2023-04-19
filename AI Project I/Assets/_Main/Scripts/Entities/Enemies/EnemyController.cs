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
    public class EnemyController : EntityController
    {
        [field: SerializeField] public PlayerModel Player { get; private set; }
        private EnemySO _data;
        private EnemyModel _model;
        private EnemyView _view;
        private FSM<EnemyStatesEnum> _fsm;
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

        protected override void InitFsm()
        {
            _fsm = new FSM<EnemyStatesEnum>();
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
                state.Init(_model, _view, this, _root);
            }
            _states = null;
            _fsm.SetInit(idle);
        }

        public void InitTree()
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
            // var isWaitTimeOver = new TreeQuestion(IsWaitTimeOver, die, isPlayerInSight);
            var isPlayerAlive = new TreeQuestion(IsPlayerAlive, isPlayerInSight, hasARoute);
            var hasTakenDamage = new TreeQuestion(HasTakenDamage, damage, isPlayerAlive);
            var isAlive = new TreeQuestion(IsAlive, hasTakenDamage, die);

            _root = isAlive;
        }
        
        private void Awake()
        {
            _model = GetComponent<EnemyModel>();
            _view = GetComponent<EnemyView>();
            _data = _model.GetData<EnemySO>();
        }

        protected override void Start()
        {
            InitSteering();
            InitTree();
            base.Start();
            _model.Spawn();
        }

        private void Update()
        {
            _fsm.OnUpdate();
            //_root.Execute();
        }

        public ISteering GetSeek() => _seek;
        public ISteering GetPursuit() => _pursuit;
        public ISteering GetObsAvoid() => _obsAvoidance;

        private bool IsInAttackingRange()
        {
            return _model.TargetInRange(Player.transform);
        }

        private bool HasARoute()
        {
            return _model.HasARoute();
        }

        private bool IsPlayerInSight()
        {
            return _model.IsTargetInSight(Player.transform);
        }

        private bool IsPlayerOutOfSight()
        {
            return !_model.IsTargetInSight(Player.transform) && _model.IsFollowing();
        }

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
            return _model.IsPlayerAlive();
        }

        private bool HasTakenDamage()
        {
            return _model.HasTakenDamage();
        }

        private bool IsAlive()
        {
            return _model.IsAlive();
        }

        private void ActionSeek()
        {
            _fsm.Transitions(EnemyStatesEnum.Seek);
        }

        private void ActionPursuit()
        {
            _fsm.Transitions(EnemyStatesEnum.Pursuit);
        }

        private void ActionLightAttack()
        {
            _fsm.Transitions(EnemyStatesEnum.LightAttack);
        }

        private void ActionHeavyAttack()
        {
            _fsm.Transitions(EnemyStatesEnum.HeavyAttack);
        }

        private void ActionDamage()
        {
            _fsm.Transitions(EnemyStatesEnum.Damage);
        }

        private void ActionDie()
        {
            _fsm.Transitions(EnemyStatesEnum.Die);
        }

        private void ActionIdle()
        {
            _fsm.Transitions(EnemyStatesEnum.Idle);
        }

        private void ActionFollowRoute()
        {
            _fsm.Transitions(EnemyStatesEnum.FollowRoute);
        }

        private void OnDestroy()
        {
            _fsm.Dispose();
            _root.Dispose();
            _seek.Dispose();
            _pursuit.Dispose();
            _obsAvoidance.Dispose();
            Player = null;
            _model = null;
            _view = null;
            _fsm = null;
            _states = null;
            _root = null;
            _seek = null;
            _pursuit = null;
            _obsAvoidance = null;
        }
    }
}