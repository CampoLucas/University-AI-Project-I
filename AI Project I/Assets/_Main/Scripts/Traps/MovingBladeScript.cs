using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingBladeScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float Maxspeed;
    private int normalize = 1;
    private Rigidbody rb;
    private float xVelocity = 0;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        xVelocity += Time.deltaTime * speed * normalize;
        xVelocity = Mathf.Clamp(xVelocity, -Maxspeed, Maxspeed);
        rb.velocity = new Vector3(xVelocity, 0, 0);

        if (transform.localPosition.x > 1)
        {
            normalize = -1;
        }

        if (transform.localPosition.x < -1)
        {
            normalize = 1;
        }
    }
}
