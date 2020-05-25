#pragma warning disable 0649
using UnityEngine;

public class StaticStorage : MonoBehaviour
{
	public bool PlayerCanInput { get; set; } = true;

	public GameObject player;
	public GameObject gameManager;
	public float teleportMaxCooldown;
	public float teleportCurrentCooldown;

	public static StaticStorage instance;

	private void Awake()
	{
		if (instance == null)
			instance = this;

		else if (instance != this)
			Destroy(gameObject);
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
}
