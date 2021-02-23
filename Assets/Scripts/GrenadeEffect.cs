using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeEffect : MonoBehaviour, IResettable
{
    private Action callBack;
    private Animator animator;
    public float EffectDuration => animator.GetCurrentAnimatorClipInfo(0).Length;

    private bool isDamageTaken;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void Initialize(Action callBack)
    {
        this.callBack = callBack;
    }

    public void Launch(Vector3 newPosition)
    {
        Debug.Log("GrenadeEffect_Launch");
        this.gameObject.SetActive(true);
        this.transform.position = newPosition;
    }

    public IEnumerator _EffectFinished()
    {
        Debug.Log("_EffectFinished");
        yield return new WaitForEndOfFrame();
        callBack?.Invoke();
    }

    public void Reset()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (!isDamageTaken && player != null)
        {
            Debug.LogError("BOOOMMMMM!!!!!");
            isDamageTaken = true;
        }
    }

}
