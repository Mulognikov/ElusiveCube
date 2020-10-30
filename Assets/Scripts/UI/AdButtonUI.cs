using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdButtonUI : MonoBehaviour, IListener
{
	private bool gameContinue = true;
	private bool adReady = false;

	private Button adButton;

	private void Start()
	{
		adButton = GetComponentInChildren<Button>();
		adButton.gameObject.SetActive(false);

		EventManager.Instance.AddListener(EventTypes.AdReady, this);
		EventManager.Instance.AddListener(EventTypes.GameContinue, this);
		EventManager.Instance.AddListener(EventTypes.GameStart, this);
		EventManager.Instance.AddListener(EventTypes.GameEnd, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.AdReady:
				OnAdReady();
				break;

			case EventTypes.GameContinue:
				OnGameContinue();
				break;

			case EventTypes.GameStart:
				OnGameStart();
				break;

			case EventTypes.GameEnd:
				OnGameEnd();
				break;
		}
	}

	private void OnAdReady()
	{
		adReady = true;
	}

	private void OnGameContinue()
	{
		gameContinue = true;
		adButton.gameObject.SetActive(false);
	}

	private void OnGameStart()
	{
		gameContinue = false;
		adButton.gameObject.SetActive(false);
	}

	private void OnGameEnd()
	{
		if (gameContinue || !adReady)
		{
			return;
		}

		adButton.gameObject.SetActive(true);
		adReady = false;
		EventManager.Instance.PostNotification(EventTypes.AdShow, this);
	}
}
