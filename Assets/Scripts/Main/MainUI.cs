using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour
{
	public Slider healthSlider;
	public MainCharacter mainCharacter;



	private void Start()
	{
		healthSlider.maxValue = mainCharacter.health;
		mainCharacter.HealthChanged += UpdateHealthBar;
	}

	

	public void UpdateHealthBar()
	{
		healthSlider.value = mainCharacter.health;


		if (healthSlider.value < 1)
		{

			HideHealthBar();
		}
	}

	public void HideHealthBar()
	{
		healthSlider.gameObject.SetActive(false);
	}

	void LateUpdate()
	{

		transform.rotation = Quaternion.identity;
	}
}
