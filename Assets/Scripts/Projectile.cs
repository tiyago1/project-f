using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Projectile : MonoBehaviour, IResettable
{
    [SerializeField] private ProjectileModel model;

    private Rigidbody2D rigidbody;

    public event Action<Projectile> OnBlowUp;

    public void Init(ProjectileModel model)
    {
        rigidbody = this.GetComponent<Rigidbody2D>();
        this.model = model;
    }

    public void Force(Vector2 direction)
    {
        model.Direction = direction;
        rigidbody.AddForce(model.Direction * model.ForceValue);
        StartCoroutine(DisappearTimer());
    }

    private IEnumerator DisappearTimer()
    {
        float waitTime = Random.Range(model.StartDuration, model.EndDuration);
        yield return new WaitForSecondsRealtime(waitTime);
        OnBlowUp?.Invoke(this);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        OnBlowUp?.Invoke(this);
    }

    public void Reset()
    {
        this.gameObject.SetActive(false);
        rigidbody.velocity = Vector2.zero;
    }
}
