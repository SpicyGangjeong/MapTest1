using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowYourPosition : MonoBehaviour
{
    RectTransform rectTransform;
    void Start()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        Debug.Log(gameObject.name + "\n"
            + transform.position + transform.localPosition + "\n"
            + rectTransform.position + rectTransform.localPosition + "\n"
            + rectTransform.anchoredPosition);
        StartCoroutine(afterBroadcast());
    }
    IEnumerator afterBroadcast()
    {
        yield return null;
        rectTransform = gameObject.GetComponent<RectTransform>();
        Debug.Log("Coroutine " + gameObject.name + "\n"
            + transform.position + transform.localPosition + "\n"
            + rectTransform.position + rectTransform.localPosition + "\n"
            + rectTransform.anchoredPosition);
    }
}
