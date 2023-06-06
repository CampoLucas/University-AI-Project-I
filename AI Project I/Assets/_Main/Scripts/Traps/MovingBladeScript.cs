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

    // Update is called once per frame
    void Update()
    {
        xVelocity += Time.deltaTime * speed * normalize;
        xVelocity = Mathf.Clamp(xVelocity, -Maxspeed, Maxspeed);
        rb.velocity = new Vector3(xVelocity, 0, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(9))
        {
            normalize *= -1;
        }
    }
}
