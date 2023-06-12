using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Entities.Flocking;
using Game.Entities.Slime.States;
using Game.Entities.Steering;
using Game.FSM;
using Game.Interfaces;
using Game.Slime.States;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeController : EntityController<SlimeStatesEnum>
    {
        [SerializeField] private EnemySO data;
        
        private ITreeNode _root;
        
        private ISteering _obsAvoidance;
        private FlockingManager _flocking;

        private bool _isFlockingNull;


        protected override void Awake()
        {
            base.Awake();
            _flocking = GetComponent<FlockingManager>();
        }

        protected override void Start()
        {
            InitSteering();
            InitTree();
            base.Start();

            _isFlockingNull = _flocking == null;
        }
        
        private void InitSteering()
        {
            _obsAvoidance = new ObstacleAvoidance(transform, data.ObsAngle, data.ObsRange, data.MaxObs,data.ObsMask);
        }

        protected override void InitFSM()
        {
            base.InitFSM();

            var states = new List<SlimeStateBase<SlimeStatesEnum>>();

            var idle = new SlimeStateIdle<SlimeStatesEnum>();
            var move = new SlimeStateMove<SlimeStatesEnum>();
            var die = new SlimeStatesDead<SlimeStatesEnum>();
            
            states.Add(idle);
            states.Add(move);
            
            idle.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Move, move },
                { SlimeStatesEnum.Die, die },
            });
            
            move.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Idle, idle },
                { SlimeStatesEnum.Die, die },
            });

            foreach (var state in states)
            {
                state.Init(GetModel<SlimeModel>(), this,Fsm, _root);
            }
            
            Fsm.SetInit(idle);
        }

        private void InitTree()
        {
            var idle = new TreeAction(ActionIdle);
            var move = new TreeAction(ActionMove);
            var death = new TreeAction(ActionDead);

            var hasToMove = new TreeQuestion(HasToMove, move, idle);
            var isAlive = new TreeQuestion(IsAlive, hasToMove, death);

            _root = isAlive;
        }

        #region TreeActions

        private void ActionIdle()
        {
            if(Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.Idle);
        }

        private void ActionMove()
        {
            if(Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.Move);
        }

        private void ActionDead()
        {
            if(Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.Die);
        }

        #endregion

        #region TreeQuestions

        private bool HasToMove()
        {
            return GetModel() && GetModel<SlimeModel>().HasARoute();
        }

        private bool IsAlive()
        {
            return GetModel() && GetModel<SlimeModel>().IsAlive();
        }

        #endregion

        #region Components

        public ISteering GetAvoidance()
        {
            return _obsAvoidance;
        }

        public FlockingManager GetFlocking()
        {
            return _isFlockingNull ? default : _flocking;
        }

        #endregion
        

    }
}