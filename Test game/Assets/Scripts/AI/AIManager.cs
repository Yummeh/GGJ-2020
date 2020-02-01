using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public List<GameObject> attackableEntities = new List<GameObject>();

    public PolygonalBoundary boundary;

    // Input from the user for point the fish should avoid, these can be moving or static
    [SerializeField] public List<AvoidPoint> avoidPoints = new List<AvoidPoint>();

    private void Awake()
    {
        boundary = GetComponent<PolygonalBoundary>();
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
