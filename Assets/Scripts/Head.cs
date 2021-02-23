using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Head : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Transform headStartHolder;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private LineRenderer lineRenderer;

    [SerializeField] private Transform holder;

    private bool isLineRendererActive;
    private Action onCompleted;

    public bool AlmostEqual(Vector3 v1, Vector3 v2, float precision)
    {
        bool equal = true;

        if (Mathf.Abs(v1.x - v2.x) > precision) equal = false;
        if (Mathf.Abs(v1.y - v2.y) > precision) equal = false;
        if (Mathf.Abs(v1.z - v2.z) > precision) equal = false;

        return equal;
    }

    private void Update()
    {
        if (isLineRendererActive)
        {
            holder.transform.DOMove(headStartHolder.position, .001f);

            this.transform.DOMove(headStartHolder.position, .5f).OnComplete(() => {
                isLineRendererActive = false;
                this.gameObject.SetActive(false);
                onCompleted();
            });

            lineRenderer.SetPositions(new Vector3[] { this.transform.position, holder.transform.position });
        }
    }

    public void Force(Vector2 direction, float value)
    {
        lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
        renderer.flipX = direction.x > 0;
        ResetSetup();
        this.gameObject.SetActive(true);
        rigidbody.AddForce(direction.normalized * value);
        rigidbody.AddTorque(value);
    }

    public void ResetSetup()
    {
        this.transform.position = headStartHolder.position;
        rigidbody.velocity = Vector2.zero;
        rigidbody.angularDrag = 0.05f;
        rigidbody.drag = 0;
        this.gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Grenade grenade = collision.gameObject.GetComponent<Grenade>();
        if (grenade != null)
            grenade.OnDamageTaken();

        rigidbody.angularDrag = 3;
        rigidbody.drag = 3;
    }

    public void GetHeadToMe(Action onCompleted)
    {
        this.onCompleted = onCompleted;
        holder.transform.position = this.transform.position;
        isLineRendererActive = true;
    }
}
