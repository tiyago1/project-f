using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour, IResettable
{
    [SerializeField] private float startDuration;
    [SerializeField] private float endDuration;
    [SerializeField] private float xDuration;
    [SerializeField] private float forceValue;
    [SerializeField] private Vector2 direction;

    [SerializeField] private List<Bullet> childBullets;
    [SerializeField] private bool haveChild;

    private Rigidbody2D rigidbody;
    private Action releaseAction;
    private Transform parent;

    private void Awake()
    {
        childBullets = this.GetComponentsInChildren<Bullet>().ToList();
        childBullets.Remove(this);
        childBullets.ForEach(it => it.name = this.gameObject.name + "_" + it.name);
        rigidbody = this.GetComponent<Rigidbody2D>();
        parent = this.transform.parent;

        if (!haveChild)
        {
            Init(() => Reset());
        }
        StopCoroutine(Loop());
    }

    public void Init(Action releaseAction)
    {
        this.releaseAction = releaseAction;
    }

    private void Start()
    {
        StartCoroutine(Loop());
    }

    private IEnumerator Loop()
    {
        if (haveChild)
        {
            yield return new WaitForSeconds(Random.Range(startDuration, endDuration));
            childBullets.ForEach(it => it.Force());
        }

        yield return new WaitForSeconds(xDuration);
        releaseAction();
    }

    public void Force()
    {
        rigidbody.transform.localPosition = Vector3.zero;
        rigidbody.transform.parent = null;
        rigidbody.gameObject.SetActive(true);
        Force(direction, forceValue);
    }

    public void Force(Vector2 direction, float forceValue)
    {
        rigidbody.AddForce(direction * forceValue);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        releaseAction();
    }

    public void Reset()
    {
        rigidbody.velocity = Vector2.zero;

        //if (haveChild)
        //{
        //    childBullets.ForEach(it => {
        //        it.gameObject.SetActive(true);
        //        it.transform.SetParent(parent);
        //        it.gameObject.SetActive(false);
        //    });
        //}

        this.transform.SetParent(parent);
        this.gameObject.SetActive(false);
    }

    private void ResetChild()
    {

    }
}
