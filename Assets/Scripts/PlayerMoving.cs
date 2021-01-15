using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MainCharacterMoving
{
	Player player;

	public override void Awake()
	{
		base.Awake();
		player = GetComponent<Player>();
	}


	void Update()
	{
		if (!player.isLife)
		{
			return;
		}

		Vector3 direction = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
		Move(direction);
		Rotate(Camera.main.ScreenToWorldPoint(Input.mousePosition));
	}

}
