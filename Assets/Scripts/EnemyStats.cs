#pragma warning disable 0414
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy Stats", menuName = "Scriptable Objects/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
	public new string name;
	public int maxHealth;

	[Header("Audio")]
	[SerializeField] public AudioClip hurtBig;
	[SerializeField] public AudioClip hurtSmall;
}
