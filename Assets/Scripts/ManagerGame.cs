using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{ 
	public PlayerInput playerInput;
	public PlayerState playerState;
	public GameState gameState;

	AudioSource source = null;

	//public delegate PlayerInput OnChangePlayerInput(PlayerInput state); public OnChangePlayerInput changePlayerInput;

	private void Start()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Throwable"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Throwable"), LayerMask.NameToLayer("Default_CanTeleport")); 

		ManagerEvents.current.onGlobalEventChange += OnGlobalEventChange;

		source = StaticStorage.instance.GameManager.GetComponent<AudioSource>();
	}

	private void OnGlobalEventChange(int id)
	{
		if (id == 3)
		{
			print("[INFO] Player's dead. Bruh.");
			// Move player and give him Health
			var p = StaticStorage.instance.player;
			var p_PlayerController = p.GetComponent<PlayerController>();

			p.gameObject.transform.position = StaticStorage.instance.respawnPosition.position;
			p_PlayerController.currentHealth = p_PlayerController.maxHealth;
			p_PlayerController.changeHealth(p_PlayerController.maxHealth);
		}
	}
}
