using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBossBehaviour : BaseBossBehaviour
{
    
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void SpawnAttack()
    {
        FlowerTentacle tentacleScript = Instantiate(tentacle, transform).GetComponent<FlowerTentacle>();
        tentacleScript.transform.position = player.transform.position;
    }
}
