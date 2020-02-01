using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PolygonalBoundary))]
public class PolygonalBoundaryEditor : BoundaryEditor
{
    PolygonalBoundary collider;

    protected override void InitFields()
    {
        base.InitFields();

        collider = target as PolygonalBoundary;
    }

    protected override void DrawFields()
    {
        base.DrawFields();

        if(GUILayout.Button("Load Boundaries"))
            collider.SetupCollider();
    }
}
