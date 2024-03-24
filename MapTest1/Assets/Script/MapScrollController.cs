using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapScrollController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public float scrollSpeed = 10f;

    void Update()
    {
        float scrollInput = Input.GetAxis("Vertical");
        float newYPos = content.anchoredPosition.y + scrollInput * scrollSpeed * Time.deltaTime;
        newYPos = Mathf.Clamp(newYPos, 0f, content.sizeDelta.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y);

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, newYPos);
    }
}
