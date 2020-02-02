using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IceDiskFunctionality : MonoBehaviour
{
    [SerializeField]
    private Animator diskAnimator;
    private BoxCollider2D diskBox;
    private Rigidbody2D rigidbody;
    
    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject returnSpot;
    private Vector2 returnPosition;

    [SerializeField]
    private float travelDistance = 10f;
    private bool throwDisk = false;
    private bool returnDisk = false;
    public Vector3 mouseWorldPos;


    // Start is called before the first frame update
    void Start()
    {
        Transform CameraTransform = camera.transform;
        rigidbody = GetComponent<Rigidbody2D>();
        diskAnimator = gameObject.GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
		rigidbody = GetComponent<Rigidbody2D>();
		Vector2 mousePos = Input.mousePosition;
        rigidbody.position = returnSpot.transform.position;
        Use(mousePos);
    }

    // Update is called once per frame
    void Update()
    {
        returnPosition = new Vector2(returnSpot.transform.position.x, returnSpot.transform.position.y);

        if (throwDisk) 
        {   
            if (Vector2.Distance(rigidbody.position, new Vector2(mouseWorldPos.x, mouseWorldPos.y)) < 0.5f || 
                Vector2.Distance(rigidbody.position, returnPosition) > travelDistance)
            {
                returnDisk = true;
                throwDisk = false;
            }
            rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, new Vector2(mouseWorldPos.x, mouseWorldPos.y), 1f));
        } 
        else if (returnDisk)
        {
            rigidbody.MovePosition(Vector2.MoveTowards(rigidbody.position, returnPosition, 0.7f));
            if (Vector2.Distance(rigidbody.position, returnPosition) < 0.5f)
            {
                returnDisk = false;
                gameObject.SetActive(false);
            }
        }
    }

    void Use(Vector2 direction) 
    {
        diskAnimator.Play("IceDisk");
        throwDisk = true;
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
         //if (collision.attachedRigidbody == enemysomehow ) // return
    }

}
