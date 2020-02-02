using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUI : MonoBehaviour
{
    private PlayerInfo player;
    public GameObject heartPrefab;
    public GameObject halfHeartPrefab;
    public GameObject emptyHeartPrefab;

    private GameObject[,] healthInstance = new GameObject[3,3];

    void Start()
    {
        player = FindObjectOfType<PlayerInfo>();
        for(int i = 0; i < healthInstance.GetLength(0); i++)
        {
            healthInstance[i,2] = Instantiate(heartPrefab, transform);
            healthInstance[i,1] = Instantiate(halfHeartPrefab, transform);
            healthInstance[i,0] = Instantiate(emptyHeartPrefab, transform);

            healthInstance[i, 2].transform.position = new Vector3(i * 100 + 100, 1000, 0);
            healthInstance[i, 1].transform.position = new Vector3(i * 100 + 100, 1000, 0);
            healthInstance[i, 0].transform.position = new Vector3(i * 100 + 100, 1000, 0);
        }

        UpdateHeart();
    }

    public void UpdateHeart()
    {
        int health = player.health;
        for (int i = 0; i < healthInstance.GetLength(0); i++, health -= 2)
        {
            if (health < 0)
                health = 0;

            healthInstance[i, 0].SetActive(0 == health ? true : false);
            healthInstance[i, 1].SetActive(1 == health ? true : false);
            healthInstance[i, 2].SetActive(2 <= health ? true : false);
        }
    }
}
