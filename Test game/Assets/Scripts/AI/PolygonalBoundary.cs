using UnityEngine;

// Calculate boundaries based on the bounds of the renderer
public class PolygonalBoundary : Boundary
{
    #region Point Setup
    // Create all the points and setup all the data needed for the points
    public override void SetupCollider()
    {
        base.SetupCollider();

        foreach (Transform point in transform.GetComponentsInChildren<Transform>())
            AddPoint(point);

        SetupLines();
    }

    // Add the point to the list
    private void AddPoint(Transform trans)
    {
        // Return when same object
        if (transform == trans) return;
        trans.name = "Point: " + (points.Count + 1);

        points.Add(new BoundaryDefinition.Point(trans));
        points[points.Count - 1].InitPoint(); // Init point
    }
    #endregion

    #region Line Setup

    protected override void SetupLines()
    {
        // Reset the checkBetweenPoints list
        float tempCount = checkIfBetweenPoints.Count;
        if(tempCount != points.Count)
            checkIfBetweenPoints.Clear();

        // Get the inside of the collider
        Vector2 inside = GeneralTools.GetVector2FromVector3(transform.position);

        var prevPoint = GeneralTools.WrapAroundArrayReturn(points, -1);
        foreach (BoundaryDefinition.Point point in points)
        {
            // Set the check if a line needs to check further than its points
            if (points.Count != tempCount)
                checkIfBetweenPoints.Add(false);

            // Add the line
            Vector2 currentPoint = GeneralTools.GetVector2FromVector3(point.trans.position);
            Vector2 previousPoint = GeneralTools.GetVector2FromVector3(prevPoint.trans.position);

            lines.Add(new BoundaryDefinition.Line(currentPoint, previousPoint, inside, checkIfBetweenPoints[points.IndexOf(point)]));
            prevPoint = point;
        }
    }

    #endregion
}
