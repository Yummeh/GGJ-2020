using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    #region Fields
    // Refference
    protected MeshRenderer renderer;

    // Settings
    [SerializeField] protected bool showGraphic;

    // Point info
    [SerializeField] protected List<BoundaryDefinition.Point> points = new List<BoundaryDefinition.Point>();
    [SerializeField] protected List<bool> checkIfBetweenPoints = new List<bool>();

    // Line Info 
    [SerializeField] protected List<BoundaryDefinition.Line> lines = new List<BoundaryDefinition.Line>();

    #endregion

    protected virtual void Start()
    {
        SetupCollider();
    }

    public virtual void SetupCollider()
    {
        renderer = GetComponent<MeshRenderer>();
        points.Clear();
        lines.Clear();
    }

    protected virtual void SetupLines()
    {

    }

    #region Math Tools

    // Return a force depending on the way it is outside
    public Vector3 ForceInsideBounds(Vector3 point, bool polygonal)
    {
        Vector3 newForce = Vector3.zero;

        // Add a force perpendicular to the line on the x and z axis if not inside
        foreach (BoundaryDefinition.Line line in lines)
        {
            // If not inside add force
            if (!line.CheckIfIsPointInside(GeneralTools.GetVector2FromVector3(point), polygonal))
                newForce += GeneralTools.MakeVector3FromVector2(line.ForceGoingInside);
        }

        return newForce;
    }

    // Return whether a point is inside the boundaries
    public bool CheckIfInsideBounds(Vector3 point)
    {
        // Check if inside the lines
        foreach (BoundaryDefinition.Line line in lines)
        {
            // If not inside return false
            if (!line.CheckIfIsPointInside(GeneralTools.GetVector2FromVector3(point)))
                return false;
        }

        return true;
    }
    #endregion

    protected virtual void OnDrawGizmos()
    {
        if (showGraphic)
        {
            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    if (points[i].trans != null)
                        Gizmos.DrawSphere(points[i].trans.position, 0.1f);
                }
            }
            if (lines != null)
            {
                for (int i = 0; i < lines.Count; i++)
                {
                    Gizmos.color = Color.blue;
                    Vector3 pointA = GeneralTools.MakeVector3FromVector2(lines[i].vectorA);
                    Vector3 pointB = GeneralTools.MakeVector3FromVector2(lines[i].vectorB);

                    Gizmos.color = lines[i].needsCheckIfBetweenLine ? Color.blue : Color.cyan;
                    Gizmos.DrawLine(pointA, pointB);

                    // Show the direction of which a force will be applied when a point goes outside those boundaries
                    Vector3 middlePos = Vector3.Lerp(pointA, pointB, 0.5f);
                    Gizmos.color = Color.green;
                    Gizmos.DrawRay(middlePos, GeneralTools.MakeVector3FromVector2(lines[i].ForceGoingInside));
                }
            }
        }
    }
}
