using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> abilities;
    [SerializeField]
    private List<bool> abilityUnlocked;

    // Returns the weapon if it exists and is owned
    public GameObject GetAbility(int index)
    {
        if (index < abilities.Count && index >= 0 && index < abilityUnlocked.Count)
        {
            if (abilityUnlocked[index])
            {
                return abilities[index];
            }
        }
        return null;
    }

    // Unlock a weapon in the player
    public void UnlockAbility(int index)
    {
        if (index >= 0 && index < abilityUnlocked.Count)
        {
            abilityUnlocked[index] = true;
        }
    }

    public int GetAblilityCount()
    {
        return abilities.Count;
    }
}
