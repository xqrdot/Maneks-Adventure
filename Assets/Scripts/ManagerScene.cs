#pragma warning disable 0414
#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ManagerScene : MonoBehaviour
{
	[SerializeField] private Level level;

	GameObject gameManager;
	Level currentLevel;

	private void Awake()
	{
		
	}
	private void Start()
	{
		gameManager = StaticStorage.instance.GameManager;
		currentLevel = level;

		LoadLevelInitials();
	}

	private void LoadLevelInitials()
	{
		gameManager.GetComponent<ManagerUI>().InitializeSceneIntro(currentLevel);
	}
}

