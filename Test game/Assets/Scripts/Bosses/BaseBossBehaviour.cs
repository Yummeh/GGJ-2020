using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected PlayerInfo player;
    [SerializeField] protected GameObject tentacle;
    protected SpriteRenderer renderer;

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
        renderer = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update()
    {
        if (!vunerable)
        {
            attackTime += Time.deltaTime;

            if (attackTime > attackTimer && mainHealth > 0)
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
        StartCoroutine(FlashDamage());

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

    // Animation event
    private void DestoyObject()
    {
        Destroy(gameObject);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon") && vunerable)
            OnHit();
    }

    private IEnumerator FlashDamage()
    {
        renderer.color = Color.red;
        while (renderer.color.r > 0.01f)
        {
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime);
            yield return null;
        }
    }
}
