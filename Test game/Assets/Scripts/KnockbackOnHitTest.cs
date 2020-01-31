using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackOnHitTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var comp = collision.gameObject.GetComponent<PlayerMovement>();
        if (comp != null)
        {
            comp.ApplyKnockback(new Vector2(-10f, 0f), 1f, 10f);
        }
    }
}
