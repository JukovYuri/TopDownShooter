using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacterMoving : MonoBehaviour
{
	public float speed = 10f;
	[HideInInspector] public Rigidbody2D rb;
	[HideInInspector] public Animator animator;
	public virtual void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}


	public void Move(Vector3 direction)
	{

		if (direction.magnitude > 1)
		{
			direction = direction.normalized;
		}

		animator.SetFloat("Speed", direction.magnitude);
		rb.velocity = direction.normalized * speed;
	}

	public void Rotate(Vector3 targetPosition)
	{
		Vector3 objectPosition = transform.position;
		Vector3 direction = targetPosition - objectPosition;
		direction.z = 0; 
		transform.up = -direction;
	}

	public void StopMove()
	{
		Move(Vector3.zero);
	}
}
