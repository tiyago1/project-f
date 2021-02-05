using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private Transform headStartHolder;
    [SerializeField] private Rigidbody2D rigidbody;

    public void Force(Vector2 direction, float value)
    {
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
        this.gameObject.SetActive(false);
    }
}
