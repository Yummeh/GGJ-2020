using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Unlock
{
    Part1,
    Part2,
    Part3,
    Sword,
    AbilityDisk,
    AbilityWhip,
}

public class Pickup : MonoBehaviour
{
    [SerializeField] private Unlock unlock = Unlock.Sword;

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerInfo info = collision.GetComponent<PlayerInfo>();
        if (info != null)
        {
            info.OnUnlock(unlock);
            Destroy(gameObject);
        }
    }
}
