using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class Bomb : MonoBehaviour
{
	public int damage = -25;
	public float damageRadius;
	public LayerMask damageLayers;
	public GameObject prefabExplosionEffect;

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Bullet"))
		{
			Explode();
			Destroy(collision.gameObject);
			Destroy(gameObject);
		}
	}

	private void Explode()
	{
		SetDamage();
		Instantiate(prefabExplosionEffect, transform.position, Quaternion.identity);
	}

	void SetDamage()
	{
		Collider2D[] collidersDamage = Physics2D.OverlapCircleAll(transform.position, damageRadius, damageLayers);

		foreach (Collider2D item in collidersDamage)
		{
			item.GetComponent<MainCharacter>().AddHealth(damage);
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, damageRadius);
	}
}
