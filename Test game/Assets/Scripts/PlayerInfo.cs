using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int health { get; private set; }
    public int partsCollected { get; private set; }

    private AbilityManager abilities;
    [SerializeField] private GameObject swordObject;

    void Start()
    {
        abilities = GetComponent<AbilityManager>();
        swordObject.SetActive(false);
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

    // Called on unlocking something
    public void OnUnlock(Unlock unlock)
    {
        switch (unlock)
        {
            case Unlock.Part1:
                partsCollected++;
                break;
            case Unlock.Part2:
                partsCollected++;
                break;
            case Unlock.Part3:
                partsCollected++;
                break;
            case Unlock.Sword:
                swordObject.SetActive(true);
                break;
            case Unlock.AbilityDisk:
                abilities.UnlockAbility(0);
                break;
            case Unlock.AbilityWhip:
                abilities.UnlockAbility(1);
                break;
        }
    }
}
