using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PendulumScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float limit;
    private float random;
    private void Awake()
    {
            random = Random.Range(0, 1);
    }

    void Update()
    {
        float angle = limit * Mathf.Cos(Time.time + random * speed);
        transform.localRotation = Quaternion.Euler(0,0,- angle);
    }
}
