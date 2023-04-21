using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    [SerializeField] private GameObject bullet;

    [SerializeField] private GameObject spawner;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
//            spawner.transform.LookAt(other.transform);
            Instantiate(bullet, spawner.transform.localPosition, spawner.transform.localRotation);
    }
}
