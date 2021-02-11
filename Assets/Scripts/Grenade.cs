using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;

public class Grenade : MonoBehaviour, IResettable
{
    private Animator animator;
    private Action callBack;
    private GrenadeEffect effect;

    private GameObject damageAreaIndicator;

    private void Awake()
    {
        animator = this.GetComponent<Animator>();
        damageAreaIndicator = this.transform.GetChild(0).gameObject;
    }

    public void Launch(GrenadeEffect effect, Vector2 target, Action callBack)
    {
        this.effect = effect;
        this.callBack = callBack;
        TweenerCore<Vector3, Vector3, VectorOptions> moveTween = this.transform.DOMove(target, 1f);

        moveTween.OnStart(() => this.transform.DOScale(3, .5f).OnComplete(() => this.transform.DOScale(2, .5f)));
        moveTween.OnComplete(() =>
        {
            this.transform.DOPunchScale(Vector3.one, .2f);
            damageAreaIndicator.SetActive(true);
            animator.SetTrigger("clocking");
        });
    }

    public IEnumerator _HearthClockFinished()
    {
        this.transform.DOScale(0, .2f);
        yield return new WaitForEndOfFrame();
        effect.Launch(this.transform.position);
        Debug.Log(effect.EffectDuration);
        yield return new WaitForSeconds(effect.EffectDuration);
        callBack();
    }

    public void Reset()
    {
        damageAreaIndicator.SetActive(false);
        effect.gameObject.SetActive(false);
        animator.ResetTrigger("clocking");
        this.transform.lossyScale.Set(3, 3, 3);
    }
}
