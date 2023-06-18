using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance { get; private set; }

    [SerializeField] private EnumDataContainer<UpdateLayer, UpdateMask> layers;

    private List<UpdateLayer> layersList = new();
    private Dictionary<UpdateMask, UpdateLayer> _updatableDictionary = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }

        Instance = this;

        layersList = new(layers.GetContent());
        layersList.Sort((layer1, layer2) => layer1.Order.CompareTo(layer2.Order));
    }

    private void Start()
    {
        foreach (var layer in layersList)
        {
            layer.SetUpdateInterval();
        }
    }

    private void Update()
    {
        for (int i = 0; i < layersList.Count; i++)
        {
            layersList[i].RunTick();
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < layersList.Count; i++)
        {
            layersList[i].RunLateTick();
        }
    }

    public void Add(IUpdatable tick)
    {
        if (!_updatableDictionary.TryGetValue(tick.UpdateMask, out var layer))
        {
            layer = layers[(int)tick.UpdateMask];
            _updatableDictionary[tick.UpdateMask] = layer;
        }
        layer.Add(tick);
    }

    public void Remove(IUpdatable tick)
    {
        if (_updatableDictionary.ContainsKey(tick.UpdateMask))
            _updatableDictionary[tick.UpdateMask].Remove(tick);
    }

    public float LayerDelta(UpdateMask updateMask)
    {
        if (!_updatableDictionary.TryGetValue(updateMask, out var layer))
        {
            layer = layers[(int)updateMask];
        }

        return layer.Delta;
    }

    private void OnDestroy()
    {
        foreach (var layer in layersList)
        {
            layer.Dispose();
        }
        layersList.Clear();
        _updatableDictionary.Clear();
        layersList = null;
        _updatableDictionary = null;
    }
}

public enum UpdateMask
{
    Default,
    Gameplay,
    UI,
    Inputs,
}

[System.Serializable]
public class UpdateLayer : System.IDisposable
{
    public float Delta => fixedFrameRate ? Time.deltaTime + _updateTime : Time.deltaTime;
    public int Order => order;

    [SerializeField] private int order;
    [SerializeField] private bool fixedFrameRate;
    [Range(30, 360)][SerializeField] public int frameRate = 60;

    private List<IUpdatable> _tickList = new();
    private Dictionary<IUpdatable, UpdateMask> _tickDictionary = new();
    private float _updateInterval;
    private float _updateTime;
    private float _lateUpdateTime;

    public void SetUpdateInterval()
    {
        if (!fixedFrameRate) return;
        if (frameRate > 0)
        {
            _updateInterval = 1 / (float)Mathf.Clamp(frameRate, 30, 360);
        }
        else
        {
            _updateInterval = 1f / 60;
        }
    }

    private void Run(ref float time, System.Action<int> tick, float delta)
    {
        time += delta;
        if (_tickList.Count > 0 && time >= _updateInterval)
        {
            if (_tickList.Count > 1)
            {
                for (var i = 0; i < _tickList.Count; i++)
                {
                    Debug.Log("TFR");
                    tick(i);
                }
            }
            else
            {
                tick(0);
            }
            time = 0;
        }
    }

    private void Run(System.Action<int> tick)
    {
        if (_tickList.Count > 1)
        {
            for (var i = 0; i < _tickList.Count; i++)
            {
                tick(i);
            }
        }
        else
        {
            tick(0);
        }
    }

    public void RunTick()
    {
        if (_tickList.Count == 0) return;
        if (fixedFrameRate)
            Run(ref _updateTime, Tick, Time.deltaTime);
        else
            Run(Tick);

        //_updateTime += Time.deltaTime;

        //if (_tickList.Count > 0 && _updateTime >= _updateInterval)
        //{
        //    if (_tickList.Count > 1)
        //    {
        //        for (var i = 0; i < _tickList.Count; i++)
        //        {
        //            _tickList[i].Tick();
        //        }
        //    }
        //    else
        //    {
        //        _tickList[0].Tick();
        //    }
        //}
    }

    public void RunLateTick()
    {
        if (_tickList.Count == 0) return;
        if (fixedFrameRate)
            Run(ref _lateUpdateTime, LateTick, Time.deltaTime);
        else
            Run(LateTick);

        //_lateUpdateTime += Time.deltaTime;

        //if (_tickList.Count > 0 && _updateTime >= _updateInterval)
        //{
        //    if (_tickList.Count > 1)
        //    {
        //        for (var i = 0; i < _tickList.Count; i++)
        //        {
        //            _tickList[i].LateTick();
        //        }
        //    }
        //    else
        //    {
        //        _tickList[0].Tick();
        //    }
        //}
    }

    private void Tick(int i) => _tickList[i].Tick();
    private void LateTick(int i) => _tickList[i].LateTick();

    public void Add(IUpdatable tick)
    {
        if (_tickDictionary.ContainsKey(tick)) return;
        Debug.Log("Updtatable Added to Layer");
        var layer = tick.UpdateMask;
        _tickDictionary[tick] = layer;
        _tickList.Add(tick);
    }

    public void Remove(IUpdatable tick)
    {
        if (!_tickDictionary.ContainsKey(tick)) return;
        _tickDictionary.Remove(tick);
        _tickList.Remove(tick);
    }

    public void Dispose()
    {
        _tickList.Clear();
        _tickDictionary.Clear();
        _tickList = null;
        _tickDictionary = null;
    }

    ~UpdateLayer()
    {
        Dispose();
    }
}
