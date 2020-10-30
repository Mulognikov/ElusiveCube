using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasMenuDisabler : MonoBehaviour, IListener
{
	private Canvas canvasMenu;

	private void Start()
	{
		canvasMenu = GetComponent<Canvas>();
		EventManager.Instance.AddListener(EventTypes.PlayerReady, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.PlayerReady:
				OnPlayerReady();
				break;
		}
	}

	private void OnPlayerReady()
	{
		canvasMenu.enabled = false;
	}
}
