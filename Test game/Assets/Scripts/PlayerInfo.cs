using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int health { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Called when player health reaches 0
    void OnDeath()
    {

    }

    // Deal damage to the player
    public void DealDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            OnDeath();
        }
    }
}
