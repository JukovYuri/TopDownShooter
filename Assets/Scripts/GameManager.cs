using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public Text textHealth;
	public Text textMoney;
	public Text textBullets;
	public GameObject panelGameOver;
	public int countDown;
	public Text textCountDown;
	LevelManager levelManager;
	Coroutine coroutineRestart;

	public Image imageWeapon;
	public Image imageWeaponParent;

	public Sprite[] weapons;

	Player player;

	private void Awake()
	{
		levelManager = GetComponent<LevelManager>();
	}
	private void Start()
	{
		panelGameOver.SetActive(false);
		player = FindObjectOfType<Player>();
		player.OnPlayerDie += RestartGame;
	}

	public void SetSpriteWeapon(int weapon)
	{
		imageWeapon.sprite = weapons[weapon];
		imageWeapon.SetNativeSize();
		imageWeaponParent.rectTransform.sizeDelta = 
		new Vector2(imageWeapon.rectTransform.rect.width + 700f, imageWeaponParent.rectTransform.rect.height);
	}

	public void SetTextHealth(int health)
	{
		textHealth.text = $"{health}";
	}

	public void SetTextMoney(int money)
	{
		textMoney.text = $"х {money}";
	}
	
	public void SetTextBullets(int bullets)
	{
		textBullets.text = $"х {bullets}";
	}

	public void RestartGame() 
	{
		coroutineRestart = StartCoroutine(Restart());
	}

	IEnumerator Restart()
	{
		panelGameOver.SetActive(true);

		for (int i = countDown; i >= 1; i--)
		{
			textCountDown.text = i.ToString();
			yield return new WaitForSeconds(1f);
		}
		levelManager.LoadLevel();

	}


}
