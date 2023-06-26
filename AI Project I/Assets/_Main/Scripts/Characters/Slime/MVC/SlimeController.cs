using System;
using System.Collections.Generic;
using Game.DecisionTree;
using Game.Entities.Flocking;
using Game.Entities.Slime.States;
using Game.Entities.Steering;
using Game.FSM;
using Game.Interfaces;
using Game.Sheared;
using Game.Slime.States;
using Game.SO;
using UnityEngine;

namespace Game.Entities.Slime
{
    public class SlimeController : EntityController<SlimeStatesEnum>
    {
        private SlimeSO _data;
        private ITreeNode _root;
        private ISteering _obsAvoidance;
        private FlockingManager _flocking;
        
        private bool _isFlockingNull;
        private bool _isDataNull;

        protected override void Awake()
        {
            base.Awake();
            _data = GetModel().GetData<SlimeSO>();
        }

        protected override void Start()
        {
            _isFlockingNull = _flocking == null;
            _isDataNull = _data == null;
            
            InitSteering();
            InitTree();
            
            if(!_isDataNull)
                _flocking = new SlimeFlockingManager(_data, GetModel<SlimeModel>(), GetModel().GetLevelable());

            _isFlockingNull = _flocking == null;
            
            base.Start();
        }

        private void InitSteering()
        {
            _obsAvoidance = new ObstacleAvoidance(transform, _data.ObsAngle, _data.ObsRange, _data.MaxObs,_data.ObsMask);
        }

        protected override void InitFSM()
        {
            base.InitFSM();

            var states = new List<SlimeStateBase<SlimeStatesEnum>>();

            var idle = new SlimeStateIdle<SlimeStatesEnum>();
            var followRoute = new SlimeStatesFollowRoute<SlimeStatesEnum>();
            var powerUp = new SlimeStatePowerUp<SlimeStatesEnum>();
            var die = new SlimeStatesDead<SlimeStatesEnum>();
            
            states.Add(idle);
            states.Add(followRoute);
            states.Add(powerUp);
            states.Add(die);
            
            idle.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.FollowRoute, followRoute },
                { SlimeStatesEnum.PowerUp, powerUp },
                { SlimeStatesEnum.Die, die }
            });
            
            followRoute.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Idle, idle },
                { SlimeStatesEnum.PowerUp, powerUp },
                { SlimeStatesEnum.Die, die }
            });
            
            powerUp.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Idle, idle },
                { SlimeStatesEnum.FollowRoute, followRoute },
                { SlimeStatesEnum.Die, die }
            });

            foreach (var state in states)
            {
                state.Init(GetModel<SlimeModel>(), this,Fsm, _root);
            }
            
            Fsm.SetInit(idle);
        }

        private void InitTree()
        {
            var death = new TreeAction(ActionDead);
            var roulette = new TreeAction(CheckRoulette);

            var isAlive = new TreeQuestion(IsAlive, roulette, death);

            _root = isAlive;
        }
        
        private void CheckRoulette()
        {
            if (Fsm == null) return;
            var state =MyRandoms.Roulette(new Dictionary<Action, float>
            {
                { ActionMove, _data.MoveOdds},
                { ActionIdle, _data.IdleOdds},
                { ActionPowerUp, _data.PowerUpOdds}
            });
            
            state?.Invoke();
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
            Fsm.Transitions(SlimeStatesEnum.FollowRoute);
        }

        private void ActionPowerUp()
        {
            if (Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.PowerUp);
        }

        private void ActionSpin()
        {
            if (Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.Spin);
        }

        private void ActionDead()
        {
            if(Fsm == null) return;
            Fsm.Transitions(SlimeStatesEnum.Die);
        }

        #endregion

        #region TreeQuestions

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