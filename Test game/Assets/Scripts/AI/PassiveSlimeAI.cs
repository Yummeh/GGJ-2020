using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveSlimeAI : BaseAI
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (collision.CompareTag("Weapon"))
        {
            fleeTimer = 0;
            state = AIState.Fleeing;
        }
    }
}
