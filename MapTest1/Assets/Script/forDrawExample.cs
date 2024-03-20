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

        // �������� ���� ���� �Ÿ��� ����մϴ�.
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;

        // ���� ������ ����մϴ�.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        RectTransform lineRectTransForm = line.GetComponent<RectTransform>();

        // ���� ��ġ �� ȸ���� �����մϴ�.
        lineRectTransForm.sizeDelta = new Vector2(distance, lineRectTransForm.sizeDelta.y);
        lineRectTransForm.anchoredPosition = startPos + direction * 0.5f;
        lineRectTransForm.rotation = Quaternion.Euler(0, 0, angle);
    }
       
}
