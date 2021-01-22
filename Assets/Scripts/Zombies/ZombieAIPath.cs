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
	public float waitingTimeForPatrol = 3f;

	private Transform startTransform;
	private Vector3 startPosition;

	LayerMask layer;
	RaycastHit2D hit;

	Player player;
	AIPath aIPath;
	AIDestinationSetter aIDestinationSetter;
	public ZombieState activeState;

	Coroutine checkFire;

	bool startFire;
	float distanceToPlayer;

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
		layer = LayerMask.GetMask("Wall", "Obstacles");

		player.OnPlayerDie += ChangeStateInStand;
		startTransform = transform;
		startPosition = transform.position;
	}

	public override void Awake()
	{
		base.Awake();
		aIPath = GetComponent<AIPath>();
		aIDestinationSetter = GetComponent<AIDestinationSetter>();
	}

	public void Update()
	{

		if (!isLife)
		{
			return;
		}

		hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position, returnRadius, layer);
		distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
		
		print(rb.velocity.magnitude);

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
				aIDestinationSetter.target = player.transform;
				StopAttack();
				print("TO_MOVE_TO_PLAYER");
				break;

			case ZombieState.RETURN:
				aIPath.enabled = true;
				animator.SetFloat("Speed", 1);
				aIDestinationSetter.target = startTransform;
				print("TO_RETURN");
				break;

			case ZombieState.PATROL:
				animator.SetFloat("Speed", 1);
				aIPath.enabled = true;
				print("TO_PATROL");
				break;
		}
		activeState = newState;
	}

	private void DoStand()
	{
		if (distanceToPlayer > returnRadius && Vector3.Distance(startPosition, transform.position) > 0.1f)
		{
			ChangeState(ZombieState.RETURN);
			return;
		}
		if (distanceToPlayer > moveRadius && distanceToPlayer < returnRadius) //обзор
		{
			ChangeState(ZombieState.ROTATE_TO_PLAYER);
			return;
		}

		if (distanceToPlayer > attackRadius && distanceToPlayer < moveRadius) //обзор
		{
			ChangeState(ZombieState.MOVE_TO_PLAYER);
			return;
		}

		if (distanceToPlayer < attackRadius) //обзор
		{
			ChangeState(ZombieState.ATTACK);
			return;
		}

		print("STAND");
	}

	private void DoRotateToPlayer()
	{

		if (distanceToPlayer > attackRadius && distanceToPlayer < moveRadius) //обзор
		{
			ChangeState(ZombieState.MOVE_TO_PLAYER);
			return;
		}

		if (distanceToPlayer > returnRadius && Vector3.Distance(startPosition, transform.position) > 0.1f)
		{
			ChangeState(ZombieState.RETURN);
			return;
		}

		if (distanceToPlayer > returnRadius && Vector3.Distance(startPosition, transform.position) <= 0.1f)
		{
			ChangeState(ZombieState.STAND);
			return;
		}

		print("ROTATE_TO_PLAYER");
		RotateOnly();
		ShowViewZone();
	}

	private void DoAttack()
	{

		if (distanceToPlayer > attackRadius)
		{
			ChangeState(ZombieState.MOVE_TO_PLAYER);
			return;
		}
		print("ATTACK");
		ShowViewZone();
	}

	private void DoMoveToPlayer()
	{
		if (distanceToPlayer > moveRadius && distanceToPlayer < returnRadius)
		{
			ChangeState(ZombieState.ROTATE_TO_PLAYER);
			return;
		}
		if (distanceToPlayer < attackRadius)
		{
			ChangeState(ZombieState.ATTACK);
			return;
		}

		print("MOVE");
		ShowViewZone();
	}

	private void DoReturn()
	{

		if (distanceToPlayer > returnRadius && Vector3.Distance(startPosition, transform.position) <= 0.1f)
		{
			ChangeState(ZombieState.STAND);
			return;
		}
		if (distanceToPlayer > moveRadius && distanceToPlayer < returnRadius)
		{
			ChangeState(ZombieState.ROTATE_TO_PLAYER);
			return;
		}
		print("RETURN");
	}

	private void DoPatrol()
	{
		print("PATROL");
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRadius);

		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, moveRadius);

		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(transform.position, returnRadius);
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
