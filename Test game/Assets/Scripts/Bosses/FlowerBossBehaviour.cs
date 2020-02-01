using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBossBehaviour : BaseBossBehaviour
{
    protected Animator animator;

    protected override void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        
    }

    public void SpawnAttack()
    {

    }
}
