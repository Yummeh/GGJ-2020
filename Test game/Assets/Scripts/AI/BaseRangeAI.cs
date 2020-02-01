using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangeAI : BaseAI
{
    [SerializeField] private GameObject bulletPrefab;

    private BulletBase[] bulletInstances = new BulletBase[20];

    protected override void Start()
    {
        base.Start();

        if (bulletPrefab != null)
        {
            for (int i = 0; i < bulletInstances.Length; i++)
            {
                bulletInstances[i] = Instantiate(bulletPrefab, transform).GetComponent<BulletBase>();
                bulletInstances[i].gameObject.SetActive(false);
            }
        }
        else
            Debug.LogError("Bullet has not been assigned in the inspector for: " + this);
    }

    protected override void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer > attackReloadTime)
        {
            attackTimer = 0;
            Shoot();
        }

        CheckForEnemyCloseBy();

        velocity *= 0.8f;
    }

    protected virtual void Shoot()
    {
        SpawnBullet();
    }

    protected void SpawnBullet()
    {
        // Error out
        if (bulletPrefab == null) return;

        int newBullet = GetBulletInstance();
        if (newBullet != -1)
        {
            bulletInstances[newBullet].transform.position = transform.position;
            bulletInstances[newBullet].bulletOwner = gameObject;
            bulletInstances[newBullet].direction = (manager.player.transform.position - transform.position).normalized;
            bulletInstances[newBullet].gameObject.SetActive(true);
        }
    }

    protected int GetBulletInstance()
    {
        for (int i = 0; i < bulletInstances.Length; i++)
        {
            if (!bulletInstances[i].gameObject.activeSelf)
                return i;
        }
        return -1;
    }

    protected override void Update()
    {
        base.Update();
    }
}
