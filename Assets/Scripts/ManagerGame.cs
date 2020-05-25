using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerGame : MonoBehaviour
{ 
	public PlayerInput playerInput;
	public PlayerState playerState;
	public GameState gameState;

	//public delegate PlayerInput OnChangePlayerInput(PlayerInput state); public OnChangePlayerInput changePlayerInput;

	private void Start()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Throwable"));
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Throwable"), LayerMask.NameToLayer("Default_CanTeleport")); 
	}
}
