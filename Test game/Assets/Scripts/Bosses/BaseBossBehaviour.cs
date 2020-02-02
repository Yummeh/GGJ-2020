using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected PlayerInfo player;
    [SerializeField] protected GameObject tentacle;

    protected int mainHealth = 10;
    protected bool vunerable = false;

    protected float attackTime;
    protected float attackTimer = 3;

    protected float vunerableTime;
    protected float vunerableTimer = 8;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerInfo>();
    }

    protected virtual void Update()
    {
        if (!vunerable)
        {
            attackTime += Time.deltaTime;

            if (attackTime > attackTimer)
            {
                attackTime = 0;
                SpawnAttack();
            }
        }
        else
        {
            vunerableTime += Time.deltaTime;

            if (vunerableTime > vunerableTimer)
            {
                vunerableTime = 0;
                SetVunerable(false);
            }
        }
    }

    protected virtual void SpawnAttack() {}

    protected virtual void OnHit()
    {
        mainHealth--;

        if (mainHealth <= 0)
            animator.SetTrigger("Death");
    }

    public void SetVunerable(bool state)
    {
        vunerable = state;
        animator.SetBool("Vunerable", state);
    }

    public void GiveDamageToPlayer(int damage)
    {
        player.DealDamage(damage);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            OnHit();
        }
    }
}
