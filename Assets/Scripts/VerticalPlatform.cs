using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
	private PlatformEffector2D effector2D;
	public float disableTime = 0.2f;
	float waitTime;
	public LayerMask layerMask;
	private LayerMask initMask;
	Vector2 playerVelocity;

	PlayerController cache_Controller2D;

	private void Start()
	{
		effector2D = GetComponent<PlatformEffector2D>();
		initMask = effector2D.colliderMask;

		cache_Controller2D = StaticStorage.instance.Player.GetComponent<PlayerController>();
	}

	private void Update()
	{
		playerVelocity = cache_Controller2D.PlayerVelocity();

		if (waitTime < disableTime)
		{
			waitTime += Time.deltaTime;

			if (playerVelocity.y <= 0)
				effector2D.colliderMask = layerMask;
			else
				effector2D.colliderMask = initMask;
		}
		else
		{
			if (playerVelocity.y > 0)
				effector2D.colliderMask = layerMask;
			else
				effector2D.colliderMask = initMask;
		}

		if (Input.GetKey(KeyCode.S))
		{
			waitTime = 0f;
		}
		if (Input.GetKey(KeyCode.Space) && waitTime < disableTime)
		{
			waitTime = disableTime;
			effector2D.colliderMask = initMask;
		}
	}
}
