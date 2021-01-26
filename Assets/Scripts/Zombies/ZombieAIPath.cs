using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ZombieAIPath : MainCharacter
{
	public int damage = -25;

	public float viewAngle = 90f;
	public float returnRadius = 30f;
	public float moveRadius = 10f;
	public float attackRadius = 3f;
	public float noticeAnywayRadius = 6f;

	public float waitingTimeForPatrol = 3f;

	public ZombieState activeState;

	Coroutine checkFire;
	bool startFire;

	float distanceToPlayer;

	Vector3 startPosition;

	public LayerMask whatIsObstacles;
	bool isObstacles;

	Player player;
	AIPath aIPath;

	public enum ZombieState
	{
		STAND,
		ROTATE_TO_PLAYER,
		MOVE_TO_PLAYER,
		ATTACK,
		RETURN,
		PATROL
	}

	public override void Start()
	{
		base.Start();
		player = FindObjectOfType<Player>();
		ChangeState(ZombieState.STAND);
		whatIsObstacles = LayerMask.GetMask("Wall", "Obstacles");


		player.OnPlayerDie += ChangeStateInStand;

		startPosition = transform.position;

	}

	public override void Awake()
	{
		base.Awake();
		aIPath = GetComponent<AIPath>();
	}

	public void Update()
	{

		if (!isLife)
		{
			return;
		}

		isObstacles = Physics2D.Raycast(transform.position, player.transform.position - transform.position, returnRadius, whatIsObstacles);

		distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);


		switch (activeState)
		{
			case ZombieState.STAND:
				DoStand();
				break;

			case ZombieState.ROTATE_TO_PLAYER:
				DoRotateToPlayer();
				break;

			case ZombieState.ATTACK:
				DoAttack();
				break;

			case ZombieState.MOVE_TO_PLAYER:
				DoMoveToPlayer();
				break;

			case ZombieState.RETURN:
				DoReturn();
				break;

			case ZombieState.PATROL:
				DoPatrol();
				break;
		}

	}

	private void ChangeState(ZombieState newState)
	{
		
		switch (newState)
		{
			case ZombieState.STAND:
				aIPath.enabled = false;
				animator.SetFloat("Speed", 0);
				print("TO_STAND");
				break;

			case ZombieState.ROTATE_TO_PLAYER:
				aIPath.enabled = false;
				animator.SetFloat("Speed", 0);
				print("TO_ROTATE_TO_PLAYER");
				break;

			case ZombieState.ATTACK:
				aIPath.enabled = false;
				animator.SetFloat("Speed", 0);
				StartAttack();
				print("TO_ATTACK");
				break;

			case ZombieState.MOVE_TO_PLAYER:
				aIPath.enabled = true;
				animator.SetFloat("Speed", 1);
				StopAttack();
				print("TO_MOVE_TO_PLAYER");
				break;

			case ZombieState.RETURN:
				aIPath.destination = startPosition;
				aIPath.enabled = true;
				animator.SetFloat("Speed", 1);
				print("TO_RETURN");
				break;

			case ZombieState.PATROL:
				aIPath.enabled = true;
				animator.SetFloat("Speed", 1);
				print("TO_PATROL");
				break;
		}
		activeState = newState;
	}

	private void DoStand()
	{
		if (CheckReturn())
		{
			return;
		}

		if (CheckRotateToPlayer())
		{
			return;
		}

		if (CheckMoveToPlayer())
		{
			return;
		}

		if (CheckAttack())
		{
			return;
		}

		print("STAND");
	}

	private void DoRotateToPlayer()
	{

		if (CheckMoveToPlayer())
		{
			return;
		}

		if (CheckReturn())
		{
			return;
		}

		if (CheckStand())
		{
			return;
		}

		print("ROTATE_TO_PLAYER");
		RotateOnly();
		ShowViewZone();
	}

	private void DoAttack()
	{
		if (CheckStand())
		{
			return;
		}

		if (CheckMoveToPlayer())
		{
			return;
		}

		print("ATTACK");
		ShowViewZone();
	}

	private void DoMoveToPlayer()
	{
		if (CheckReturn())
		{
			return;
		}

		if (CheckRotateToPlayer())
		{
			return;
		}

		if (CheckAttack())
		{
			return;
		}

		aIPath.destination = player.transform.position;
		print("MOVE");
		ShowViewZone();
	}

	private void DoReturn()
	{

		if (CheckStand())
		{
			return;
		}

		if (CheckRotateToPlayer())
		{
			return;
		}
		print("RETURN");
	}

	private void DoPatrol()
	{
		print("PATROL");
	}

	private bool CheckReturn()
	{
		if (Vector3.Distance(startPosition, transform.position) > 0.1f && 
		   (distanceToPlayer > noticeAnywayRadius && isObstacles) ||
		   (distanceToPlayer > returnRadius))
		{
			ChangeState(ZombieState.RETURN);
			return true;
		}
		return false;
	}

	private bool CheckStand()
	{
		if (Vector3.Distance(startPosition, transform.position) <= 0.1f && isObstacles)
		{
			ChangeState(ZombieState.STAND);
			return true;
		}
		return false;
	}



	private bool CheckRotateToPlayer()
	{
		if (distanceToPlayer > moveRadius && distanceToPlayer < returnRadius && !isObstacles) //обзор
		{
			ChangeState(ZombieState.ROTATE_TO_PLAYER);
			return true;
		}
		return false;
	}

	private bool CheckMoveToPlayer()
	{
		if ((distanceToPlayer > attackRadius && distanceToPlayer < moveRadius && !isObstacles) ||
			(distanceToPlayer > attackRadius && distanceToPlayer < noticeAnywayRadius && isObstacles))
		{
			ChangeState(ZombieState.MOVE_TO_PLAYER);
			return true;
		}
		return false;
	}

	private bool CheckAttack()
	{
		if (distanceToPlayer <= attackRadius && !isObstacles) //обзор
		{
			ChangeState(ZombieState.ATTACK);
			return true;
		}
		return false;
	}




	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, moveRadius);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, returnRadius);

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, noticeAnywayRadius);
	}

	private void ShowViewZone()
	{
		Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, viewAngle / 2) * -transform.up * returnRadius, Color.red);
		Debug.DrawRay(transform.position, Quaternion.Euler(0, 0, -viewAngle / 2) * -transform.up * returnRadius, Color.red);
	}

	public void StartAttack()
	{
		if (!startFire)
		{
			checkFire = StartCoroutine(CheckFire());
			startFire = true;
		}
	}

	public void StopAttack()
	{
		if (startFire)
		{
			StopCoroutine(checkFire);
			startFire = false;
		}
	}

	IEnumerator CheckFire()
	{
		while (true)
		{
			float fireDelay = Random.Range(0f, 2 * fireRate);
			yield return new WaitForSeconds(fireDelay);
			if (isLife && player.isLife)
			{
				Hit();
				player.AddHealth(damage);
			}
			else
			{
				yield break;
			}
		}
	}

	public void ChangeStateInStand()
	{
		ChangeState(ZombieState.STAND);
	}

	public void RotateOnly()
	{
		Rotate(player.transform.position);
	}

	public void Rotate(Vector3 targetPosition)
	{
		Vector3 objectPosition = transform.position;
		Vector3 direction = targetPosition - objectPosition;
		direction.z = 0;
		transform.up = -direction;
	}

}
