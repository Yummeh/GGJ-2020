using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerTentacle : MonoBehaviour
{
    private FlowerBossBehaviour boss;
    protected Animator animator;
    protected BoxCollider2D collider;
    protected SpriteRenderer renderer;

    // Timer
    private float waitTime = 0;
    [SerializeField] private float waitTimer = 3;
    private float despawnTime = 0;
    [SerializeField] private float despawnTimer = 5;
    private bool spawned = false;
    private bool canGiveDamage = true;

    // Stats
    [SerializeField] private int health = 3;

    private void Start()
    {
        boss = GetComponentInParent<FlowerBossBehaviour>();
        animator = GetComponent<Animator>();
        collider = GetComponent<BoxCollider2D>();
        renderer = GetComponent<SpriteRenderer>();

        collider.enabled = false;
    }

    private void Update()
    {
        if (spawned)
        {
            despawnTime += Time.deltaTime;
            if (despawnTime > despawnTimer)
                Despawn();
        }
        else
        {
            waitTime += Time.deltaTime;
            if (waitTime > waitTimer)
                Spawn();
        }
    }

    private void Spawn()
    {
        animator.SetTrigger("Spawn");
        spawned = true;
        collider.enabled = true;
    }

    private void Despawn()
    {
        animator.SetTrigger("Spawn");
    }

    // Animation event
    private void DestoyObject()
    {
        Destroy(gameObject);
    }

    // Animation event
    private void DoneSpawning()
    {
        canGiveDamage = false;
    }

    private void MakeBossVunerable()
    {
        boss.SetVunerable(true);

        Despawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Weapon"))
        {
            health--;
            StartCoroutine(FlashDamage());
            if (health <= 0)
                MakeBossVunerable();
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canGiveDamage)
        {
            boss.GiveDamageToPlayer(1);
            canGiveDamage = false;
        }
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
}
