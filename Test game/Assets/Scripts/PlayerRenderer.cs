using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class PlayerRenderer : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer sprite;
    private PlayerMovement parentBody;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        parentBody = gameObject.GetComponentInParent<PlayerMovement>();
        if (parentBody == null)
        {
            enabled = false;
            print("could not find PlayerMovement");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (parentBody.movementState == PlayerMovement.MovementState.Moving)
        {
            if (parentBody.velocity.magnitude > 0.1f)
            {
                animator.SetTrigger("ToWalk");
            }
            else
            {
                animator.SetTrigger("ToIdle");
            }
        }

        if (parentBody.velocity.x > 0.1f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (parentBody.velocity.x < -0.1f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
}
