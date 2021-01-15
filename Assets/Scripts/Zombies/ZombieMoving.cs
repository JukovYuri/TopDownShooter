using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMoving : MainCharacterMoving
{
	public GameObject[] wayPoints;
	public bool isPatrol;

	Zombie zombie;
	Player player;
	public Vector3 startPosition;
	int index = 0;

	public override void Awake()
	{
		base.Awake();
		zombie = GetComponent<Zombie>();
	}
	void Start()
	{
		player = FindObjectOfType<Player>();
		startPosition = transform.position;
		startPosition.z = 0;

		player.OnPlayerDie += StopMove;
	}
	void Update()
	{

		if (isPatrol)
		{
			ToPatrolMoving();
			return;
		}

	}


	public void ToPlayerMoving()
	{
		Rotate(player.transform.position);
		Move(player.transform.position - transform.position);
	}

	public void ToPatrolMoving()
	{
		if (Vector3.Distance(wayPoints[index].transform.position, transform.position) < 0.1f)
		{
			index++;
			if (index > wayPoints.Length - 1)
			{
				index = 0;
			}
		}
		Rotate(wayPoints[index].transform.position);
		Move(wayPoints[index].transform.position - transform.position);
	}

	public void ToStartMoving() 
	{
		Rotate(startPosition);
		Move(startPosition - transform.position);
	}

	public void RotateOnly()
	{
		Rotate(player.transform.position);
	}
}
