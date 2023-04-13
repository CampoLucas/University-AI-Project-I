using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Enemies.States;
using Game.Entities;
using UnityEngine;
using Game.FSM;
using Game.Sheared;
using Unity.VisualScripting;

namespace Game.Enemies
{
    public class EnemyController : EntityController
    {
        [field: SerializeField] public Transform Target { get; private set; }
        private EnemyModel _model;
        private EnemyView _view;
        private FSM<EnemyStatesEnum> _fsm;
        private List<EnemyStateBase<EnemyStatesEnum>> _states;
        private ITreeNode _root;

        protected override void InitFsm()
        {
            _fsm = new FSM<EnemyStatesEnum>();
            _states = new List<EnemyStateBase<EnemyStatesEnum>>();

            var idle = new EnemyStateIdle<EnemyStatesEnum>();
            var chase = new EnemyStateChase<EnemyStatesEnum>();
            var damage = new EnemyStateDamage<EnemyStatesEnum>();
            var lightAttack = new EnemyStateLightAttack<EnemyStatesEnum>();
            var heavyAttack = new EnemyStateHeavyAttack<EnemyStatesEnum>();
            var dead = new EnemyStateDeath<EnemyStatesEnum>();
            var followRoute = new EnemyStateFollowRoute<EnemyStatesEnum>();
            
            _states.Add(idle);
            _states.Add(chase);
            _states.Add(damage);
            _states.Add(lightAttack);
            _states.Add(heavyAttack);
            _states.Add(dead);
            _states.Add(followRoute);
            
            idle.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Chase, chase },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead },
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            chase.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            lightAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Chase, chase },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.FollowRoute, followRoute },
                { EnemyStatesEnum.Die, dead},
            });
            
            heavyAttack.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Chase, chase },
                { EnemyStatesEnum.Damage, damage },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            damage.AddTransition(new Dictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Chase, chase },
                { EnemyStatesEnum.Die, dead},
                { EnemyStatesEnum.FollowRoute, followRoute },
            });
            
            followRoute.AddTransition(new FlexibleDictionary<EnemyStatesEnum, IState<EnemyStatesEnum>>
            {
                { EnemyStatesEnum.Idle, idle },
                { EnemyStatesEnum.Chase, chase },
                { EnemyStatesEnum.LightAttack, lightAttack },
                { EnemyStatesEnum.HeavyAttack, heavyAttack },
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
            var chase = new TreeAction(ActionChase);
            var damage = new TreeAction(ActionDamage);
            var lightAttack = new TreeAction(ActionLightAttack);
            var heavyAttack = new TreeAction(ActionHeavyAttack);
            var die = new TreeAction(ActionDie);
            var followRoute = new TreeAction(ActionFollowRoute);

            var isHeavyAttack = new TreeQuestion(IsHeavyAttack, heavyAttack, lightAttack);
            var willAttack = new TreeQuestion(WillAttack, isHeavyAttack, idle);
            var isInAttackRange = new TreeQuestion(IsInAttackingRange, willAttack, chase);
            var hasARoute = new TreeQuestion(HasARoute, followRoute, idle);
            var isPlayerInSight = new TreeQuestion(IsPlayerInSight, isInAttackRange, hasARoute);
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
            InitTree();
        }

        private void Update()
        {
            _fsm.OnUpdate();
            //_root.Execute();
        }

        private bool IsInAttackingRange()
        {
            return _model.TargetInRange(Target);
        }

        private bool HasARoute()
        {
            return _model.HasARoute();
        }

        private bool IsPlayerInSight()
        {
            return _model.IsAlive() && _model.CheckRange(Target) && _model.CheckAngle(Target) && _model.CheckView(Target);
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

        private void ActionChase()
        {
            _fsm.Transitions(EnemyStatesEnum.Chase);
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
            Target = null;
            _model = null;
            _view = null;
            _fsm.Dispose();
            Logging.LogDestroy("FSM Disposed");
            _fsm = null;
            Logging.LogDestroy("FSM Nullified");
            _states = null;
            _root.Dispose();
            Logging.LogDestroy("TreeRoot Disposed");
            _root = null;
            Logging.LogDestroy("TreeRoot Nullified");
        }
    }
}