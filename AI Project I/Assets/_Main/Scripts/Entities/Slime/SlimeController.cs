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
    public class SlimeController : MonoBehaviour
    {
        [SerializeField] private EnemySO data;

        private FSM<SlimeStatesEnum> _fsm;
        private ITreeNode _root;
        
        private SlimeModel _model;
        private ISteering _obsAvoidance;

        private void Awake()
        {
            _model = GetComponent<SlimeModel>();
        }

        private void Start()
        {
            InitSteering();
            InitTree();
            InitFSM();
            
        }

        private void Update()
        {
            _fsm?.OnUpdate();
            _root?.Execute();
        }

        private void InitSteering()
        {
            _obsAvoidance = new ObstacleAvoidance(transform, data.ObsAngle, data.ObsRange, data.MaxObs,data.ObsMask);
        }

        private void InitFSM()
        {
            _fsm = new FSM<SlimeStatesEnum>();
            
            var states = new List<SlimeStateBase<SlimeStatesEnum>>();

            var idle = new SlimeStateIdle<SlimeStatesEnum>();
            var move = new SlimeStateMove<SlimeStatesEnum>();
            
            states.Add(idle);
            states.Add(move);
            
            idle.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Move, move },
            });
            
            move.AddTransition(new Dictionary<SlimeStatesEnum, IState<SlimeStatesEnum>>
            {
                { SlimeStatesEnum.Idle, idle },
            });

            foreach (var state in states)
            {
                state.Init(_model, this,_fsm);
            }
            
            _fsm.SetInit(idle);
        }

        private void InitTree()
        {
            var idle = new TreeAction(() => _fsm.Transitions(SlimeStatesEnum.Idle));
            var move = new TreeAction(() => _fsm.Transitions(SlimeStatesEnum.Move));
            var death = new TreeAction(() => _fsm.Transitions(SlimeStatesEnum.Die));

            var hasToMove = new TreeQuestion(HasToMove, move, idle);
            var isAlive = new TreeQuestion(IsAlive, hasToMove, death);

            _root = isAlive;
        }

        private bool HasToMove()
        {
            return _model.hasToMove;
        }

        private bool IsAlive()
        {
            return true;
        }

        public ISteering GetObsAvoid()
        {
            return _obsAvoidance;
        }
    }
}