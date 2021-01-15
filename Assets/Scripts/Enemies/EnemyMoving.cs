using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MainCharacterMoving
{
	Enemy enemy;
	Player player;

	public override void Awake()
	{
		base.Awake();
		enemy = GetComponent<Enemy>();
	}
	void Start()
	{
		player = FindObjectOfType<Player>();
	}
	void Update()
	{
		if (!player.isLife || !enemy.isLife)  
		{
			return;
		}

			Rotate(player.transform.position);
			Move(Vector2.zero);
	}

}
