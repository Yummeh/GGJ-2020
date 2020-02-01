using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDefinition
{
    #region Data
    // Points
    [System.Serializable]
    public class Point
    {
        public Transform trans;
        public Vector3 startPos;

        

        public Point(Transform trans)
        {
            this.trans = trans;
        }

        public void InitPoint()
        {
            startPos = trans.position;
        }

        public void MovePoint(Vector3 center, float scale)
        {
            trans.position = Vector3.Lerp(center, startPos, scale);
        }
    }

    // Lines
    [System.Serializable]
    public class Line
    {
        // Point data of the line in which the
        public Vector2 vectorA;
        public Vector2 vectorB;

        private Vector2 minValues;
        private Vector2 maxValues;

        // A number to check on which side the inside of the line is (1 or -1)
        private float insideValue;

        public bool needsCheckIfBetweenLine;

        // Force needed to force any object to the inside of this line
        public Vector2 ForceGoingInside { get; set; }
        private Vector2 perpendicularForce;

        public Line(Vector2 A, Vector2 B, Vector2 inside, bool needsCheckIfBetweenLine)
        {
            vectorA = A;
            vectorB = B;

            minValues = new Vector2(Mathf.Min(A.x, B.x), Mathf.Min(A.y, B.y));
            maxValues = new Vector2(Mathf.Max(A.x, B.x), Mathf.Max(A.y, B.y));

            this.needsCheckIfBetweenLine = needsCheckIfBetweenLine;

            CallibrateInside(inside);
        }

        // Calculate the inside side of the line and set the forces
        public void CallibrateInside(Vector2 inside)
        {
            insideValue = PointDirectionOnLine(vectorA, vectorB, inside);

            ForceGoingInside = -GeneralTools.GetVector2FromVector3(CalculateForceInside());
            perpendicularForce = Vector2.Perpendicular(ForceGoingInside);
        }

        // Tool that can be used to check if a point is inside this line
        public bool CheckIfIsPointInside(Vector2 point, bool CheckWithinLineEnds = false)
        {
            if (CheckWithinLineEnds && needsCheckIfBetweenLine)
                if(!CalculateIfBetweenPoints(point))
                    return true;

            // Check on which side the point is of the line
            float value = PointDirectionOnLine(vectorA, vectorB, point);

            return value == insideValue ? true : false;
        }

        // Caculate the side the point is facing in relation with the line
        private float PointDirectionOnLine(Vector2 A, Vector2 B, Vector2 C)
        {
            //(Bx - Ax) * (Cy - Ay) - (By - Ay) * (Cx - Ax)
            return Mathf.Sign(
                    (B.x - A.x) *
                    (C.y - A.y) -
                    (B.y - A.y) *
                    (C.x - A.x));
        }

        private Vector3 CalculateForceInside()
        {
            Vector2 directionVector = Vector2.Perpendicular((vectorA - vectorB).normalized);
            return new Vector3(directionVector.x, directionVector.y, 0) * -1;
        }

        // Check whether a point is inbetween the two points that make up this line
        private bool CalculateIfBetweenPoints(Vector2 point)
        {
            // Calculate the closest point on this line to the fish
            // (1) = (point - VectorA) / forceForInside
            // (2) = Perpendicular(forceForInside) / forceForInside
            // (3) = ((1)x - (1)y) / ((2)x - (2)y)
            // (4) = (3) *  Perpendicular(forceForInside) + VectorA /=/ Point on the line of the perpendicular line relative to the fish
            Vector2 one = (point - vectorA) / ForceGoingInside;
            Vector2 two = perpendicularForce / ForceGoingInside;
            float three = (one.x - one.y) / (two.x - two.y);
            Vector2 closestPointOnLine = three * perpendicularForce + vectorA;

            return closestPointOnLine.x > minValues.x &&
                closestPointOnLine.x < maxValues.x &&
                closestPointOnLine.y > minValues.y &&
                closestPointOnLine.y < maxValues.y;
        }
    }
    #endregion
}
