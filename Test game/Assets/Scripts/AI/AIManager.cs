using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<GameObject> attackableEntities = new List<GameObject>();

    [HideInInspector] public PolygonalBoundary boundary;

    [HideInInspector] public PlayerMovement player;

    public Transform bulletParent;
    [SerializeField] private Transform avoidPointParent;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform spawnPointParent;

    // Input from the user for point the fish should avoid, these can be moving or static
    public List<AvoidPoint> avoidPoints = new List<AvoidPoint>();
    public List<SpawnList> spawnPoints = new List<SpawnList>();
    public List<EnemyList> spawnableEnemies = new List<EnemyList>();

    [Space(20f)]
    public int maxEnemiesInArea = 20;
    private int currentEnemiesInArea = 0;
    private AreaLevel currentLevel = 0;

    private float spawnTime = 0;
    private float spawnTimer = 10;

    private void Awake()
    {
        boundary = FindObjectOfType<PolygonalBoundary>();
        player = FindObjectOfType<PlayerMovement>();
    }

    private void Update()
    {
        spawnTime += Time.deltaTime;
        if(spawnTime > spawnTimer)
        {
            spawnTime = 0;

        }
    }

    // Draw info in the scene
    private void OnDrawGizmos()
    {
        // Draw the avoid points green or red in the scene
        // Green is attracted towards and red avoiding
        for (int iPoint = 0; iPoint < avoidPoints.Count; iPoint++)
        {
            Gizmos.color = avoidPoints[iPoint].strength > 0 ? Color.red : Color.green;

            if (avoidPoints[iPoint].point != null)
                Gizmos.DrawSphere(avoidPoints[iPoint].GetPos(), avoidPoints[iPoint].radius);
        }
    }
}

[System.Serializable]
public class AvoidPoint
{
    public Transform point;
    public float radius;
    public float strength;

    public AvoidPoint(Transform point, float radius, float strength)
    {
        this.point = point;
        this.radius = radius;
        this.strength = strength;
    }

    public Vector3 GetPos()
    {
        return point.position;
    }
}

[System.Serializable]
public class SpawnPoint
{
    public Transform position;
    public AreaLevel level;
}

[System.Serializable]
public class SpawnList
{
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
    public AreaLevel level;
}

[System.Serializable]
public class EnemyList
{
    public List<GameObject> availableEnemies = new List<GameObject>();
}

public enum AreaLevel
{
    Level0,
    Level1,
    Level2,
    Level3
}