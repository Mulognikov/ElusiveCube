using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour, IListener
{
    void Start()
    {
        int tutorialState = PlayerPrefs.GetInt("TutorialState");

        if (tutorialState > 0)
        {
            Destroy(gameObject);
            return;
        }

        EventManager.Instance.AddListener(EventTypes.SuccessJump, this);
    }

    public void OnEvent(EventTypes eventType, Component sender, object param = null)
    {
        switch (eventType)
        {
            case EventTypes.SuccessJump:
                OnSuccessJump();
                break;
        }
    }

    private void OnSuccessJump()
	{
        StartCoroutine(TutorialComplited());
	}

    private IEnumerator TutorialComplited()
	{
        PlayerPrefs.SetInt("TutorialState", 1);
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
        EventManager.Instance.RemoveRedundancies();
    }
}
