using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
	private Animator animator;
	private Rigidbody2D rigidbody;

	private float maxHealth = 100;
	private float currentHealth = 100;
	private string state = "idle";
	private bool isSplit = false;
	private bool isAttacking = false;
	
	private bool isConfused = false;
	private int confusedCounter = 0;
	private bool isRecharging = false;
	private int confusedTime = 420;
	private int rechargeTime = 300;
	private int varTimer = 300;
	private int rechargeCounter = 0;

	private Vector2 goalPosition;

	// Idle
	private int idleTime = 180;
	private int idleTimeCounter = 0;
	private bool smallhopping = false;
	private int smallhopRadius = 10;


    // Start is called before the first frame update
    void Start()
    {
		animator = GetComponent<Animator>();
		rigidbody = GetComponent<Rigidbody2D>();
		if (rigidbody == null)
		{
			print("hoi");
		}
		state = "idle";
		
    }

	private void FixedUpdate()
	{
		// Check slime
		CheckSlimeState();
		UpdateSlime();

		switch (state)
		{
			case "idle": // Wandering state
				// Hop -> idle -> repeat
				//if (idleTimeCounter < idleTime)
				//{
				//	idleTimeCounter++;
				//} else
				//{
				//	Smallhop();
				//	idleTimeCounter = 0;
				//}
				break;
			case "aggro":
				if (!isAttacking && rechargeCounter < varTimer)
				{
					rechargeCounter++;
				}
				else if (!isAttacking)
				{
					int r = UnityEngine.Random.Range(1, 2);
					if (r == 1)
					{
						varTimer = confusedTime;
						Bigjump();
					} else
					{
						varTimer = rechargeTime;
						Triplejump();
					}
					isAttacking = true;
					rechargeCounter = 0;
				}

				// Random chance to do jumping spree or big shockwave jump
				// Jumping spree: jump to player 3x then redo aggro state
				// Big shockwave jump: jump to player with a huge jump, then a after the big aoe damage he is confused
				// Repeat state
				break;
			case "split":
				// stop animation and drop the 3 slimes
				
				break;
			case "resetting":
				currentHealth = maxHealth;
				// move to mid then -> idle

				break;
		}
	}

	private void UpdateSlime()
	{
		if (smallhopping)
		{
			print(goalPosition);
			Vector2 pos = Vector2.MoveTowards(rigidbody.position, goalPosition, 0.5f);
			rigidbody.MovePosition(pos);
			smallhopping = (Vector2.Distance(rigidbody.position, goalPosition) < 1) ? false : true;
		}
	}

	private void Triplejump()
	{
		throw new NotImplementedException();
	}

	private void Bigjump()
	{
		throw new NotImplementedException();
	}

	private void Smallhop()
	{
		goalPosition = new Vector2(rigidbody.position.x + UnityEngine.Random.Range(-smallhopRadius, smallhopRadius),
									rigidbody.position.y + UnityEngine.Random.Range(-smallhopRadius, smallhopRadius));
		smallhopping = true;
		// animation
		animator.SetTrigger("Smallhop");

	}

	// Update is called once per frame
	void Update()
    {
        
    }

	public void CheckSlimeState()
	{
		if (currentHealth < maxHealth/2)
		{
			state = "split";
		}

		if (isConfused && confusedCounter < confusedTime)
		{
			confusedCounter++;
		} else
		{
			isConfused = false;
		}

		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		TriggerBigSlime();
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		ResetSlimeBoss();
	}

	public void TriggerBigSlime()
	{
		// start attack pattern 
	}

	public void ResetSlimeBoss()
	{
		if (state == "split")
		{
			// All smaller slimes move to middle
		} else
		{
			// Big slime moves to middle
		}
	}

	public void Confuse()
	{
		isConfused = true;
		confusedCounter = 0;
	}

	public void Recharge()
	{
		isRecharging = true;
		rechargeCounter = 0;
	}

}
