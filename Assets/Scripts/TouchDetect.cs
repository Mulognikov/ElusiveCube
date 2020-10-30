using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchDetect : MonoBehaviour
{
	private enum directions
	{
		Left,
		Right
	}

	[SerializeField] private directions MoveDirection;

	public void OnClick()
	{
		if (MoveDirection == directions.Left)
		{
			EventManager.Instance.PostNotification(EventTypes.ScreenTouch, this, -1f);
		}
		else
		{
			EventManager.Instance.PostNotification(EventTypes.ScreenTouch, this, 1f);
		}
		
	}
}
