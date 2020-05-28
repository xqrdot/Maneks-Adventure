#pragma warning disable 0649
using UnityEngine;
using System.Collections;

public class StaticStorage : MonoBehaviour
{
	public bool PlayerCanInput { get; set; } = true;


	public GameState gameState;

	[Space(20)]
	public GameObject player;
	public GameObject gameManager;

	[HideInInspector] public float gameTime = 0;
	[HideInInspector] public float gameScore = 0;
	[HideInInspector] public float teleportMaxCooldown;
	[HideInInspector] public float teleportCurrentCooldown;
	
	public static StaticStorage instance;

	public delegate void OnChangeGameTime(float time); public OnChangeGameTime changeGameTime;


	private void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);
	}

	private void Start() {
		StartCoroutine(CountGameTime());
	}

	public GameObject Player
	{
		get
		{
			if (player == null)
				player = GameObject.FindGameObjectWithTag("Player");

			return player;
		}
	}
	public GameObject GameManager
	{
		get
		{
			if (gameManager == null)
				gameManager = GameObject.FindGameObjectWithTag("GameManager");

			return gameManager;
		}
	}

	IEnumerator CountGameTime()
  {
		var increment = 1f;
		while (gameState != GameState.Ended)
		{
			yield return new WaitForSeconds(increment);
			gameTime += increment;
			
			changeGameTime?.Invoke((int)gameTime);
		}

    yield return null;
  }
}
