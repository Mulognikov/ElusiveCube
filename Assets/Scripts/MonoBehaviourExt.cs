using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourExt : MonoBehaviour
{
	private Coroutine currentCoroutine;

	protected void SwitchCoroutine(IEnumerator start = null)
	{
		if (currentCoroutine != null)
		{
			StopCoroutine(currentCoroutine);
		}

		if (start != null)
		{
			currentCoroutine = StartCoroutine(start);
		}
	}
}
