using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Projectile))]
public class MultiProjectile : MonoBehaviour, IResettable
{
    [SerializeField] private List<ProjectileModel> projectileModels;
    private Projectile projectile;
    public event Action<List<ProjectileModel>, MultiProjectile> OnBlowUp;

    public void Init(ProjectileModel model)
    {
        projectile = this.GetComponent<Projectile>();
        projectile.Init(model);
    }

    public void Force(Vector2 direction)
    {
        projectile.OnBlowUp += Projectile_OnBlowUp;
        projectile.Force(direction);
    }

    private void Projectile_OnBlowUp(Projectile projectile)
    {
        projectile.OnBlowUp -= Projectile_OnBlowUp;
        OnBlowUp?.Invoke(projectileModels, this);
    }

    public void Reset()
    {
        this.gameObject.SetActive(false);
    }
}
