using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UpdatableBehaviour : MonoBehaviour, IUpdatable
{
    [field: SerializeField] public Layer UpdateLayer { get; private set; }

    public virtual void Tick() { }
    public virtual void LateTick() { }

    protected virtual void Start()
    {
        AddUpdatable();
    }

    protected virtual void OnEnable()
    {
        AddUpdatable();
    }

    protected virtual void OnDisable()
    {
        RemoveUpdatable();
    }

    private void AddUpdatable()
    {
        if (UpdateManager.Instance == null) return;
        UpdateManager.Instance.Add(this);
    }

    private void RemoveUpdatable()
    {
        if (UpdateManager.Instance == null) return;
        UpdateManager.Instance.Remove(this);
    }
}
