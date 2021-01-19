using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
	public Bullet bulletPrefab;
	public GameObject shootPosition;
	private string layerMaskName = "PlayerBullet";
	public float shootingAngle = 14f;

	[HideInInspector] public SpriteRenderer sr;
	[HideInInspector] public bool isTurretReady;

	public float fireRate = 1f;
	[HideInInspector] public float nextFire;

	[HideInInspector] public Animator animator;
	Player player;
	MainMachine machine;

	public void Awake()
	{
		animator = GetComponent<Animator>();
		sr = GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		player = FindObjectOfType<Player>();
		machine = GetComponentInParent<MainMachine>();
	}



	void Update()
	{
		if (isTurretReady)
		{
			Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));//ограничить

			CheckFire();

			return;
		}

		transform.up = machine.transform.up;
	}

	public void Rotate(Vector3 targetPosition)
	{
		Vector3 objectPosition = transform.position;
		Vector3 direction = targetPosition - objectPosition;
		direction.z = 0;

		float angle = Vector2.SignedAngle(machine.transform.up, -direction);
		print(angle);
		if (angle > shootingAngle)
		{
			transform.rotation = Quaternion.Euler(0, 0, shootingAngle);
			return;
		}

		if (angle < -shootingAngle)
		{
			transform.rotation = Quaternion.Euler(0, 0, -shootingAngle);
			return;
		}

		transform.up = -direction;


	}


	public void TurretControlOn() //запуск из аниматора, после анимации подключения башни
	{
		isTurretReady = true;
	}

	public void TurretControlOff() //запуск из аниматора, перед отключением башни
	{
		isTurretReady = false;
	}

	public void TurretHide()
	{
		sr.enabled = false;
	}

	public void Shoot()
	{
		//todo sound
		animator.SetTrigger("Shoot");
		if (bulletPrefab && shootPosition)
		{

			Bullet bullet = Instantiate(bulletPrefab, shootPosition.transform.position, transform.rotation);
			bullet.gameObject.layer = LayerMask.NameToLayer(layerMaskName);
		}
	}

	private void CheckFire()
	{
		if (Input.GetButton("Fire1") && nextFire <= 0)
		{
			if (player.bullets < 1)
			{
				return;
			}
			else
			{
				Shoot();
				player.SetBullets(--player.bullets);
			}

			nextFire = fireRate;
		}

		if (nextFire > 0)
		{
			nextFire -= Time.deltaTime;
		}
	}
}
