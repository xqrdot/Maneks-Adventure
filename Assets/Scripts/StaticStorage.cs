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

	[HideInInspector] public Transform respawnPosition = null;
	[HideInInspector] public float gameTime = 0;
	[HideInInspector] public float gameScore = 0;
	[HideInInspector] public float teleportMaxCooldown;
	[HideInInspector] public float teleportCurrentCooldown;
	
	public static StaticStorage instance;

	public delegate void OnChangeGameTime(float time); public OnChangeGameTime changeGameTime;
	public delegate void PlayerHealth_Delegate(float health); public PlayerHealth_Delegate PlayerHealth_delegate;


	private void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);
	}

	private void Start() {
		StartCoroutine(CountGameTime());

		if (respawnPosition == null) {
			SetRespawnPosition(player.transform);
		}
	}

	public GameObject Player {
		get {
			if (player == null)
				player = GameObject.FindGameObjectWithTag("Player");

			return player;
		}
	}
	public GameObject GameManager {
		get {
			if (gameManager == null)
				gameManager = GameObject.FindGameObjectWithTag("GameManager");

			return gameManager;
		}
	}

	public void SetRespawnPosition(Transform position)
	{
		respawnPosition = position;
		print("[INFO] Respawn position set to:" + respawnPosition.position);
	}

	IEnumerator CountGameTime() 
	{
		var increment = 1f;
		while (gameState != GameState.Ended) {
			yield return new WaitForSeconds(increment);
			gameTime += increment;
			
			changeGameTime?.Invoke((int)gameTime);
		}
		yield return null;
	}
}
