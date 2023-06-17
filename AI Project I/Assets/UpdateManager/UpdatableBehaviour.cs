using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UpdatableBehaviour : MonoBehaviour, IUpdatable
{
    [field: SerializeField] public UpdateMask UpdateMask { get; private set; }
    protected UpdateManager UpdateManager;

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
        if (UpdateManager == null)
        {
            UpdateManager = UpdateManager.Instance;
            UpdateManager.Add(this);
        }
    }

    private void RemoveUpdatable()
    {
        if (UpdateManager.Instance == null) return;
        if (UpdateManager == null)
        {
            UpdateManager = UpdateManager.Instance;
            UpdateManager.Remove(this);
        }
    }
}
