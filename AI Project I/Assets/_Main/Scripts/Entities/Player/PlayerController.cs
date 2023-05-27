using System;
using System.Collections.Generic;
using Game.Entities;
using Game.Player.States;
using Game.FSM;
using UnityEngine;

namespace Game.Player
{
    public class PlayerController : EntityController<PlayerStatesEnum>
    {
        private PlayerInputHandler _inputs;
        private List<PlayerStateBase<PlayerStatesEnum>> _states;

        protected override void InitFSM()
        {
            base.InitFSM();
            _states = new List<PlayerStateBase<PlayerStatesEnum>>();
            
            var idle = new PlayerStateIdle<PlayerStatesEnum>(PlayerStatesEnum.Moving, PlayerStatesEnum.LightAttack, PlayerStatesEnum.HeavyAttack, PlayerStatesEnum.Damage, PlayerStatesEnum.Dead);
            var move = new PlayerStateMove<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.LightAttack, PlayerStatesEnum.HeavyAttack, PlayerStatesEnum.Damage, PlayerStatesEnum.Dead);
            var lightAttack = new PlayerStateLightAttackOne<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage, PlayerStatesEnum.Dead);
            var heavyAttack = new PlayerStateHeavyAttackOne<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage, PlayerStatesEnum.Dead);
            var damage = new PlayerStateDamage<PlayerStatesEnum>(PlayerStatesEnum.Idle, PlayerStatesEnum.Moving, PlayerStatesEnum.Damage, PlayerStatesEnum.Dead);
            var dead = new PlayerStateDead<PlayerStatesEnum>();
            
            _states.Add(idle);
            _states.Add(move);
            _states.Add(lightAttack);
            _states.Add(heavyAttack);
            _states.Add(damage);
            _states.Add(dead);

            idle.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.LightAttack, lightAttack },
                { PlayerStatesEnum.HeavyAttack, heavyAttack },
                { PlayerStatesEnum.Damage, damage },
                { PlayerStatesEnum.Dead, dead },
            });
            
            move.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.LightAttack, lightAttack },
                { PlayerStatesEnum.HeavyAttack, heavyAttack },
                { PlayerStatesEnum.Damage, damage },
                { PlayerStatesEnum.Dead, dead },
            });
            
            lightAttack.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Damage, damage },
                { PlayerStatesEnum.Dead, dead },
            });
            
            heavyAttack.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Damage, damage },
                { PlayerStatesEnum.Dead, dead },
            });
            
            damage.AddTransition(new Dictionary<PlayerStatesEnum, IState<PlayerStatesEnum>>
            {
                { PlayerStatesEnum.Idle, idle },
                { PlayerStatesEnum.Moving, move },
                { PlayerStatesEnum.Dead, dead },
            });

            foreach (var state in _states)
            {
                state.Init(GetModel<PlayerModel>(), GetView<PlayerView>(), _inputs, Fsm);
            }
            _states = null;
            Fsm.SetInit(idle);
        }

        protected override void Awake()
        {
            base.Awake();
            _inputs = GetComponent<PlayerInputHandler>();
        }

        private void Update()
        {
            Fsm.OnUpdate();
        }

        private void OnDestroy()
        {
            _inputs = null;
            Fsm = null;
            _states = null;
        }
    }
}