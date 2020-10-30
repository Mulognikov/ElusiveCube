using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour, IListener
{
	private int score
	{
		get { return _score; }
		set
		{
			_score = value;
			EventManager.Instance.PostNotification(EventTypes.ScoreChange, this, _score);
		}
	}

	private int _score;

	private void Start()
	{
		EventManager.Instance.AddListener(EventTypes.GameStart, this);
		EventManager.Instance.AddListener(EventTypes.SuccessJump, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.GameStart:
				OnGameStart();
				break;

			case EventTypes.GameContinue:
				OnGameContinue();
				break;

			case EventTypes.SuccessJump:
				OnSuccessJump();
				break;
		}
	}

	private void OnGameStart()
	{
		score = 0;
	}

	private void OnGameContinue()
	{
		score = score;
	}

	private void OnSuccessJump()
	{
		score++;
	}
}
