using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Activate()
    {
        //enabled = true;
        //gameObject.SetActive(true);
    }

    public virtual void Deactivate()
    {
        //enabled = false;
        //gameObject.SetActive(false);
    }

    public virtual bool IsActive()
    {
        return false;
    }

    public virtual void Use(Vector2 direction)
    {
    }
}
