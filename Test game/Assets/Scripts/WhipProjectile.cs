using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class WhipProjectile : MonoBehaviour
{
    public enum State
    {
        Flying,
        Returning,
        StuckOnSolid,
        StuckOnEntity,
    }

    public State state { get; private set; }
    private Rigidbody2D body;
    private BoxCollider2D coll;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float returnSpeed = 10f;

    // Whip set by whip ability class
    public AbilityWhip whip;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Flying;
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        Vector3 delta = whip.transform.position - transform.position;
        delta.z = 0;
        if (delta.magnitude > maxDistance)
        {
            state = State.Returning;
            whip.OnRopeConnect(this);
        }

        if (state == State.Returning)
        {
            Vector3 deltaNorm = delta;
            deltaNorm.Normalize();
            body.velocity = new Vector2(deltaNorm.x, deltaNorm.y) * returnSpeed; // * (dis * returnDistStrnScalar);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == whip.gameObject)
        {
            return;
        }
        
        if (state == State.Flying)
        {
            BaseAI ai = collision.gameObject.GetComponent<BaseAI>();
            if (ai != null)
            {
                // Enemy hit!
                state = State.StuckOnEntity;

                // Reset velocity
                body.velocity = new Vector2(0f, 0f);

                // Callback
                whip.OnRopeConnect(this);
            }
            else
            {
                // Wall hit
                state = State.StuckOnSolid;
                
                // Stop physics
                body.isKinematic = true;
                body.velocity = new Vector2(0f, 0f);

                // Callback
                whip.OnRopeConnect(this);
            }
        }
    }

}
