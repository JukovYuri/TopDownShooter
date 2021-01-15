using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MainCharacter

{
	public Action OnPlayerDie = delegate { };
	GameManager gameManager;
	public int bullets = 15;
	public int money = 20;
	private enum Weapons
	{
		BAT,
		PISTOL,
		RIFFLE
	}


	public override void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		base.Start();
		gameManager.SetTextBullets(bullets);
		gameManager.SetTextMoney(money);

	}



	public void Update()
	{
		if (!isLife)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			animator.SetTrigger("ChangeWeapon");
		}

		CheckFire();
	}


	private void CheckFire()
	{
		if (Input.GetButton("Fire1") && nextFire <= 0)
		{
			if (bullets < 1)
			{
				animator.SetTrigger(Weapons.BAT.ToString());
			}
			else
			{
				SetBullets(--bullets);
			}
			Shoot();
			nextFire = fireRate;
		}


		if (nextFire > 0)
		{
			nextFire -= Time.deltaTime;
		}
	}

	public override void CheckLife()
	{
		base.CheckLife();

		gameManager.SetTextHealth(health);

		if (health < 1)
		{
			OnPlayerDie();
			gameManager.RestartGame();
		}
	}

	public void AddMoney(int money)
	{
		this.money += money;
		gameManager.SetTextMoney(this.money);
	}

	public void AddBullets(int bullets)
	{
		this.bullets += bullets;
		gameManager.SetTextBullets(this.bullets);
	}

	public void SetBullets(int bullets)
	{
		this.bullets = bullets;
		gameManager.SetTextBullets(this.bullets);
	}

	public void SetHealth(int health)
	{
		this.health = health;
		gameManager.SetTextHealth(health);

	}

}