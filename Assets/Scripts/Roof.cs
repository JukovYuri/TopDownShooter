using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roof : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			SetActiveRoof(false);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
			SetActiveRoof(true);
		}
	}

	void SetActiveRoof(bool active)
	{
		SpriteRenderer[] roofs = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer item in roofs)
		{
			item.enabled = active;
		}
	}

}
