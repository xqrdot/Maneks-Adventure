using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BehaviourUI : MonoBehaviour
{

	TextMeshProUGUI textMesh;

	private void Start()
	{
		textMesh = GetComponent<TextMeshProUGUI>();
	}

	public void FadeText(float timeFadeIn = 4, float timeFadeOut = 3, float timeHold = 4, float timeWait = 0)
	{
		StartCoroutine(IFadeText(timeFadeIn, timeFadeOut, timeHold, timeWait));
	}

	private IEnumerator IFadeText(float timeFadeIn, float timeFadeOut, float timeHold, float timeWait)
	{
		if (timeWait != 0)
		{
			textMesh.canvasRenderer.SetAlpha(0.001f);
			yield return new WaitForSeconds(timeWait);
		}

		if (timeFadeIn != 0)
		{
			textMesh.canvasRenderer.SetAlpha(0.001f);
			textMesh.CrossFadeAlpha(1.0f, timeFadeIn, false);
			yield return new WaitForSeconds(timeFadeIn);
		}

		if (timeHold != 0)
		{
			textMesh.canvasRenderer.SetAlpha(1.0f);
			yield return new WaitForSeconds(timeHold);
		}

		if (timeFadeOut != 0)
		{
			textMesh.CrossFadeAlpha(0.001f, timeFadeOut, false);
			yield return new WaitForSeconds(timeFadeOut);
		}

		textMesh.canvasRenderer.SetAlpha(0.001f);
		yield return null;
	}
}
