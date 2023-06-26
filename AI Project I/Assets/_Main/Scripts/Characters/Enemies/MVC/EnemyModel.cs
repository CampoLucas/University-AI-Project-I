using _Main.Scripts.VisionCone;
using UnityEngine;
using Game.Entities;
using Game.Interfaces;
using Game.Player;
using Game.SO;
using Game.Pathfinding;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.Enemies
{
    public class EnemyModel : EntityModel
    {
        private VisionCone visionCone;
        private bool _hasVisionCone;
        private EnemySO _data;
        private FieldOfView _fieldOfView;
        private PathToFollow _path;
        private InRange _range;
        private Vector3 _direction = Vector3.zero;
        private bool _isFollowing;
        private FollowTarget _followTarget;
        private Pathfinder _pathfinder;

        
        protected override void Awake()
        {
            base.Awake();
            _data = GetData<EnemySO>();
            _fieldOfView = new FieldOfView(_data.FOV, transform);
            _path = GetComponent<PathToFollow>();
            _range = new InRange(transform);
            _followTarget = new FollowTarget(transform, this, _data);
            _pathfinder = GetComponent<Pathfinder>();
            visionCone = GetComponentInChildren<VisionCone>();

            if (visionCone != null)
            {
                _hasVisionCone = true;
                visionCone.SetMesh((int)_data.FOV.Angle, _data.FOV.Range);
            }
        }


        public float GetMoveAmount() => Mathf.Clamp01(Mathf.Abs(_direction.x) + Mathf.Abs(_direction.z));

        public override void Move(Vector3 dir)
        {
            _direction = dir;
            base.Move(_direction);
        }

        private bool CheckRange(Transform target) => _fieldOfView.CheckRange(target);
        private bool CheckAngle(Transform target) => _fieldOfView.CheckAngle(target);
        private bool CheckView(Transform target) => _fieldOfView.CheckView(target);

        public void Spawn()
        {
            if (_path)
            {
                transform.position = _path.GetCurrentPoint();
                transform.rotation = Quaternion.LookRotation(_path.GetWaypointDirection());
            }
            
        }

        public Vector3 GetWaypointDirection() => _path.GetWaypointDirection();
        public Vector3 GetNextWaypoint() => _path.GetNextWaypoint();
        public bool HasARoute() => _path.Path;
        public bool ReachedWaypoint() => _path.ReachedWaypoint();
        public void ChangeWaypoint() => _path.ChangeWaypoint();
        
        public void FollowTarget(Transform target, ISteering obsAvoidance)
        {
            _followTarget.Follow(target, obsAvoidance);
        }
        
        public void FollowTarget(Vector3 target, ISteering obsAvoidance)
        {
            _followTarget.Follow(target, obsAvoidance);
        }
        
        public void FollowTarget(Vector3 target, Vector3 flocking ,ISteering obsAvoidance)
        {
            _followTarget.Follow(target,flocking, obsAvoidance);
        }
        
        public void FollowTarget(ISteering steering, ISteering obsAvoidance)
        {
            _followTarget.Follow(steering, obsAvoidance);
        }

        public void FollowTarget(IPathfinder pathfinder, ISteering steering, ISteering obsAvoidance)
        {
            _followTarget.Follow(pathfinder, steering, obsAvoidance);
        }
        public void FollowTarget(IPathfinder pathfinder, Vector3 flocking, ISteering obsAvoidance)
        {
            _followTarget.Follow(pathfinder, flocking, obsAvoidance);
        }

        public bool SetNodes(Vector3 origin, Vector3 target)
        {
            return _pathfinder.SetNodes(origin, target);
        }

        public void CalculatePath()
        {
            _pathfinder.Run();
        }

        public bool IsTargetInRange()
        {
            return _pathfinder.IsTargetInRange();
        }

        public IPathfinder GetPathfinder()
        {
            return _pathfinder;
        }

        public void SetTarget(Transform target)
        {
            _pathfinder.SetTarget(target);
        }

        public bool IsTargetInSight(Transform target) => CheckRange(target) && CheckAngle(target) && CheckView(target);
        public bool IsFollowing() => _isFollowing;
        public void SetFollowing(bool isFollowing) => _isFollowing = isFollowing;
        public bool TargetInRange(Transform target) => _range.GetBool(target, _data.AttackRange);
        public bool IsPlayerAlive(PlayerModel player) => player && player.IsAlive();

        public void SetVisionConeColor(VisionConeEnum input)
        {
            if (_hasVisionCone) 
                visionCone.SetMaterial(input);
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            _fieldOfView.Dispose();
            _fieldOfView = null;
            _data = null;
        }

        protected virtual void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, GetData<EnemySO>().AttackRange);
        }

        #if UNITY_EDITOR
        protected virtual void OnDrawGizmos()
        {
            var transform1 = transform;
            var forward = transform1.forward;
            var position = transform1.position;
            
            #region FOV

            GetData<EnemySO>().FOV.DebugGizmos(transform, Color.red);

            #endregion

            #region ObsAvoidance

            Gizmos.color = Color.blue;
            var halfObs = GetData<EnemySO>().ObsAngle / 2f;
            var obsLeftRayRotation = Quaternion.AngleAxis(-halfObs, Vector3.up);
            var obsRightRayRotation = Quaternion.AngleAxis(halfObs, Vector3.up);

            var obsLeftRayDirection = obsLeftRayRotation * forward;
            var obsRightRayDirection = obsRightRayRotation * forward;

            
            Gizmos.DrawRay(position, obsLeftRayDirection * GetData<EnemySO>().ObsRange);
            Gizmos.DrawRay(position, obsRightRayDirection * GetData<EnemySO>().ObsRange);

            Handles.color = new Color(0f, 0f, 1f, 0.1f);
            Handles.DrawSolidArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObsAngle, GetData<EnemySO>().ObsRange);
            Handles.color = Color.blue;
            Handles.DrawWireArc(position, Vector3.up, obsLeftRayDirection, GetData<EnemySO>().ObsAngle, GetData<EnemySO>().ObsRange);

            #endregion

        }
        #endif
    }
}