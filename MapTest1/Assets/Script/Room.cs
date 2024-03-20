using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField]
    public Types.RoomType _roomType = Types.RoomType.Battle;
    public GameObject[] BeforeRoom;
    public GameObject[] AfterRoom;
    [SerializeField]
    public GameObject linePrefab;
    public RectTransform[] linePoints;
    public bool isAvailable = false;
    RectTransform rectTransform;

    private void Start()
    {
        
    }
    public void setMaximumRoom(int maxRoom)
    {
        BeforeRoom = new GameObject[maxRoom];
        AfterRoom = new GameObject[maxRoom];
        linePoints = new RectTransform[maxRoom]; 
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void setNextRoom(GameObject objRoom)
    {
        for (int i = 0; i < AfterRoom.Length; i++) // 빈방위치에 방 할당
        {
            if (AfterRoom[i] == null)
            {
                AfterRoom[i] = objRoom;
                setBeforeRoom(gameObject);
                break;
            }
        }

        for (int i = 0; i < linePoints.Length; i++) // 라인포인트에 objRoom.transform 할당
        {
            if (linePoints[i] == null)
            {
                linePoints[i] = objRoom.GetComponent<RectTransform>();
                Debug.Log("Now Room / RectPos = " + gameObject.name + rectTransform.position + "\nobjRoom = " + objRoom.name + objRoom.GetComponent<RectTransform>().position);
                break;
            }
        }
    }
    void setBeforeRoom(GameObject objRoom)
    {
        for (int i = 0; i < BeforeRoom.Length; i++)
        {
            if (BeforeRoom[i] == null)
            {
                BeforeRoom[i] = objRoom;
                break;
            }
        }
    }

    public void startDraw()
    {
        // 각각에 라인 이미지를 생성하여 라인을 그립니다.
        for (int i = 0; i < linePoints.Length; i++)
        {
            if (linePoints[i] == null)
            {
                break;
            }
            DrawLine(rectTransform.position, linePoints[i].position);
        }
    }
    // 두 점 사이에 라인을 그리는 함수
    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        // 시작점과 끝점 간의 거리를 계산합니다.
        Vector3 direction = endPos - startPos;
        float distance = direction.magnitude;

        // 선의 각도를 계산합니다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject line = Instantiate(linePrefab, gameObject.transform) as GameObject;
        RectTransform lineRectTransForm = line.GetComponent<RectTransform>();



        // 선의 위치 및 회전을 설정합니다.
        lineRectTransForm.sizeDelta = new Vector2(distance, lineRectTransForm.sizeDelta.y);
        lineRectTransForm.anchoredPosition = startPos + direction * 0.5f;
        lineRectTransForm.rotation = Quaternion.Euler(0, 0, angle);

    }
}
