using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ShipPartMissingRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    private PlayerInfo playerInfo;
    private SpriteRenderer sprite;
    [SerializeField]
    private Sprite unlockedSprite;
    [SerializeField]
    private int partNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerInfo = player.GetComponent<PlayerInfo>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (playerInfo.partsCollected[partNum])
        {
            sprite.sprite = unlockedSprite;
            enabled = false;
        }
    }
}
