using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MainCharacter
{
	Coroutine checkFire;
	Player player;

	public override void Start()
	{
		base.Start();
		player = FindObjectOfType<Player>();
		checkFire = StartCoroutine(CheckFire());
		player.OnPlayerDie += StopAttack;
	}

	public void StopAttack()
	{
		StopCoroutine(checkFire);
	}

	IEnumerator CheckFire()
	{
		while (true)
		{
			float fireDelay = Random.Range(0f, 2 * fireRate);
			yield return new WaitForSeconds(fireDelay);

			if (isLife)
			{
				Shoot();
			}
			else
			{
				yield break; 
			}

		}

	}
}
