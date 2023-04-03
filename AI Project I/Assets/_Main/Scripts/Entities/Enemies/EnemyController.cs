using System;
using UnityEngine;

namespace Game.Entities.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        private EnemyModel _model;

        private void Awake()
        {
            _model = GetComponent<EnemyModel>();
        }

        private void Update()
        {
            if (target && _model.CheckRange(target) && _model.CheckAngle(target) && _model.CheckView(target))
            {
                print("Dentro");
            }
            else
            {
                print("Fuera");
            }
        }
    }
}