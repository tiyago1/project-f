using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Boss : MonoBehaviour
{
    public int Health;

    [SerializeField] private Animator animator;

    public int phaseIndex;

    private void Start()
    {
        phaseIndex = -1;
        Health = 10;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        phaseIndex++;
        if (phaseIndex == 1)
        {
            animator.SetTrigger("p1");
        }
        else if (phaseIndex == 2)
        {
            animator.SetTrigger("p2");
        }
    }
}
