using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField]
    private GameObject weaponObj;
    private BoxCollider2D weaponBox;
    [SerializeField]
    private GameObject weaponSpriteObj;
    private Animator weaponAnimator;

    [SerializeField]
    private float activeDuration = 1f;
    [SerializeField]
    private float activeCooldown = 1f;
    private float activeTimer = 0f;
    private bool weaponActive = false;
    private bool weaponUsed = false;

    // Start is called before the first frame update
    void Start()
    {
        if (weaponObj != null)
        {
            weaponBox = weaponObj.GetComponent<BoxCollider2D>();
            if (weaponBox == null)
            {
                print("weapon object has no box collider 2D");
                enabled = false;
            }
            else
            {
                weaponBox.enabled = false;
            }
        }
        else
        {
            print("weapon object not set");
            enabled = false;
        }

        if (weaponSpriteObj != null)
        {
            weaponAnimator = weaponSpriteObj.GetComponent<Animator>();
            if (weaponBox == null)
            {
                print("weapon sprite has no Animator");
                enabled = false;
            }
        }
        else
        {
            print("weapon sprite not set");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotation();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!weaponUsed)
            {
                Use();
            }
        }

        UpdateUse();
    }

    void UpdateRotation()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 delta = mouseWorldPos - transform.position;

        if (delta.magnitude > 0.01f)
        {
            float angle = Mathf.Atan2(delta.x, -delta.y) * Mathf.Rad2Deg - 90f;
            transform.localEulerAngles = new Vector3(0f, 0f, angle);
        }
    }

    void Use()
    {
        weaponUsed = true;
        weaponActive = true;
        activeTimer = activeDuration;
        weaponBox.enabled = true;
        weaponAnimator.SetTrigger("Attack");
    }

    void UpdateUse()
    {
        if (weaponUsed)
        {
            activeTimer -= Time.deltaTime;
            if (activeTimer <= 0f)
            {
                if (weaponActive)
                {
                    weaponActive = false;
                    weaponBox.enabled = false;
                    activeTimer = activeCooldown;
                }
                else
                {
                    weaponUsed = false;
                }
            }
        }
    }
}
