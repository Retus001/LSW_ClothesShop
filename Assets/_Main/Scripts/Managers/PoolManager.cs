using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObjectType
{
    AudioClip,
    IGPopupSmall,
    DialogueOption,
    InteractionPrompt
}

[Serializable]
public class PoolData
{
    public PoolObjectType type;
    public int amount = 0;
    public GameObject prefab;
    public GameObject container;

    [HideInInspector]
    public List<GameObject> pool = new List<GameObject>();
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField]
    List<PoolData> poolsList;

    void Start()
    {
        foreach (PoolData _pool in poolsList)
            FillPool(_pool);
    }

    void FillPool(PoolData _data)
    {
        for(int i = 0; i < _data.amount; i++)
        {
            GameObject objInstance = null;
            objInstance = Instantiate(_data.prefab, _data.container.transform);
            objInstance.SetActive(false);
            objInstance.transform.localPosition = Vector3.zero;
            _data.pool.Add(objInstance);
        }
    }

    public GameObject GetPoolObject(PoolObjectType _type)
    {
        PoolData selectedPool = GetPoolByType(_type);
        List<GameObject> pool = selectedPool.pool;

        GameObject objInstance = null;
        if(pool.Count > 0)
        {
            objInstance = pool[pool.Count - 1];
            pool.Remove(objInstance);
        } else
        {
            objInstance = Instantiate(selectedPool.prefab, selectedPool.container.transform);
        }

        return objInstance;
    }

    public void ResetObjInstance(GameObject _obj, PoolObjectType _type)
    {
        _obj.SetActive(false);
        _obj.transform.localPosition = Vector3.zero;

        PoolData selectedPoolData = GetPoolByType(_type);
        List<GameObject> pool = selectedPoolData.pool;

        if (!pool.Contains(_obj))
        {
            pool.Add(_obj);
        }
    }

    private PoolData GetPoolByType(PoolObjectType _type)
    {
        foreach(PoolData _pool in poolsList)
        {
            if (_pool.type == _type) return _pool;
        }

        return null;
    }
}
