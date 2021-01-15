using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoofTilemap : MonoBehaviour
{

	TilemapRenderer tilemapRenderer;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		tilemapRenderer = GetComponent<TilemapRenderer>();
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
		
		tilemapRenderer.enabled = active;

	}

}
