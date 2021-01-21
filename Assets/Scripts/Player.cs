using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MainCharacter

{
	public static Player Instance;
	public Action OnPlayerDie = delegate { };
	GameManager gameManager;
	public int bullets = 15;
	public int money = 20;
	public float hitDistance;
	public float hitAngle = 20;

	public Weapons activeWeapon;

	public enum Weapons
	{
		BAT,
		PISTOL,
		RIFFLE
	}

	public override void Awake()
	{
		base.Awake();
		if (Player.Instance != null)
		{
			Destroy(gameManager);
			return;
		}
		Instance = this;

	}


	public override void Start()
	{

		gameManager = FindObjectOfType<GameManager>();
		base.Start();
		gameManager.SetTextBullets(bullets);
		gameManager.SetTextMoney(money);
		SetBullets(bullets);
		SetWeapon(Weapons.BAT);
	}

	public void Update()
	{
		if (!isLife)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			ChangeWeapon();
		}

		CheckFire();
	}

	public void SetWeapon(Weapons weapon)
	{
		activeWeapon = weapon;
		gameManager.SetSpriteWeapon((int) weapon);

	}

	public void ChangeWeapon()
	{
		activeWeapon++;

		if ((int) activeWeapon > Enum.GetNames(typeof(Weapons)).Length - 1)
		{
			SetWeapon(Weapons.BAT);
			animator.SetTrigger(Weapons.BAT.ToString());
			return;
		}
		SetWeapon(activeWeapon);
		animator.SetTrigger(activeWeapon.ToString());


	}


	private void CheckFire()
	{
		if (Input.GetButton("Fire1") && nextFire <= 0)
		{
			if (bullets < 1)
			{
				Hit();
			}
			else
			{
				Shoot();
				SetBullets(--bullets);
			}			
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
		SetBullets(this.bullets);
	}

	public void SetBullets(int bullets)
	{
		this.bullets = bullets;
		gameManager.SetTextBullets(this.bullets);
		animator.SetInteger("Bullets", bullets);
	}

	public void SetHealth(int health)
	{
		this.health = health;
		gameManager.SetTextHealth(health);

	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(transform.position, hitDistance);
	}

	private void OnDestroy()
	{
			
	}
}