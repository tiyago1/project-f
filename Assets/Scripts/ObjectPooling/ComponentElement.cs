using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentElement : MonoBehaviour,IResettable
{
    private Action releaseAction;
    public void Initialize(Action releaseAction)
    {
        this.releaseAction = releaseAction;
    }

    public void Release()
    {
        releaseAction?.Invoke();
    }

    public void Reset()
    {
        gameObject.SetActive(false);
    }

}
