using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MapScrollController : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;

    void Update()
    {
        float scrollSpeed = 25000f;
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        float newYPos = content.anchoredPosition.y + Math.Sign(-scrollInput) * scrollSpeed * Time.deltaTime;
        newYPos = Mathf.Clamp(newYPos, 0f, content.sizeDelta.y - scrollRect.GetComponent<RectTransform>().sizeDelta.y); // 화면이 벗어나는걸 방지하는 부분

        content.anchoredPosition = new Vector2(content.anchoredPosition.x, newYPos); // 화면을 실제로 이동시키는 부분
    }
}
