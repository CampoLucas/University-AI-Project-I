using System;
using UnityEngine;
using Game.FSM;

namespace Game.Entities
{
    public class EntityController<T> : UpdatableBehaviour
    {
        private EntityModel _model;
        private EntityView _view;
        protected FSM<T> Fsm;
        protected Transform MyTransform;

        protected virtual void InitFSM()
        {
            Fsm = new FSM<T>();
        }

        protected virtual void Awake()
        {
            _model = GetComponent<EntityModel>();
            _view = GetComponent<EntityView>();
            MyTransform = transform;
        }

        protected override void Start()
        {
            base.Start();
            InitFSM();
        }

        public override void Tick()
        {
            base.Tick();
            Fsm.OnUpdate();
        }

        public EntityModel GetModel() => _model;
        public T1 GetModel<T1>() where T1 : EntityModel => (T1)_model ? (T1)_model : default;
        public EntityView GetView() => _view;
        public T1 GetView<T1>() where T1 : EntityView => (T1)_view ? (T1)_view : default;

        protected virtual void OnDestroy()
        {
            _model = null;
            _view = null;
            Fsm.Dispose();
            Fsm = null;
        }
    }
}