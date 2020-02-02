using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWhip : AbilityBase
{
    public enum State
    {
        Inactive,
        Shooting,
        Returning,
        LatchSurface,
        LatchEnemy,
    }

    // Comps
    private PlayerMovement playerMovement;

    public State state { get; private set; }
    [SerializeField] private GameObject whipProjectileType;
    [SerializeField] private GameObject whipRopeType;
    private GameObject whipProjectile = null;
    private GameObject whipRope = null;
    private WhipProjectile whipProjectileComp = null;

    [SerializeField] private float whipSpeed = 5f;
    [SerializeField] private float whipMoveToSpeed = 5f;
    [SerializeField] private float whipMoveToDuration = 5f;
    [SerializeField] private float whipPullDuration = 5f;
    [SerializeField] private float whipPickupRange = 0.8f;
    [SerializeField] private float whipPullPickupRange = 2.5f;
    private float movePullTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        state = State.Inactive;
        playerMovement = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRopeRender(transform.position);

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 delta = mouseWorldPos - transform.position;
            delta.z = 0f;
            delta.Normalize();
            Use(delta);
        }

        // Update movement towards or from the rope
        UpdateMovement();
    }

    public override bool IsActive()
    {
        return state != State.Inactive;
    }

    public override void Use(Vector2 direction)
    {
        if (!IsActive())
        {
            // Set new state
            state = State.Shooting;

            // Create new gameobjects
            whipProjectile = Instantiate(whipProjectileType, transform.position, Quaternion.identity);
            whipRope = Instantiate(whipRopeType, transform.position, Quaternion.identity);
            whipProjectileComp = whipProjectile.GetComponent<WhipProjectile>();

            // Set speed
            whipProjectile.GetComponent<Rigidbody2D>().velocity = direction * whipSpeed;
            WhipProjectile projComp = whipProjectile.GetComponent<WhipProjectile>();
            projComp.whip = this;
        }
    }

    void UpdateRopeRender(Vector3 startPos)
    {
        if (whipProjectile != null && whipRope != null)
        {
            Vector3 delta = whipProjectile.transform.position - startPos;
            Vector3 deltaNorm = delta;
            deltaNorm.z = 0f;
            deltaNorm.Normalize();
            float angle = Mathf.Atan2(deltaNorm.x, -deltaNorm.y) * Mathf.Rad2Deg - 90f;

            Vector3 lenDelta = new Vector3(delta.x, delta.y, 0f);
            whipRope.transform.position = startPos + delta / 2f;
            whipRope.transform.localScale = new Vector3(delta.magnitude, 1f, 1f);
            whipRope.transform.localEulerAngles = new Vector3(0f, 0f, angle);

            whipProjectile.transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }
    
    public void OnRopeConnect(WhipProjectile whipProj)
    {
        if (whipProj.state == WhipProjectile.State.StuckOnEntity)
        {
            state = State.LatchEnemy;
            movePullTimer = whipPullDuration;
        }
        else if (whipProj.state == WhipProjectile.State.StuckOnSolid)
        {
            state = State.LatchSurface;
            movePullTimer = whipMoveToDuration;
            playerMovement.StartCustomMovement();
        }
        else if (whipProj.state == WhipProjectile.State.Returning)
        {
            state = State.Returning;
            movePullTimer = whipMoveToDuration;
        }
    }

    void UpdateMovement()
    {
        if (whipProjectile == null ||
            whipRope == null || 
            whipProjectileComp == null)
        {
            return;
        }

        if (state == State.LatchEnemy || state == State.LatchSurface || state == State.Returning)
        {
            if (movePullTimer >= 0f)
            {
                // Timer
                movePullTimer -= Time.deltaTime;

                Vector3 delta = whipProjectile.transform.position - transform.position;
                delta.z = 0;
                if (state == State.LatchSurface)
                {
                    Vector3 deltaNorm = delta;
                    deltaNorm.Normalize();

                    playerMovement.velocity = deltaNorm * whipMoveToSpeed;
                }
                else if (state == State.LatchEnemy)
                {
                    if (delta.magnitude < whipPullPickupRange)
                    {
                        EndWhip();
                    }
                }

                if (delta.magnitude < whipPickupRange)
                {
                    EndWhip();
                }
            }
            else
            {
                EndWhip();
            }
        }
    }

    void EndWhip()
    {
        Destroy(whipProjectile);
        Destroy(whipRope);
        whipProjectile = null;
        whipRope = null;
        state = State.Inactive;
        playerMovement.EndCustomMovement();
    }
}
