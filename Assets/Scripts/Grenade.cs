using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using Random = UnityEngine.Random;

public class Grenade : MonoBehaviour, IResettable
{
    [SerializeField] private int health = 1;
    [SerializeField] private InteractionArea interactionArea;
    private Animator animator;
    private Action callBack;
    private GrenadeEffect effect;

    private GameObject damageAreaIndicator;
    private bool isDamageable;
    private bool isDamageTaken;
    private bool isClocking;

    private void Awake()
    {
        Debug.Log("AWAKE");
        animator = this.GetComponent<Animator>();
        damageAreaIndicator = this.transform.GetChild(0).gameObject;
        interactionArea.OnTriggerEnter += (Collider2D collision) => SetActiveSlowEffectForPlayer(collision, true);
        interactionArea.OnTriggerExit += (Collider2D collision) => SetActiveSlowEffectForPlayer(collision, false);
    }

    private Coroutine xx;

    public void Launch(GrenadeEffect effect, Vector2 target, Transform playerPosition, Action callBack)
    {
        this.effect = effect;
        this.callBack = callBack;
        TweenerCore<Vector3, Vector3, VectorOptions> moveTween = this.transform.DOMove(target, 1f);

        moveTween.OnStart(() => this.transform.DOScale(3, .5f).OnComplete(() => this.transform.DOScale(2, .5f)));
        moveTween.OnComplete(() =>
        {
            this.transform.DOPunchScale(Vector3.one, .2f);
            Vector3 direction = (this.transform.position - playerPosition.position).normalized;
            Vector2 newPosition = this.transform.position - (direction);
            //Vector2 newPosition = playerPosition;

            xx = StartCoroutine(GrenadeClockingLoop(target, playerPosition, Random.Range(0.2f, 2.5f), 10));
            damageAreaIndicator.SetActive(true);
            animator.SetTrigger("clocking");
        });
    }


    private IEnumerator GrenadeClockingLoop(Vector2 target, Transform playerPosition, float waitTime, int lenght)
    {
        int counter = 0;
        //yield return new WaitForSeconds(waitTime);
        while (counter < lenght)
        {
            Vector2 newPosition = this.transform.position - (GetRandomDirection(playerPosition));
            counter++;
            this.transform.DOKill();
            Tween x = this.transform.DOMove(newPosition, 1f);
            yield return x.WaitForCompletion();
            //yield return new WaitForEndOfFrame();
            //yield return new WaitForSeconds(Random.Range(.1f,.8f));
        }

    }

    public IEnumerator _HearthClockFinished()
    {
        if (xx != null)
            StopCoroutine(xx);
        xx = null;
        this.transform.DOScale(0, .2f);
        yield return new WaitForEndOfFrame();
        isDamageable = true;
        effect.Launch(this.transform.position);
        yield return new WaitForSeconds(effect.EffectDuration);
        callBack();
    }

    private Vector3 GetRandomDirection(Transform playerPosition)
    {
        Vector3 direction = Vector2.zero;
        do
        {
            if (Random.value >= 0.5f)
                direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1));
            else
                direction = (this.transform.position - playerPosition.position).normalized;

        } while (direction == Vector3.zero);

        return direction;
    }

    public void Reset()
    {
        if (xx != null)
            StopCoroutine(xx);
        xx = null;
        isDamageable = false;
        isDamageTaken = false;
        damageAreaIndicator.SetActive(false);
        effect.gameObject.SetActive(false);
        animator.ResetTrigger("clocking");
        this.transform.lossyScale.Set(3, 3, 3);
        this.gameObject.SetActive(false);
    }

    public void OnDamageTaken()
    {
        this.transform.DOPunchScale(Vector3.one, .2f).OnComplete(() => this.transform.DOScale(Vector3.zero, .1f));
        callBack();
    }

    private void SetActiveSlowEffectForPlayer(Collider2D collision, bool isActive)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            if (isActive)
                player.OnGrenadeOverEffectEnter();
            else
                player.OnGrenadeOverEffectExit();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerController player = collision.gameObject.GetComponent<PlayerController>();
        if (player != null)
        {
            player.OnGrenadeOverEffectExit();
        }
    }
}
