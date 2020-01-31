using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    // To indicate if the player is able to move
    public enum MovementState
    {
        // Player is able to move around
        Moving,

        // Player has no control over movement
        Knockback,
    }

    // Other components
    Rigidbody2D characterController;

    public MovementState movementState { get; private set; }

    // Knockback
    private float knockbackTimer = 0f;
    private float knockbackVelChange = 0f;

    public Vector2 velocity;
    public Vector2 moveDirection { get; private set; }

    // Movement
    public float movementSpeed = 100f;

    void Start()
    {
        characterController = GetComponent<Rigidbody2D>();
        movementState = MovementState.Moving;
        velocity = new Vector2(0f, 0f);
        moveDirection = new Vector2(1f, 0f);
    }

    void FixedUpdate()
    {
        switch (movementState)
        {
            case MovementState.Moving:
                {
                    float xAxis = Input.GetAxisRaw("Horizontal");
                    float yAxis = Input.GetAxisRaw("Vertical");
                    Vector2 asVec = new Vector2(xAxis, yAxis);

                    // Normalize if too big
                    if (asVec.magnitude > 1f)
                    {
                        asVec.Normalize();
                    }
                    moveDirection = asVec;

                    // Perform movement
                    velocity = moveDirection * movementSpeed;
                }
                break;
            case MovementState.Knockback:
                {
                    if (knockbackTimer >= 0f)
                    {
                        // Decrease timer
                        knockbackTimer -= Time.deltaTime;

                        // Change knockback value
                        Vector2 velNorm = velocity;
                        velNorm.Normalize();
                        float change = knockbackVelChange * Time.deltaTime;
                        if (velocity.magnitude > change)
                        {
                            velocity -= velNorm * change;
                        }
                        else
                        {
                            velocity = new Vector2(0f, 0f);
                        }
                    }
                    else
                    {
                        movementState = MovementState.Moving;
                    }
                }
                break;
        }

        // Perform actual movement
        characterController.velocity = velocity;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public void ApplyKnockback(Vector2 kvelocity, float duration, float velChange = 0f)
    {
        movementState = MovementState.Knockback;
        velocity = kvelocity;
        knockbackTimer = duration;
        knockbackVelChange = velChange;
    }
}
