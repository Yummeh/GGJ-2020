using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer), typeof(BaseAI))]
public class SlimeRenderer : MonoBehaviour
{
    private BaseAI ai;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ai = GetComponent<BaseAI>();

        // Register triggers
        ai.destroyOnDeath = false;
        ai.eventDeath.AddListener(OnDeath);
        ai.eventStateChange.AddListener(OnStateBegin);
    }

    void FixedUpdate()
    {
        if (ai.state != AIState.Hurt)
        {
            if (ai.velocity.x >= 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
    }

    public void OnStateBegin(AIState oldState, AIState newState)
    {
        if (newState == AIState.Charging)
        {
            animator.SetTrigger("ToCharge");
        }
        else if (newState == AIState.Hurt)
        {
            animator.SetTrigger("ToHurt");
        }
        else if (oldState == AIState.Charging || oldState == AIState.Hurt)
        {
            animator.SetTrigger("ToIdle");
        }
    }

    public void OnDeath()
    {
        animator.SetTrigger("ToDeath");
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
