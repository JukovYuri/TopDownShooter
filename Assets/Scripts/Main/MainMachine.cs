using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainMachine : MainCharacterMoving
{
	public int health = 50;
	public int coeffRotate = 3;

	[HideInInspector] public int playerHealthOnEnter;

	public int bonusBullets = 100;
	[HideInInspector] public int playerBulletsOnEnter;

	public bool isLife = true;

	public string tagName = "MachineOfPlayer";

	public Sprite[] platformStateSprites;

	private bool isPlayerNearby;
	private bool isMachinesDriver;
	private bool isMachinesShooter;
	[HideInInspector] public Turret turret;
	Player player;
	SpriteRenderer sr;
	LayerMask layerOfMachine;
	string tagOfMachine;


	public override void Awake()
	{
		base.Awake();
		sr = GetComponent<SpriteRenderer>();
	}
	
	private void Start()
	{
		player = FindObjectOfType<Player>();

		turret = GetComponentInChildren<Turret>();

		animator.enabled = false;
		layerOfMachine = gameObject.layer;
		tagOfMachine = gameObject.tag;
	}

	private void Update()
	{
		if (!isLife)
		{
			return;
		}

		SetMachineDriver();
		SetMachineShooter();

		if (!isMachinesDriver)
		{
			return;
		}

		if (Input.GetAxis("Vertical") != 0)
		{
			transform.Rotate(0f, 0f, -Input.GetAxis("Horizontal")/2);
		}

		Move(transform.up * Input.GetAxis("Vertical"));

		player.transform.position = transform.position;
	}

	public void MachineOn(bool state)
	{
		isMachinesDriver = state;
		animator.enabled = state;
		player.gameObject.SetActive(!state);

		if (state)
		{
			gameObject.layer = player.gameObject.layer;
			gameObject.tag = tagName;

			playerHealthOnEnter = player.health;
			player.SetHealth(health);

			playerBulletsOnEnter = player.bullets;
			player.AddBullets(bonusBullets);
		}
		else
		{
			StopMove();
			gameObject.layer = layerOfMachine;
			gameObject.tag = tagOfMachine;

			if (player.bullets > playerBulletsOnEnter)
			{
				bonusBullets = player.bullets - playerBulletsOnEnter;
				player.SetBullets(playerBulletsOnEnter);
			}
			else
			{
				bonusBullets = 0;
			}

			player.SetHealth(playerHealthOnEnter);
		}
	}

	public void MachineDeath() // для аниматора
	{
		MachineOn(false);
		turret.TurretHide();
	}

	private void SetMachineDriver()
	{

		if (isPlayerNearby && Input.GetKeyDown(KeyCode.Tab))
		{
			MachineOn(true);
			return;
		}

		if (isMachinesDriver && Input.GetKeyDown(KeyCode.Tab))
		{
			MachineOn(false);
			return;
		}
	}

	private void SetMachineShooter()
	{
		if (isMachinesDriver && Input.GetKeyDown(KeyCode.Return))
		{
			StopMove();
			TurretOn();
			return;
		}

		if (isMachinesShooter && Input.GetKeyDown(KeyCode.Return))
		{

			TurretOff();
			return;
		}
	}


	public virtual void TurretOn()
	{
		isMachinesShooter = true;
		isMachinesDriver = false;
		turret.animator.SetBool("ShooterOn", true);
	}

	public virtual void TurretOff()
	{
		StopMove();
		isMachinesShooter = false;
		isMachinesDriver = true;
		turret.animator.SetBool("ShooterOn", false);


	}



	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			isPlayerNearby = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			isPlayerNearby = false;
		}
	}

	public void AddHealth(int health)
	{
		if (isLife)
		{
			this.health += health;
			player.SetHealth(this.health);
			CheckLife();
		}

	}

	public void CheckLife()
	{
		if (health < 1)
		{
			ToDie();
			sr.sprite = platformStateSprites[4];
			
			return;
		}

		if (health > 75)
		{
			sr.sprite = platformStateSprites[0];
			return;
		}

		if (health > 50 && health <= 75)
		{
			sr.sprite = platformStateSprites[1];
			return;
		}

		if (health > 25 && health <= 50)
		{
			sr.sprite = platformStateSprites[2];
			return;
		}

		if (health > 0 && health <= 25)
		{
			sr.sprite = platformStateSprites[3];
			return;
		}

	}

	private void ToDie()
	{
		isLife = false;

		if (isMachinesDriver)
		{
			MachineOn(false);
			return;
		}
		if (isMachinesShooter)
		{
			TurretOff();
			Invoke(nameof(MachineDeath), 2f);
		}


	}
	//если патроны закончились
}