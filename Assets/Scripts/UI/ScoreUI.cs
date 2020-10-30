using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour, IListener
{
	protected TextMeshProUGUI text;

	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		EventManager.Instance.AddListener(EventTypes.ScoreChange, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.ScoreChange:
				OnScoreChange((int)param);
				break;
		}
	}

	protected virtual void OnScoreChange(int score)
	{
		text.text = score.ToString();
	}
}
