using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoTurret : MonoBehaviour
{
    [SerializeField] private GameObject multiProjectilePrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float fireRate;
    [SerializeField] private float force;
    [SerializeField] private Transform target;

    private SpriteRenderer renderer;

    public Pool<Projectile> projectilePool;
    public Pool<MultiProjectile> multiProjectilePool;

    public Pool<Bullet> bulletFactSory;

    [SerializeField] ProjectileModel multiProjectileModel;
    [SerializeField] ProjectileModel projectileModel;

    private void Awake()
    {
        renderer = this.GetComponent<SpriteRenderer>();

        multiProjectilePool = new Pool<MultiProjectile>(new PrefabFactory<MultiProjectile>(multiProjectilePrefab, "BossMultiProjectilePool"), 2);
        projectilePool = new Pool<Projectile>(new PrefabFactory<Projectile>(projectilePrefab,"BossProjectilePool"), 8);
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(fireRate);
        StartCoroutine(LoopMulti());
    }
    private IEnumerator LoopMulti()
    {
        while (true)
        {
            Vector2 direction = (target.transform.position - this.transform.position).normalized;
            MultiProjectile multiProjectile = multiProjectilePool.Allocate();
            multiProjectile.transform.position = this.transform.position;
            multiProjectile.Init(multiProjectileModel);
            multiProjectile.OnBlowUp += MultiProjectile_OnBlowUp;
            multiProjectile.gameObject.SetActive(true);
            multiProjectile.Force(direction);
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void MultiProjectile_OnBlowUp(List<ProjectileModel> projetileModels, MultiProjectile multiProjectile)
    {
        multiProjectile.OnBlowUp -= MultiProjectile_OnBlowUp;
        multiProjectilePool.Release(multiProjectile);

        foreach (ProjectileModel model in projetileModels)
        {
            Projectile projectile = projectilePool.Allocate();
            projectile.transform.position = multiProjectile.transform.position;
            projectile.Init(model);
            projectile.OnBlowUp += Projectile_OnBlowUp;
            projectile.gameObject.SetActive(true);
            projectile.Force(model.Direction);
        }
    }

    private void Projectile_OnBlowUp(Projectile projectile)
    {
        projectilePool.Release(projectile);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        this.transform.DOPunchScale(new Vector3(2.1f, 2.1f, 2.1f), .2f);
        renderer.DOColor(new Color(.5f, .5f, .5f), .1f).OnComplete(() => renderer.DOColor(new Color(1, 1, 1), .1f));
    }
}
