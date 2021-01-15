using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MainItem : MonoBehaviour
{
	public int bonus = 20;
	[Space]
	[Header("Animation param")]
	public float delta = 1.5f;
	public float speed = 1f;
	private int direction = 1;
	[HideInInspector] public Player player;

	public virtual void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			player = collision.gameObject.GetComponent<Player>();
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		TransformScale();
	}

	private void TransformScale()
	{
		if (transform.localScale.x > delta || transform.localScale.x < 1 / delta)
		{
			direction = -direction;
		}

		float change = Time.deltaTime * speed * direction;
		Vector3 scaleChange = new Vector3(change, change, 1);
		transform.localScale += scaleChange;
	}

}

