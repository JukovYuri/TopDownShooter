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


	public void SetTextHealth(int health)
	{
		textHealth.text = $"Health: {health}";
	}

	public void SetTextMoney(int money)
	{
		textMoney.text = $"Money: {money}";
	}
	
	public void SetTextBullets(int bullets)
	{
		textBullets.text = $"Bullets: {bullets}";
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
