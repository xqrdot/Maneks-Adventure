#pragma warning disable 0414
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Level", menuName = "Scriptable Objects/Level")]
public class Level : ScriptableObject
{
	[SerializeField] public string nameStr = "LevelInfo.nameStr";
	[SerializeField] public string chapter = "LevelInfo.chapter";
	[SerializeField] public string objective = "LevelInfo.objective";

	//[SerializeField] public SceneAsset scene;
}
