using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingBladeScript : MonoBehaviour
{
    [SerializeField] private Transform saw, pointA, pointB;
    [SerializeField] private float speed;
    private Transform _currentPoint;
    private void Start()
    {
        saw.transform.position = pointA.transform.position;
        _currentPoint = pointB;
    }

    void Update()
    {
        saw.position = Vector3.MoveTowards(saw.position, _currentPoint.position, speed * Time.deltaTime);

        if (Vector3.Distance(saw.position, _currentPoint.position) <= 0.1f)
        {
            _currentPoint = _currentPoint == pointB ? pointA : pointB;
        }
    }
}
