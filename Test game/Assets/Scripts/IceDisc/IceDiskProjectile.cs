using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D), typeof(WeaponInfo))]
public class IceDiskProjectile : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D body;
    public bool returning { get; private set; }

    public Vector3 startingPosition;
    public Vector3 startingVelocity;
    public GameObject returnTo;
    public float maxDistance = 10f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.Play("IceDisk");
        body = GetComponent<Rigidbody2D>();
        returning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!returning)
        {
            Vector3 delta = transform.position - startingPosition;
            delta.z = 0f;

            if (delta.magnitude > maxDistance)
            {
                returning = true;
            }

            body.velocity = startingVelocity;
        }
        else
        {
            Vector3 delta = returnTo.transform.position - transform.position;
            delta.z = 0f;
            delta.Normalize();

            body.velocity = delta * startingVelocity.magnitude;
        }
    }
}
