#pragma warning disable 0649
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManagerUI : MonoBehaviour
{
	GameObject player;

	[Header("HP Heart")]
	[SerializeField] GameObject groupHealth = null;
	[SerializeField] Sprite heartFull = null;
	[SerializeField] Sprite heartHalf = null;
	[SerializeField] Sprite heartEmpty = null;

	[Header("Teleport Ability")]
	[SerializeField] Image teleportFG = null;
	[SerializeField] Image teleportBG = null;

	[Header("Prompts")]
	[SerializeField] GameObject promptInteractionGroup = null;
	[SerializeField] TextMeshProUGUI promptInteractionButton = null;
	[SerializeField] TextMeshProUGUI promptInteractionAction = null;

	[Header("Scene Intro Initializer")]
	public bool showSceneIntro = true;
	[SerializeField] TextMeshProUGUI groupSceneIntro_Name = null;
	[SerializeField] TextMeshProUGUI groupSceneIntro_Chapter = null;
	[SerializeField] TextMeshProUGUI groupSceneIntro_Objective = null;


	void Awake()
  {
		

		//teleportFG.fillAmount
	}

	private void Start()
	{
		player = StaticStorage.instance.Player;
		player.GetComponent<PlayerController>().changeHealth += UpdateHearts;

		InitializeHearts();
	}

	private void Update()
	{
		teleportFG.fillAmount = StaticStorage.instance.teleportCurrentCooldown / StaticStorage.instance.teleportMaxCooldown;
	}

	public void InitializeHearts()
	{
		// Destroy current hearts
		foreach (Transform child in groupHealth.transform)
		{
			Destroy(child.gameObject);
		}

		// Instantiate childs
		int playerHealth = player.GetComponent<PlayerController>().maxHealth;
		int heartsAmount = playerHealth % 2 == 0 ? playerHealth / 2 : (playerHealth + 1) / 2;

		for (int i = 0; i < heartsAmount; i++)
		{
			GameObject img = new GameObject
			{
				name = $"Heart {i} ",
				layer = LayerMask.NameToLayer("UI")
			};

			img.AddComponent<RectTransform>();
			img.AddComponent<Image>().sprite = heartFull;

			Instantiate(img, groupHealth.transform);
		}
	}

	public void UpdateHearts(int currentHealth)
	{
		bool withHalf = currentHealth % 2 == 0 ? false : true;

		for (int i = groupHealth.transform.childCount - 1; i > -1; i--)
		{
			if (i == currentHealth / 2 && withHalf)
			{
				groupHealth.transform.GetChild(i).GetComponent<Image>().sprite = heartHalf;
			}
			else if (i >= currentHealth / 2)
			{
				groupHealth.transform.GetChild(i).GetComponent<Image>().sprite = heartEmpty;
			}
		}
	}

	public void InteractionPromptSwitchVisibility(bool state)
	{
		//promptInteractionGroup.SetActive(!promptInteractionGroup.activeSelf);

		if (state)
			promptInteractionGroup.LeanScaleX(1, 0.01f);
		else
			promptInteractionGroup.LeanScaleX(0, 0.01f);
	}

	public void InteractionPromptSwitchVisibility(bool state, string action, string button = "F")
	{
		//promptInteractionGroup.SetActive(!promptInteractionGroup.activeSelf);
		promptInteractionButton.SetText(button);
		promptInteractionAction.SetText(action);

		if (state)
			promptInteractionGroup.LeanScaleX(1, 0.01f);
		else
			promptInteractionGroup.LeanScaleX(0, 0.01f);
	}

	public void InitializeSceneIntro(Level passedLevel)
	{
		var _Name = groupSceneIntro_Name;
		var _Chapter = groupSceneIntro_Chapter;
		var _Objective = groupSceneIntro_Objective;

		_Name.		SetText(passedLevel.nameStr);
		_Chapter.	SetText(passedLevel.chapter);
		_Objective.	SetText(passedLevel.objective);

		if (showSceneIntro)
			_Name.transform.parent.localScale = Vector3.one;
		// Debug.Log(_Name.transform.parent.gameObject.name + " " + _Name.transform.parent.gameObject.activeSelf);

		var textMeshes = new List<TextMeshProUGUI>
		{
			_Name,
			_Chapter,
			_Objective
		};

		//for (int i = 0; i < textMeshes.Count; i++)
		//{
		//	var textMesh = textMeshes[i];

		//	if (textMesh.gameObject.GetComponent<BehaviourUI>() == null)
		//		textMesh.gameObject.AddComponent<BehaviourUI>();
		//	var x = textMesh.gameObject.GetComponent<BehaviourUI>();

		//	if (showSceneIntro)
		//		x.FadeText();
		//	else
		//		x.FadeText(0, 0, 0, 0);
		//}

		//foreach (var textMesh in textMeshes)
		//{
		//	if (textMesh.gameObject.GetComponent<BehaviourUI>() == null)
		//		textMesh.gameObject.AddComponent<BehaviourUI>();
		//	var x = textMesh.gameObject.GetComponent<BehaviourUI>();

		//	if (showSceneIntro)
		//		x.FadeText();
		//	else
		//		x.FadeText(0, 0, 0, 0);
		//}
	}
}
