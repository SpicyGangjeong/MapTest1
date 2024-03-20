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
        for (int i = 0; i < AfterRoom.Length; i++) // �����ġ�� �� �Ҵ�
        {
            if (AfterRoom[i] == null)
            {
                AfterRoom[i] = objRoom;
                setBeforeRoom(gameObject);
                break;
            }
        }

        for (int i = 0; i < linePoints.Length; i++) // ��������Ʈ�� objRoom.transform �Ҵ�
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
        // ������ ���� �������� �����Ͽ� ������ �׸��ϴ�.
        for (int i = 0; i < linePoints.Length; i++)
        {
            DrawLine(gameObject.transform.position, linePoints[i].position);
        }
    }
    // �� �� ���̿� ������ �׸��� �Լ�
    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        // ���� ������ ����
        LineRenderer lineRenderer = Instantiate(lineRendererPrefab, gameObject.transform);

        // ���� ������ �Ӽ� ����
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
        lineRenderer.useWorldSpace = false; // ���� �������� ����� ������ ���� ����
    }
}
