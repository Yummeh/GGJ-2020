using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAreaController : MonoBehaviour
{
    public PolygonalBoundary[] areas = new PolygonalBoundary[4];
    private AIManager manager;

    private float time = 0;
    private float checkBoundariesAgain = 1;

    private void Start()
    {
        manager = FindObjectOfType<AIManager>();
    }

    private void Update()
    {
        time += Time.deltaTime;

        if(time > checkBoundariesAgain)
        {
            time = 0;
            manager.currentLevel = CheckIfInsideBoundary(manager.player.transform.position);
        }
    }

    public AreaLevel CheckIfInsideBoundary(Vector3 point)
    {
        for (int i = 0; i < areas.Length; i++)
        {
            if (areas[i] != null)
                if (areas[i].CheckIfInsideBounds(point))
                    return (AreaLevel)i;
        }
        return AreaLevel.Level0;
    }
}
