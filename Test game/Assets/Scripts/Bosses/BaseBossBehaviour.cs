using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossBehaviour : MonoBehaviour
{
    protected int mainHealth = 10;
    protected bool vunerable = false;

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public void OnHit()
    {
        mainHealth--;
    }
}
