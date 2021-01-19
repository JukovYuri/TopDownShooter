using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainCharacter : MonoBehaviour
{
	public Action HealthChanged = delegate { };

	public Bullet bulletPrefab;
	public GameObject shootPosition;
	public string layerOfBullets = "default";
	public float fireRate = 1f;
	[HideInInspector] public float nextFire;

	public int health = 100;
	public bool isLife = true;

	[Range(0, 1)] public float chanceOfDropping = 0.5f;
	public GameObject[] prefabDropBonus;

	[HideInInspector] public Animator animator;
	[HideInInspector] public CircleCollider2D circleCollider;
	[HideInInspector] public SpriteRenderer sr;
	[HideInInspector] MainCharacterMoving mainCharacterMoving;


	public virtual void Awake()
	{
		animator = GetComponent<Animator>();
		circleCollider = GetComponent<CircleCollider2D>();
		sr = GetComponent<SpriteRenderer>();
		
		mainCharacterMoving = GetComponent<MainCharacterMoving>();
	}


	public virtual void Start()
	{
		CheckLife();
	}

	public void Shoot()
	{
		//todo sound
		animator.SetTrigger("Shoot");
		if (bulletPrefab && shootPosition)
		{			
			Bullet bullet = Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
			bullet.gameObject.layer = LayerMask.NameToLayer(layerOfBullets);
		}
	}

	public void Hit()
	{
		animator.SetTrigger("Hit");
	}

	public void AddHealth(int health)
	{
		if (isLife)
		{
			this.health += health;
			CheckLife();
		}
	}


	public virtual void CheckLife()
	{
		HealthChanged();
		if (health < 1)
		{
			ToDie();
			DropBonus();
		}
		animator.SetInteger("Health", health);
	}
	public void ToDie()
	{
		isLife = false;
		mainCharacterMoving.StopMove();
		circleCollider.enabled = false;
	}

	public void DropBonus()
	{

		if (Random.value <= chanceOfDropping)
		{
			int index = Random.Range(0, prefabDropBonus.Length);
			if (prefabDropBonus.Length > 0 && prefabDropBonus[index] != null)
			{
				Instantiate(prefabDropBonus[index], transform.position, Quaternion.identity);
			}		
		}
	}


}
