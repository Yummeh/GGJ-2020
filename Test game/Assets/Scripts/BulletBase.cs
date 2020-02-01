using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected float maxSpeed = 3;
    [HideInInspector] public GameObject bulletOwner;
    [HideInInspector] public Vector3 direction;

    protected virtual void OnEnable()
    {
        if(direction != Vector3.zero)
            transform.localRotation = Quaternion.LookRotation(direction, -Vector3.up);

        transform.Rotate(-Vector3.up, 90);
    }

    protected virtual void Update()
    {
        transform.position += direction * maxSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.transform.CompareTag("Player"))
        {
            // Do damage to enemy
        }

        if(bulletOwner != collider.gameObject)
            gameObject.SetActive(false);
    }
}
