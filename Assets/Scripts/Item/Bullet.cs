using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	public float speed = 20f;
	public int damage = -25;
	Rigidbody2D rb;
	Player player;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		rb.velocity = -transform.up * speed;
	}


	private void OnCollisionEnter2D(Collision2D collision)
	{

		if (collision.gameObject.CompareTag("MachineOfPlayer"))

		{
			MainMachine machine = collision.gameObject.GetComponent<MainMachine>();
			machine.AddHealth(damage);
		}

		if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") )

		{   
			MainCharacter character = collision.gameObject.GetComponent<MainCharacter>();
			character.AddHealth(damage);	
		}

		Destroy(gameObject);
	}

	private void OnBecameInvisible()
	{
		Destroy(gameObject);
	}
}
