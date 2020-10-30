using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IListener
{
	void OnEvent(EventTypes eventType, Component sender, object param = null);
}

