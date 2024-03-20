using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class forDrawExample : MonoBehaviour
{
    [SerializeField]
    RectTransform StartUI;
    [SerializeField]
    RectTransform EndUI;
    [SerializeField]
    GameObject linePrefab;
    private void Start()
    {
        GameObject line = Instantiate(linePrefab, gameObject.transform) as GameObject;

        Vector3 startPos = StartUI.position;
        Vector3 endPos = EndUI.position;

        // 시작점과 끝점 간의 거리를 계산합니다.
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;

        // 선의 각도를 계산합니다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        RectTransform lineRectTransForm = line.GetComponent<RectTransform>();

        // 선의 위치 및 회전을 설정합니다.
        lineRectTransForm.sizeDelta = new Vector2(distance, lineRectTransForm.sizeDelta.y);
        lineRectTransForm.anchoredPosition = startPos + direction * 0.5f;
        lineRectTransForm.rotation = Quaternion.Euler(0, 0, angle);
    }
       
}
