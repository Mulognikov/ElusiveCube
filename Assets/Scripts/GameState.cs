using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour, IListener
{
	public Canvas CanvasMenu;
	private bool game = false;

	private void Start()
	{
		Application.targetFrameRate = 60;

		EventManager.Instance.AddListener(EventTypes.ScreenTouch, this);
		EventManager.Instance.AddListener(EventTypes.GameContinue, this);

		StartCoroutine(Coroutine());
		IEnumerator Coroutine()
		{
			yield return null;
			EventManager.Instance.PostNotification(EventTypes.GameInit, this);
		}
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.ScreenTouch:
				OnScreenTouched();
				break;

			case EventTypes.GameContinue:
				OnGameContinue();
				break;
		}
	}

	private void OnScreenTouched()
	{
		if (game)
		{
			return;
		}

		game = true;
		EventManager.Instance.PostNotification(EventTypes.GameStart, this);
	}

	private void OnGameContinue()
	{
		game = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		game = false;
		CanvasMenu.enabled = true;
		EventManager.Instance.PostNotification(EventTypes.GameEnd, this);
	}
}
