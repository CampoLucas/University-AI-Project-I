using UnityEngine;

namespace Game.Entities.Enemies
{
    public class EnemyModel : EntityModel
    {
        private FieldOfView _fieldOfView;

        protected override void Awake()
        {
            base.Awake();
            _fieldOfView = GetComponent<FieldOfView>();
        }

        public bool CheckRange(Transform target) => _fieldOfView && _fieldOfView.CheckRange(target);
        public bool CheckAngle(Transform target) => _fieldOfView && _fieldOfView.CheckAngle(target);
        public bool CheckView(Transform target) => _fieldOfView && _fieldOfView.CheckView(target);
    }
}