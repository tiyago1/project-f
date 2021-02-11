using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float speed = 500f;
    public void Update()
    {
        this.transform.Rotate(Vector3.forward* Time.deltaTime * speed);
    }
}
