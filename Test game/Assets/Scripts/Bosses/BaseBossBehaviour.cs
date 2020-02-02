using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossBehaviour : MonoBehaviour
{
    protected Animator animator;
    protected PlayerInfo player;
    [SerializeField] protected GameObject tentacle;
    protected SpriteRenderer renderer;

    [SerializeField] protected float activationRange = 20;

    protected int mainHealth;
    [SerializeField] protected int defaultHealth = 10;
    protected bool vunerable = false;

    protected float attackTime;
    [SerializeField] protected float attackTimer = 3;

    protected float vunerableTime;
    [SerializeField] protected float vunerableTimer = 8;

    [SerializeField] protected GameObject bossItemDrop;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerInfo>();
        renderer = GetComponent<SpriteRenderer>();

        mainHealth = defaultHealth;
    }

    protected virtual void Update()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < activationRange)
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
        // Dont start battle when player is not close enough
        else
            mainHealth = defaultHealth;
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
            renderer.color = Color.Lerp(renderer.color, Color.white, Time.deltaTime * 2);
            yield return null;
        }
    }

    private void SpawnBossDrop()
    {
        // Error out
        if(bossItemDrop == null) { Debug.LogError("Boss item was not given, item wont spawn"); return; }
            
        GameObject itemDrop = Instantiate(bossItemDrop);
        itemDrop.transform.position += Vector3.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0,0,1,0.2f);
        Gizmos.DrawSphere(transform.position, activationRange);
    }
}
