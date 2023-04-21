using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBladeScript : MonoBehaviour
{
    [SerializeField] private bool startLeft;
    [SerializeField] private float speed;
    [SerializeField] private float limit;
    private float currentMovement = 0;
    private float normalize;
    private Rigidbody rb;
    private void Start()
    {
        normalize = startLeft ? 1 : -1;
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity += new Vector3(normalize * Time.deltaTime * speed, 0, 0);
        currentMovement += Vector3.left.x * speed * Time.deltaTime * normalize;
        Debug.Log(Math.Abs(currentMovement));
        if (Math.Abs(currentMovement) >= limit)
        {
            normalize *= -1;
        }
    }

}
