using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIceDisk : AbilityBase
{
    [SerializeField]
    private GameObject iceDiskType;
    private GameObject iceDisk;
    private IceDiskProjectile iceDiskProj;

    [SerializeField]
    private float pickupRange = 1f;
    [SerializeField]
    private float diskSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (iceDisk == null)
            return;

        Vector3 delta = iceDisk.transform.position - transform.position;

        if (delta.magnitude < pickupRange && iceDiskProj.returning)
        {
            Destroy(iceDisk);
            iceDisk = null;
        }
    }

    public override void Use(Vector2 dir)
    {
        if (iceDisk == null)
        {
            iceDisk = Instantiate(iceDiskType, transform.position, Quaternion.identity);
            iceDiskProj = iceDisk.GetComponent<IceDiskProjectile>();

            iceDiskProj.startingVelocity = new Vector3(dir.x, dir.y, 0f) * diskSpeed;
            iceDiskProj.startingPosition = transform.position;
            iceDiskProj.returnTo = gameObject;
        }
    }
}
