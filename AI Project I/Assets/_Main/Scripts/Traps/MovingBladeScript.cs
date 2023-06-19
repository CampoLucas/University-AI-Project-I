using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovingBladeScript : MonoBehaviour
{
    [SerializeField] private Vector3 speed;
    [SerializeField] private Vector3 Maxspeed;
    private int normalize = 1;
    private Rigidbody rb;
    private Vector3 initPos;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        initPos = transform.localPosition;
    }

    void Update()
    {
        rb.velocity += Time.deltaTime * speed * normalize;
        rb.velocity = new Vector3(Mathf.Clamp(rb.velocity.x, -Maxspeed.x, Maxspeed.x),0,
            Mathf.Clamp(rb.velocity.z, -Maxspeed.z, Maxspeed.z));

        if (Vector3.Distance(transform.localPosition, initPos) > 1)
        {
            if (speed.x != 0)
            {
                normalize = transform.localPosition.x > initPos.x ? -1 : 1;
            }
            else if (speed.z != 0)
            {
                normalize = transform.localPosition.z < initPos.z ? -1 : 1;
            }
        }
    }
}
