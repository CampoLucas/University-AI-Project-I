using Game.Entities;
using Game.Interfaces;
using Game.SO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget
{
    private Transform _origin;
    private EntityModel _model;
    private EnemySO _data;

    public FollowTarget(Transform origin, EntityModel model, EnemySO data)
    {
        _origin = origin;
        _model = model;
        _data = data;
    }

    public void Follow(Vector3 target, ISteering obsAvoidance)
    {
        var transform1 = _origin;
        var dir = (target - transform1.position).normalized + obsAvoidance.GetDir() * _data.ObsMultiplier;
        dir.y = 0;
        _model.Move(dir);
        _model.Rotate(dir);
    }
    
    public void Follow(Vector3 target,Vector3 flocking, ISteering obsAvoidance)
    {
        var transform1 = _origin;
        var dir = target - transform1.position;
        var finalDir = (dir + flocking + obsAvoidance.GetDir() * _data.ObsMultiplier).normalized;
        finalDir.y = 0;
        _model.Move(finalDir);
        _model.Rotate(finalDir);
    }

    public void Follow(Transform target, ISteering obsAvoidance)
    {
        Follow(target.position, obsAvoidance);
    }

    public void Follow(Transform target, Vector3 flocking, ISteering obsAvoidance)
    {
        var transform1 = _origin;
        var dir = (target.position - transform1.position).normalized + (obsAvoidance.GetDir() * _data.ObsMultiplier) + flocking;
        dir.y = 0;
        _model.Move(transform1.forward);
        _model.Rotate(dir);
    }

    public void Follow(ISteering steering, ISteering obsAvoindance)
    {
        var dir = (steering.GetDir() + obsAvoindance.GetDir() * _data.ObsMultiplier).normalized;
        dir.y = 0;
        _model.Move(dir);
        _model.Rotate(dir);
    }

    public void Follow(IPathfinder pathfinder, ISteering steering, ISteering obsAvoidance)
    {
        if (pathfinder.NextPoint == 0 && pathfinder.NextPoint + 1 < pathfinder.Waypoints.Count)
            pathfinder.SetNextPoint();

        if (pathfinder.NextPoint >= pathfinder.Waypoints.Count)
            return;

        var point = pathfinder.Waypoints[pathfinder.NextPoint];
        point.y = _origin.position.y;

        var dir = point - _origin.position;
        var finalDir = (dir + obsAvoidance.GetDir() * _data.ObsMultiplier).normalized;
        if (dir.sqrMagnitude < 0.001f)
        {
            if (pathfinder.NextPoint + 1 < pathfinder.Waypoints.Count)
                pathfinder.SetNextPoint();
        }


        // Si es el ultimo punto del path va directo hacia el player en vez del punto
        if (pathfinder.NextPoint == pathfinder.Waypoints.Count - 1)
            finalDir = (steering.GetDir() + obsAvoidance.GetDir() * _data.ObsMultiplier).normalized;

        finalDir.y = 0;

        
        _model.Move(finalDir);
        _model.Rotate(finalDir);
    }
    
    public void Follow(IPathfinder pathfinder, Vector3 flocking, ISteering obsAvoidance)
    {
        if (pathfinder.NextPoint == 0 && pathfinder.NextPoint + 1 < pathfinder.Waypoints.Count)
            pathfinder.SetNextPoint();

        if (pathfinder.NextPoint >= pathfinder.Waypoints.Count)
            return;

        var point = pathfinder.Waypoints[pathfinder.NextPoint];
        point.y = _origin.position.y;
        
        var dir = point - _origin.position;
        var finalDir = (dir + flocking + obsAvoidance.GetDir() * _data.ObsMultiplier).normalized;
        if (dir.sqrMagnitude < 0.001f)
        {
            if (pathfinder.NextPoint + 1 < pathfinder.Waypoints.Count)
                pathfinder.SetNextPoint();
        }

        finalDir.y = 0;

        _model.Move(finalDir);
        _model.Rotate(finalDir);
    }
}
