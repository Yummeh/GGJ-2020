using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormPart : MonoBehaviour
{
    public Transform connectionPointFront;
    public Transform connectionPointEnd;
    public Transform connectedToTransform;
    private Vector3 prevPosOther;

    void Start()
    {
        
    }

    void Update()
    {
        //transform.position += connectedToTransform.position - prevPosOther;
        //transform.rotation = Quaternion.Lerp(transform.rotation, connectedToTransform.rotation, Time.deltaTime);

        //  prevPosOther = connectedToTransform.position;
    }
}
