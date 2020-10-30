using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShowHideText : MonoBehaviourExt, IListener
{
	public EventTypes[] ShowEvent;
	public EventTypes[] HideEvent;
	public float ShowLatency = 0;
	public float HideLatency = 0;
	public float speed = 3f;

	private TextMeshProUGUI text;

	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		foreach (EventTypes eventType in ShowEvent)
		{
			EventManager.Instance.AddListener(eventType, this);
		}

		foreach (EventTypes eventType in HideEvent)
		{
			EventManager.Instance.AddListener(eventType, this);
		}
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{

		foreach (EventTypes eventTypes in ShowEvent)
		{
			if (eventType == eventTypes)
			{
				ShowText();
			}
		}

		foreach (EventTypes eventTypes in HideEvent)
		{
			if (eventType == eventTypes)
			{
				HideText();
			}
		}
	}
	
	public void ShowText()
	{
		SwitchCoroutine(ChangeTextAlpha(1, ShowLatency));
	}

	public void HideText()
	{
		SwitchCoroutine(ChangeTextAlpha(0, HideLatency));
	}

	IEnumerator ChangeTextAlpha(float alpha, float latency)
	{
		yield return new WaitForSeconds(latency);
		float time = 0;

		while (!Mathf.Approximately(text.color.a, alpha))
		{
			time += Time.deltaTime;
			float a = Mathf.Lerp(1 - alpha, alpha, time * speed);
			text.color = new Color(text.color.r, text.color.g, text.color.b, a);
			yield return null;
		}
	}
}
