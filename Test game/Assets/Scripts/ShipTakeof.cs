using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class ShipTakeof : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private GameObject player;
    private PlayerInfo playerInfo;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerInfo = player.GetComponent<PlayerInfo>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        bool hasAll = true;
        for (int i = 0; i < playerInfo.partsCollected.Length; i++)
        {
            if (!playerInfo.partsCollected[i])
            {
                hasAll = false;
                break;
            }
        }

        if (hasAll)
        {
            animator.SetTrigger("Play");

            // Remove parts
            ShipPartMissingRenderer[] pickups = Object.FindObjectsOfType<ShipPartMissingRenderer>();
            foreach (var p in pickups)
            {
                Destroy(p.gameObject);
            }

            // Disable player
            SpriteRenderer[] playerSprites = player.GetComponentsInChildren<SpriteRenderer>();
            foreach (var s in playerSprites)
            {
                s.enabled = false;
            }
            Rigidbody2D playerBody = player.GetComponent<Rigidbody2D>();
            if (playerBody != null)
            {
                playerBody.bodyType = RigidbodyType2D.Static;
            }

        }
    }
}
