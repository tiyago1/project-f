using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : MonoBehaviour
{
    [SerializeField] private GameObject grenadePrefab;
    [SerializeField] private GameObject grenadeEffectPrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private Transform target;

    public Pool<Grenade> grenadePool;
    public Pool<GrenadeEffect> grenadeEffectPool;

    private void Awake()
    {
        grenadePool = new Pool<Grenade>(new PrefabFactory<Grenade>(grenadePrefab, "BossGranadePool"), 4);
        grenadeEffectPool = new Pool<GrenadeEffect>(new PrefabFactory<GrenadeEffect>(grenadeEffectPrefab, "BossGranadeEffectPool"), 4);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(Launch());
            //Test();
        }
    }

    private IEnumerator Launch()
    {
        while (true)
        {
            Test();
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void Test()
    {
        Vector2 direction = (target.transform.position - this.transform.position).normalized;
        GrenadeEffect effect = grenadeEffectPool.Allocate();
        effect.Initialize(() => grenadeEffectPool.Release(effect));
        Grenade granede = grenadePool.Allocate();
        granede.transform.position = this.transform.position;
        granede.gameObject.SetActive(true);
        granede.Launch(effect, target.transform.position, () => grenadePool.Release(granede));
    }

}
