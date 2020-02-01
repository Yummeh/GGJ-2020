using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [SerializeField] protected AIState state;

    // Speed info
    [SerializeField] protected float maxSpeed = 3;
    [SerializeField] protected float accelerationChangeMultiplier = 2;
    protected Vector3 velocity;

    // Stats
    [SerializeField] protected int health = 3;
    [SerializeField] protected float sightRange = 2;
    [SerializeField] protected float attackRange = 0.3f;

    // Wander variables.
    protected float wanderTimer = 0;
    protected float changeDirectionWanderTime = 1;
    protected Vector3 randomDirection;

    // Flee variables
    protected GameObject recievedDamageFrom;
    protected float fleeTimer;
    protected float fleeMaxTime = 3;

    // Attack variables
    protected float attackTimer;
    protected float attackReloadTime = 1;

    // Charge variables
    protected GameObject closeByEntity;

    // References
    protected AIManager manager;

    // Fleeing behaviour
    protected virtual void Flee()
    {
        fleeTimer += Time.deltaTime;

        velocity += (transform.position - recievedDamageFrom.transform.position).normalized * maxSpeed * Time.deltaTime * accelerationChangeMultiplier;

        // Go back to wandering when the flee time has run out
        if (fleeTimer > fleeMaxTime)
            state = AIState.Wandering;
    }

    // Wandering behaviour
    protected virtual void Wander()
    {
        wanderTimer += Time.deltaTime;

        if (wanderTimer > changeDirectionWanderTime)
        {
            wanderTimer = 0;
            changeDirectionWanderTime = Random.Range(1f, 3f);
            randomDirection = new Vector3(Random.Range(-maxSpeed, maxSpeed), Random.Range(-maxSpeed, maxSpeed), 0).normalized;
        }
        velocity += randomDirection * Time.deltaTime * accelerationChangeMultiplier;

        CheckForEnemyCloseBy();
    }

    // Charge Behaviour 
    protected virtual void ChargeTowardsEntity()
    {
        CheckForEnemyCloseBy();
        if (closeByEntity == null)
        {
            state = AIState.Wandering;
            return;
        }

        velocity += (closeByEntity.transform.position - transform.position).normalized * maxSpeed * Time.deltaTime * accelerationChangeMultiplier;
    }

    // Check if an enemy is close by
    protected void CheckForEnemyCloseBy()
    {
        closeByEntity = null;
        foreach (GameObject entity in manager.attackableEntities)
        {
            float distanceToEntity = Vector3.Distance(entity.transform.position, transform.position);
            if (distanceToEntity < sightRange)
            {
                closeByEntity = entity;
                state = AIState.Charging;

                if (distanceToEntity < attackRange)
                    state = AIState.Attacking;

                break;
            }
        }
    }

    protected virtual void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackReloadTime)
            attackTimer = 0;

        CheckForEnemyCloseBy();

        velocity *= 0.8f;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    protected virtual void Start()
    {
        manager = FindObjectOfType<AIManager>();
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case AIState.Wandering:
                Wander();
                break;
            case AIState.Fleeing:
                Flee();
                break;
            case AIState.Charging:
                ChargeTowardsEntity();
                break;
            case AIState.Attacking:
                Attack();
                break;
        }

        // Check if this AI is dead
        if (health <= 0)
            Death();

        // Cap speed
        if (velocity.magnitude > maxSpeed)
            velocity = velocity.normalized * maxSpeed;

        // Set the speed
        transform.position += velocity * Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            recievedDamageFrom = collision.gameObject;
            health--;
        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        velocity = new Vector3(-collision.relativeVelocity.x, -collision.relativeVelocity.y, 0);
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, sightRange);

        Gizmos.color = new Color(1, 0, 0, 0.2f);
        Gizmos.DrawSphere(transform.position, attackRange);
    }
}

// States
public enum AIState
{
    Wandering,
    Fleeing,
    Attacking,
    Charging
}
