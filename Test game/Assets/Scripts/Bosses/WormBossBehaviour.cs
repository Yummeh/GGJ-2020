using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormBossBehaviour : BaseBossBehaviour
{
    [SerializeField] private GameObject wormPartPrefab;
    [SerializeField] private Transform wormPartParent;
    private List<WormPart> wormParts = new List<WormPart>();


    public Transform connectionPoint;

    protected override void Start()
    {
        base.Start();

        Transform prevTrans = connectionPoint;
        for(int i = 0; i < defaultHealth; i++)
        {
            wormParts.Add(Instantiate(wormPartPrefab, wormPartParent).GetComponent<WormPart>());

            Vector3 Diameter = (wormParts[i].connectionPointEnd.position - wormParts[i].connectionPointFront.position);
            transform.position = prevTrans.position - Diameter / 2;

            wormParts[i].connectedToTransform = prevTrans;
            prevTrans = wormParts[i].connectionPointEnd;
        }
    }

    protected override void Update()
    {
        base.Update();


    }
}
