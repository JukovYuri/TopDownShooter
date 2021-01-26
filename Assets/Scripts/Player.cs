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

	public float radiusVisibility;
	public float angleVisibility;

	public float hitDistance;
	public float hitAngle = 20;

	public Weapons activeWeapon;

	public LayerMask whatIsEnemy;
	public LayerMask whatIsObstacles;

	Collider2D[] enemyColliders;

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

		CheckEnemyVisiblity();




		if (Input.GetKeyDown(KeyCode.Space))
		{
			ChangeWeapon();
		}

		CheckFire();
	}


	public void CheckEnemyVisiblity()
	{


		enemyColliders = Physics2D.OverlapCircleAll(transform.position, radiusVisibility, whatIsEnemy);

		foreach (Collider2D item in enemyColliders)
		{
			Vector2 itemDirection = item.transform.position - transform.position;

			if (Vector2.Angle(-transform.up, itemDirection) < angleVisibility/2)
			{
				if (!Physics2D.Raycast(transform.position, itemDirection, itemDirection.magnitude, whatIsObstacles))
				{
					item.GetComponent<SpriteRenderer>().enabled = true;
					item.gameObject.GetComponentInChildren<Canvas>().enabled = true;
				}

			}

		}

	}

	public void SetWeapon(Weapons weapon)
	{
		activeWeapon = weapon;
		gameManager.SetSpriteWeapon((int)weapon);

	}

	public void ChangeWeapon()
	{
		activeWeapon++;

		if ((int)activeWeapon > Enum.GetNames(typeof(Weapons)).Length - 1)
		{
			SetWeapon(Weapons.BAT);
			animator.SetTrigger(Weapons.BAT.ToString());
			return;
		}
		SetWeapon(activeWeapon);
		animator.SetTrigger(activeWeapon.ToString());


	}


	public override void Hit()
	{
		base.Hit();

		Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, hitDistance, whatIsEnemy);

		foreach (Collider2D item in col)
		{
			Vector2 direction = item.transform.position - transform.position;
			if (Vector2.Angle(-transform.up, direction) > hitAngle / 2)
			{
				return;
			}
			item.GetComponent<Rigidbody2D>().AddForce(item.transform.up * 2000f);
		}
	}

	private void CheckFire()
	{
		if (Input.GetButton("Fire1") && nextFire <= 0)
		{
			nextFire = fireRate;

			if (activeWeapon == Weapons.BAT)
			{
				Hit();
				return;
			}

			if (bullets < 1)
			{
				SetWeapon(Weapons.BAT);
				Hit();
				return;
			}

			Shoot();
			SetBullets(--bullets);
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

	private void OnDrawGizmosSelected()
	{   // угол удара битой
		Gizmos.color = Color.magenta;
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, hitAngle) * -transform.up * hitDistance);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -hitAngle) * -transform.up * hitDistance);

		// видимость
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(transform.position, radiusVisibility);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0,  angleVisibility/2) * -transform.up * radiusVisibility);
		Gizmos.DrawRay(transform.position, Quaternion.Euler(0, 0, -angleVisibility/2) * -transform.up * radiusVisibility);

	}

}