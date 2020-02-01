using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    [SerializeField] protected AIState state;

    // Speed info
    [SerializeField] protected float maxSpeed = 3;
    [SerializeField] protected float accelerationChangeMultiplier = 2;
    protected float timeWithMultiplier;
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
    [SerializeField] protected float fleeMaxTime = 3;

    // Attack variables
    protected float attackTimer;
    [SerializeField] protected float attackReloadTime = 1;

    // Charge variables
    protected GameObject closeByEntity;

    // Hurt variables
    protected bool invincible = false;
    protected float invincibleTimer = 0f;
    [SerializeField] protected float invincibleDuration = 0.5f;
    protected float knockbackTimer = 0f;
    protected float knockbackReduction = 0.5f;
    private AIState preKnockbackState = AIState.Attacking;

    // References
    protected AIManager manager;
    protected Rigidbody2D rb;

    #region Behaviours

    // Fleeing behaviour
    protected virtual void Flee()
    {
        fleeTimer += Time.deltaTime;

        velocity += (transform.position - recievedDamageFrom.transform.position).normalized * timeWithMultiplier;

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
            randomDirection = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0).normalized;
        }
        velocity += randomDirection * timeWithMultiplier;

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

        velocity += (closeByEntity.transform.position - transform.position).normalized * timeWithMultiplier;
    }

    protected virtual void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackReloadTime)
            attackTimer = 0;

        CheckForEnemyCloseBy();

        velocity *= 0.8f;
    }

    protected virtual void Hurt()
    {
        if (knockbackTimer >= 0f)
        {
            Vector3 dir = velocity;
            dir.Normalize();
            dir *= knockbackReduction * Time.deltaTime;
            if (velocity.magnitude > dir.magnitude)
            {
                velocity -= dir;
            }
            else
            {
                velocity = new Vector3(0f, 0f, 0f);
            }
        }
        else
        {
            state = preKnockbackState;
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    #endregion

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

    // Check collision with boundary
    protected void CheckCollisionWithBoundaries()
    {
        Vector3 boundaryForce = manager.boundary.ForceInsideBounds(transform.position, true);
        boundaryForce += AvoidPoints();

        if (boundaryForce != Vector3.zero)
        {
            if (state != AIState.Wandering)
                velocity += boundaryForce * timeWithMultiplier;

            randomDirection = boundaryForce;
            wanderTimer = 0;
        }
    }

    // Avoid points provide by the manager
    private Vector3 AvoidPoints()
    {
        for (int iPoint = 0; iPoint < manager.avoidPoints.Count; iPoint++)
        {
            // Get point from index
            AvoidPoint point = manager.avoidPoints[iPoint];

            // Calculate the distance the fish is from the avoid point
            float distance = Vector3.Distance(transform.position, point.GetPos());
            if (distance < point.radius)
            {
                // Add force in the opposite direction of the point
                Vector3 direction = (transform.position - point.GetPos()).normalized;
                return direction * point.strength;
            }
        }

        return Vector3.zero;
    }

    private void UpdateInvincibility()
    {
        if (invincibleTimer >= 0f)
        {
            invincibleTimer -= Time.deltaTime;
        }
        else
        {
            invincible = false;
        }
    }

    public void MakeInvincible(float duration)
    {
        invincible = true;
        invincibleTimer = duration;
    }

    public virtual void Knockback(Vector3 knockback, float duration, float velReduction)
    {
        knockback.z = 0;
        velocity = knockback;
        knockbackTimer = duration;
        knockbackReduction = velReduction;
        preKnockbackState = state;
        state = AIState.Hurt;
    }

    protected virtual void Start()
    {
        manager = FindObjectOfType<AIManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        timeWithMultiplier = Time.deltaTime * accelerationChangeMultiplier;

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
            case AIState.Hurt:
                Hurt();
                break;
        }

        CheckCollisionWithBoundaries();
        // Update the invincibility
        UpdateInvincibility();

        // Check if this AI is dead
        if (health <= 0)
            Death();

        // Cap speed (but not during knockback)
        if (velocity.magnitude > maxSpeed && state != AIState.Hurt)
            velocity = velocity.normalized * maxSpeed;

        // Set the speed
        rb.velocity = velocity;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            if (!invincible)
            {
                // Get properties
                WeaponInfo weapon = collision.GetComponent<WeaponInfo>();

                // Knockback
                Transform root = collision.transform.root;
                if (root == null)
                {
                    root = collision.transform;
                }
                Vector3 diff = transform.position - root.position;
                if (diff.magnitude > 0.05f)
                {
                    diff.Normalize();

                    if (weapon != null)
                    {
                        Knockback(diff * weapon.knockbackSpeed, weapon.knockbackDuration, weapon.knockbackOverTime);
                    }
                    else
                    {
                        Knockback(diff * 5f, 1.5f, 5f / 1.5f);
                    }
                }

                // Invincibility
                if (weapon != null)
                {
                    MakeInvincible(weapon.invincibilityDuration);
                }
                else
                {
                    MakeInvincible(invincibleDuration);
                }

                // Handle hit
                recievedDamageFrom = collision.gameObject;
                health--;
            }
        }
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
    Charging,
    Hurt,
}
