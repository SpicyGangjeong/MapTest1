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
    public Transform[] linePoints;
    public bool isAvailable = false;
    int _maximumRoom;
    int _currentRoomNumber;
    int _currentLevel;

    private void Start()
    {
        
    }
    public void setEnvironment(int currentRoomNumber, int currentLevel, int maxRoom)
    {
        _currentRoomNumber = currentRoomNumber;
        _currentLevel = currentLevel;
        _maximumRoom = maxRoom;
        
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
                setBeforeRoom(objRoom);
                break;
            }
        }

        for (int i = 0; i < linePoints.Length; i++) // ��������Ʈ�� objRoom.transform �Ҵ�
        {
            if (linePoints[i] == null)
            {
                linePoints[i] = objRoom.transform;
                /*Debug.Log("Now Room / RectPos = " + gameObject.name + gameObject.transform.localPosition + "_" + transform.parent.name + transform.parent.GetComponent<RectTransform>().position
                    + "\nobjRoom = " + objRoom.name + objRoom.transform.localPosition + "_" + objRoom.transform.parent.name + objRoom.transform.parent.GetComponent<RectTransform>().position);
                */break;
            }
        }
    }
    void setBeforeRoom(GameObject objRoom)
    {
        GameObject[] objBeforeRoom = objRoom.GetComponent<Room>().BeforeRoom;
        if (objBeforeRoom.Length < _maximumRoom)
        {
            objBeforeRoom = new GameObject[_maximumRoom];
        }
        for (int i = 0; i < objBeforeRoom.Length; i++)
        {
            if (objBeforeRoom[i] == null)
            {
                objBeforeRoom[i] = gameObject;
                break;
            }
        }
    }

    public void startDraw()
    {
        // ������ ���� �̹����� �����Ͽ� ������ �׸��ϴ�.
        for (int i = 0; i < linePoints.Length; i++)
        {
            if (linePoints[i] == null)
            {
                break;
            }
            //Debug.Log(transform.position + "" + linePoints[i].position);
            DrawLine(transform.parent.GetComponent<RectTransform>().position + transform.localPosition, linePoints[i].parent.GetComponent<RectTransform>().position + linePoints[i].localPosition);
        }
    }
    // �� �� ���̿� ������ �׸��� �Լ�
    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        // �������� ���� ���� �Ÿ��� ����մϴ�.
        Vector3 direction = endPos - startPos;
        direction.y = direction.y / 2;
        float distance = direction.magnitude;

        // ���� ������ ����մϴ�.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // ���� �����մϴ�.
        GameObject line = Instantiate(linePrefab, gameObject.transform) as GameObject;
        RectTransform lineRectTransform = line.GetComponent<RectTransform>();

        // ���� ũ�� �� ��ġ, ȸ���� �����մϴ�.
        lineRectTransform.sizeDelta = new Vector2(distance, lineRectTransform.sizeDelta.y);
        lineRectTransform.anchoredPosition = (direction * 0.5f);
        lineRectTransform.rotation = Quaternion.Euler(0, 0, angle);


    }
}
