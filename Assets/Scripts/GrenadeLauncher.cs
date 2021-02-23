using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject grenadeEffectPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform target;
    [SerializeField] private AreaPoints areaPoints;
    [SerializeField] private int grenadeMaxCount;

    public Pool<Grenade> grenadePool;
    public Pool<GrenadeEffect> grenadeEffectPool;
    private int firedGrenadeCount;

    private void Awake()
    {
        grenadePool = new Pool<Grenade>(new PrefabFactory<Grenade>(grenadePrefab, "BossGranadePool"), 4);
        grenadeEffectPool = new Pool<GrenadeEffect>(new PrefabFactory<GrenadeEffect>(grenadeEffectPrefab, "BossGranadeEffectPool"), 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            StartCoroutine(Launch());
        }
    }

    private IEnumerator Launch()
    {
        while (firedGrenadeCount <= grenadeMaxCount)
        {
            firedGrenadeCount++;
            GrenadeEffect effect = grenadeEffectPool.Allocate();
            effect.Initialize(() => grenadeEffectPool.Release(effect));
            Grenade granede = grenadePool.Allocate();
            granede.transform.position = this.transform.position;
            granede.gameObject.SetActive(true);
            granede.Launch(effect, areaPoints.GetRandomPoint(target.position), target, () => grenadePool.Release(granede));
            yield return new WaitForSeconds(fireRate);
        }
    }
}
