using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TopScoreUI : MonoBehaviour, IListener
{
	private TextMeshProUGUI text;

	private void Start()
	{
		text = GetComponent<TextMeshProUGUI>();

		EventManager.Instance.AddListener(EventTypes.TopScoreChange, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.TopScoreChange:
				OnTopScoreChange((int)param);
				break;
		}
	}

	private void OnTopScoreChange(int score)
	{
		text.text = "  TOP: " + score.ToString();
	}
}
