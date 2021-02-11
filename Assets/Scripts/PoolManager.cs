using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolType
{
    GrenadeEffect
}

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private GameObject grenadeEffectPrefab;
    public static Pool<ComponentElement> grenadeEffectPool;

    private void Awake()
    {
        grenadeEffectPool = new Pool<ComponentElement>(new PrefabFactory<ComponentElement>(grenadeEffectPrefab, "xaxa"), 10);
    }

    public ComponentElement Test(PoolType pool)
    {
        ComponentElement element = grenadeEffectPool.Allocate();
        element.Initialize(() => grenadeEffectPool.Release(element));
        
        return element;
    }
}
