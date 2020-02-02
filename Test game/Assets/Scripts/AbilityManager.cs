using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> abilities = new List<GameObject>();
    [SerializeField]
    private List<bool> abilityUnlocked = new List<bool>();
    private int selected = -1;

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

    void SelectAbility(int ind)
    {
        if (ind == selected)
            return;

        if (selected != -1)
        {
            GameObject abilityObj = abilities[selected];
            if (abilityObj)
            {
                AbilityBase ability = abilityObj.GetComponent<AbilityBase>();
                ability.Deactivate();
            }
        }
        selected = ind;
        {
            GameObject abilityObj = abilities[selected];
            if (abilityObj)
            {
                AbilityBase ability = abilityObj.GetComponent<AbilityBase>();
                ability.Activate();
            }
        }
    }

    void Update()
    {
        if (selected == -1)
        {
            // Try to select something
            for (int i = 0; i < abilityUnlocked.Count; i++)
            {
                if (abilityUnlocked[i])
                    SelectAbility(i);
            }
            return;
        }

        // Check right click
        if (Input.GetMouseButtonDown(1))
        {
            GameObject abilityObj = abilities[selected];
            if (abilityObj)
            {
                AbilityBase ability = abilityObj.GetComponent<AbilityBase>();
                if (!ability.IsActive())
                {
                    Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 delta = mouseWorldPos - transform.position;
                    delta.z = 0f;
                    delta.Normalize();
                    ability.Use(delta);
                }
            }
        }

        // Ability switching
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            // Scroll up
            int ind = selected;
            for (int i = 0; i < abilityUnlocked.Count - 1; i++)
            {
                ind++;
                if (ind >= abilityUnlocked.Count)
                    ind = 0;

                if (abilityUnlocked[ind])
                {
                    SelectAbility(ind);
                    break;
                }
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            // Scroll down
            int ind = selected;
            for (int i = 0; i < abilityUnlocked.Count - 1; i++)
            {
                ind--;
                if (ind < 0)
                    ind = abilityUnlocked.Count - 1;

                if (abilityUnlocked[ind])
                {
                    SelectAbility(ind);
                    break;
                }
            }
        }

    }
}
