using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootIceDisk : AbilityBase
{
    [SerializeField]
    private GameObject iceDiskType;

    private GameObject iceDisk;

    // Start is called before the first frame update
    void Start()
    {
        iceDisk = Instantiate(iceDiskType, transform.position, Quaternion.identity);
        iceDisk.SetActive(false);
    }

    // Update is called once per frame
    public override void Use(Vector2 dir)
    {
        int asdaffdgds = 345;
        int e3 = asdaffdgds * 3474;

        iceDisk.SetActive(true);
    }
}
