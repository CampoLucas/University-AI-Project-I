using System;
using System.Collections.Generic;
using Game.Player.States;
using Game.FSM;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _model;
        private PlayerView _view;
        private PlayerInputHandler _inputs;
        private FSM<PlayerStatesEnum> _fsm;
        private List<PlayerStateBase<PlayerStatesEnum>> _states;

        private void InitFSM()
        {
            _fsm = new FSM<PlayerStatesEnum>();
            _states = new List<PlayerStateBase<PlayerStatesEnum>>();
            
            var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Moving, PlayerStatesEnum.Attack, PlayerStatesEnum.Damage);
            var move = new PlayerStateMove<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Attack, PlayerStatesEnum.Damage);
            var attack = new PlayerStateAttack<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage);
            var damage = new PlayerStateDamage<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving);
            
            _states.Add(idle);
            _states.Add(move);
            _states.Add(attack);
            _states.Add(damage);

            idle.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Attack, attack },
                { PlayerStatesEnum.Damage, damage },
            });
            
            move.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Attack, attack },
                { PlayerStatesEnum.Damage, damage },
            });
            
            attack.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Damage, damage },
            });
            
            damage.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
            });

            foreach (var state in _states)
            {
                state.Init(_model, _view, _inputs, _fsm);
            }
            _states = null;
            _fsm.SetInit(idle);
        }

        private void Awake()
        {
            _model = GetComponent<PlayerModel>();
            _view = GetComponent<PlayerView>();
            _inputs = GetComponent<PlayerInputHandler>();
            InitFSM();
        }

        private void Update()
        {
            _fsm.OnUpdate();
        }
    }
}