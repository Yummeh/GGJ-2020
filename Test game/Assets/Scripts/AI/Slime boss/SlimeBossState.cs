using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBoss : MonoBehaviour
{
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


	// Idle
	private int idleTime = 180;
	private int idleTimeCounter = 0;
	private bool smallhop = true;


    // Start is called before the first frame update
    void Start()
    {
		state = "idle";
    }

	private void FixedUpdate()
	{
		// Check slime
		CheckSlimeState();

		switch (state)
		{
			case "idle": // Wandering state
				// Hop -> idle -> repeat
				if (idleTimeCounter < idleTime)
				{
					idleTimeCounter++;
				} else
				{
					Smallhop();
					idleTimeCounter = 0;
				}
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
		// animation
		throw new NotImplementedException();
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
