using System;
using System.Collections.Generic;
using Game.Player.States;
using Game.FSM;
using UnityEngine;

namespace Game.Player
{
    [System.Serializable]
    public struct Transition
    {
        [SerializeField] private PlayerStatesEnum key;
        [SerializeField] private PlayerStateBase<PlayerStatesEnum> state;
    }

    [System.Serializable]
    public struct State
    {
        [SerializeField] private string name;
        [SerializeField] private PlayerStatesEnum[] inputs;
    }
    
    public class PlayerController : MonoBehaviour
    {
        private PlayerModel _model;
        private PlayerView _view;
        private PlayerInputHandler _inputs;
        private FSM<PlayerStatesEnum> _fsm;
        private List<PlayerStateBase<PlayerStatesEnum>> _states;
        [SerializeField] private Transition[] tr;
        [SerializeField] private State[] st;

        private void InitFSM()
        {
            _fsm = new FSM<PlayerStatesEnum>();
            _states = new List<PlayerStateBase<PlayerStatesEnum>>();
            
            var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Moving, PlayerStatesEnum.LightAttack01, PlayerStatesEnum.HeavyAttack01, PlayerStatesEnum.Damage);
            var move = new PlayerStateMove<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.LightAttack01, PlayerStatesEnum.HeavyAttack01, PlayerStatesEnum.Damage);
            var lightAttackOne = new PlayerStateLightAttackOne<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.LightAttack02, PlayerStatesEnum.Damage);
            var lightAttackTwo = new PlayerStateLightAttackTwo<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage);
            var heavyAttackOne = new PlayerStateHeavyAttackOne<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage);
            var damage = new PlayerStateDamage<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving);
            
            _states.Add(idle);
            _states.Add(move);
            _states.Add(lightAttackOne);
            _states.Add(lightAttackTwo);
            _states.Add(heavyAttackOne);
            _states.Add(damage);

            idle.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.LightAttack01, lightAttackOne },
                { PlayerStatesEnum.HeavyAttack01, heavyAttackOne },
                { PlayerStatesEnum.Damage, damage },
            });
            
            move.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.LightAttack01, lightAttackOne },
                { PlayerStatesEnum.HeavyAttack01, heavyAttackOne },
                { PlayerStatesEnum.Damage, damage },
            });
            
            lightAttackOne.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.LightAttack02, lightAttackTwo},
                { PlayerStatesEnum.Damage, damage },
            });
            
            lightAttackTwo.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Damage, damage },
            });
            
            heavyAttackOne.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
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

        private void OnDestroy()
        {
            _model = null;
            _view = null;
            _inputs = null;
            _fsm = null;
            _states = null;
        }
    }
}