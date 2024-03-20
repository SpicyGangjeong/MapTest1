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
    public LineRenderer lineRendererPrefab;
    public Transform[] linePoints;
    public bool isAvailable = false;

    private void Start()
    {

    }
    public void setMaximumRoom(int maxRoom)
    {
        BeforeRoom = new GameObject[maxRoom];
        AfterRoom = new GameObject[maxRoom];
        linePoints = new Transform[maxRoom];
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
                linePoints[i] = objRoom.transform;
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
        // 각각에 라인 렌더러를 생성하여 라인을 그립니다.
        for (int i = 0; i < linePoints.Length; i++)
        {
            DrawLine(gameObject.transform.position, linePoints[i].position);
        }
    }
    // 두 점 사이에 라인을 그리는 함수
    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        // 라인 렌더러 생성
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, gameObject.transform);

        // 라인 렌더러 속성 설정
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.useWorldSpace = false; // 월드 공간에서 사용할 것인지 여부 설정
    }
}
