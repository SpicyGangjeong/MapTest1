using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Room : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    [SerializeField]
    public Types.RoomType _roomType;
    public GameObject[] BeforeRoom; 
    public GameObject[] AfterRoom; 
    [SerializeField]
    public GameObject linePrefab;
    public Transform[] linePoints;
    public bool isAvailable = false;
    public Sprite bossSprite;
    public Sprite battleSprite;
    public Sprite eliteSprite;
    public Sprite treasureSprite;
    public Sprite eventSprite;
    public Sprite mapCompleteSprite;
    public Sprite restSprite;
    public Sprite merchantSprite;
    
    int _maximumRoom;
    int _currentRoomNumber;
    int _currentLevel;
    public Vector3 minScale = new Vector3(1f, 1f, 1f);
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);
    public float duration = 1f;
    public Coroutine breathCoroutine;
    public bool breathControlFlag = true;   


    private void Start()
    {
        StartCoroutine(BuildRoom());
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
        for (int i = 0; i < AfterRoom.Length; i++) // 빈방위치에 방 할당
        {
            if (AfterRoom[i] == null)
            {
                AfterRoom[i] = objRoom;
                setBeforeRoom(objRoom);
                break;
            }
        }

        for (int i = 0; i < linePoints.Length; i++) // 라인포인트에 objRoom.transform 할당
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
        // 각각에 라인 이미지를 생성하여 라인을 그립니다.
        for (int i = 0; i < linePoints.Length; i++)
        {
            if (linePoints[i] == null)
            {
                break;
            }
            //Debug.Log(transform.position + "" + linePoints[i].position);
            DrawLine(transform.parent.GetComponent<RectTransform>().position + transform.localPosition , linePoints[i].parent.GetComponent<RectTransform>().position + linePoints[i].localPosition + (Vector3.up * 5));
        }
    }
    // 두 점 사이에 라인을 그리는 함수
    void DrawLine(Vector3 startPos, Vector3 endPos)
    {
        // 시작점과 끝점 간의 거리를 계산합니다.
        Vector3 direction = endPos - startPos;
        // direction.y = direction.y / 2;   //  <<<=== Y값 터지면 조절해야함. TODO

        float distance = direction.magnitude;

        // Debug.Log(startPos + "\n" + endPos + "\n" + direction + "\n" + distance);
        // 선의 각도를 계산합니다.
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 선을 생성합니다.
        GameObject line = Instantiate(linePrefab, gameObject.transform) as GameObject;
        RectTransform lineRectTransform = line.GetComponent<RectTransform>();

        // 선의 크기 및 위치, 회전을 설정합니다.
        lineRectTransform.sizeDelta = new Vector2(distance, lineRectTransform.sizeDelta.y);
        lineRectTransform.anchoredPosition = (direction * 0.5f);
        lineRectTransform.rotation = Quaternion.Euler(0, 0, angle);


    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Enter" + gameObject.name);
        Hover();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Exit" + gameObject.name);
        Descend();
        
    }
    public void Hover()
    {
        transform.localScale = new Vector3(2, 2, 2);
        breathControlFlag = true;
        breathCoroutine =StartCoroutine(StartBreath());
        for (int i = 0; i < AfterRoom.Length; i++)
        {
            if (AfterRoom[i] == null) break;
            AfterRoom[i].GetComponent<Room>().Hover();
        }
    }
    public void Descend()
    {
        breathControlFlag = false;
        transform.localScale = new Vector3(1, 1, 1);
        for (int i = 0; i < AfterRoom.Length; i++)
        {
            if (AfterRoom[i] == null) break;
            AfterRoom[i].GetComponent<Room>().Descend();
        }
    }
    IEnumerator StartBreath()
    {
        while (breathControlFlag)
        {
            yield return StartCoroutine(ScaleCoroutine(minScale, maxScale, duration));
            yield return StartCoroutine(ScaleCoroutine(maxScale, minScale, duration));
        }
    }
    IEnumerator ScaleCoroutine(Vector3 startScale, Vector3 endScale, float duration)
    {
        float startTime = Time.time;
        float endTime = startTime + duration;
        while(Time.time < endTime)
        {
            float progress = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, endScale, progress);
            yield return null;
        }
        transform.localScale = endScale;
    }
    IEnumerator BuildRoom()
    {
        yield return null;

        switch (_currentLevel)
        {
            case 1:
                _roomType = Types.RoomType.Battle;
                break;
            case 9:
                _roomType = Types.RoomType.Treasure;
                break;
            case 15:
                _roomType = Types.RoomType.Rest;
                break;
            case 16:
                _roomType = Types.RoomType.Boss;
                break;
            default:
                Types types = new Types();
                WeightedRandomPicker<Types.RoomType> picker = new WeightedRandomPicker<Types.RoomType>();
                foreach (KeyValuePair<Types.RoomType, int> item in types.EncounterPairs)
                {
                    picker.AddItem(item.Key, item.Value);
                }
                _roomType = picker.PickRandom(); //랜덤배치

                bool NotAvailableRoom = true;
                do
                {
                    NotAvailableRoom = false;
                    if ((_currentLevel <= 6 && _roomType == Types.RoomType.Rest) || (_currentLevel <= 6 && _roomType == Types.RoomType.Elite)) // 6층 이하에는 엘리트, 휴식 금지
                    {
                        _roomType = picker.PickRandom();
                        while (_roomType == Types.RoomType.Elite || _roomType == Types.RoomType.Rest)
                        {
                            _roomType = picker.PickRandom();
                        }
                        NotAvailableRoom = true;
                    }
                    switch (_roomType) // 연속된 엘리트, 상점, 휴식 금지
                    {
                        case Types.RoomType.Elite:
                        case Types.RoomType.Merchant:
                        case Types.RoomType.Rest:
                            for (int i = 0; i < BeforeRoom.Length; i++)
                            {
                                if (BeforeRoom[i] == null) break;
                                Room ancestorRoom = BeforeRoom[i].GetComponent<Room>();
                                if (ancestorRoom._roomType == _roomType)
                                {
                                    _roomType = picker.PickRandom();
                                    NotAvailableRoom = true;
                                    break;
                                }
                            }
                            break;
                        default: break;
                    }
                    /*if (_roomType == Types.RoomType.Battle || _roomType == Types.RoomType.Elite) // TODO 연속된 전투조우 방지
                    {
                        for (int i )
                    }*/

                    if (_currentLevel == 14 && _roomType == Types.RoomType.Rest) // 14층에는 휴식 x
                    {
                        _roomType = picker.PickRandom();
                        while (_roomType == Types.RoomType.Rest)
                        {
                            _roomType = picker.PickRandom();
                        }
                        NotAvailableRoom = true;
                    }
                    /* // 같은 부모에서 나온 자식 방들이 같은 룸타입임을 방지하는 코드지만 특성상 비활성화함
                    for(int i = 0; i < BeforeRoom.Length; i++)
                    {
                        if (BeforeRoom[i] == null) break;
                        for (int j = 0; j < AfterRoom.Length; j++)
                        {
                            if (BeforeRoom[i].GetComponent<Room>().AfterRoom[j] == null) break;
                            if (_roomType == BeforeRoom[i].GetComponent<Room>().AfterRoom[j].GetComponent<Room>()._roomType)
                            {
                                _roomType = picker.PickRandom();
                                while (_roomType == BeforeRoom[i].GetComponent<Room>().AfterRoom[j].GetComponent<Room>()._roomType)
                                {
                                    _roomType = picker.PickRandom();
                                }
                                NotAvailableRoom = true;
                            }
                        }
                    }*/
                } while (NotAvailableRoom);
                break;
        }
        Image imageComponent = gameObject.GetComponent<Image>();
        switch (_roomType)
        {
            case Types.RoomType.Boss:
                imageComponent.sprite = bossSprite; // 보스 방 이미지 할당
                imageComponent.color = Color.red;
                break;
            case Types.RoomType.Battle:
                imageComponent.sprite = battleSprite; // 전투 방 이미지 할당
                imageComponent.color = Color.red;
                break;
            case Types.RoomType.Elite:
                imageComponent.sprite = eliteSprite; // 엘리트 방 이미지 할당
                imageComponent.color = Color.red;
                break;
            case Types.RoomType.Event:
                imageComponent.sprite = eventSprite; // 이벤트 방 이미지 할당
                imageComponent.color = Color.blue;
                break;
            case Types.RoomType.Merchant:
                imageComponent.sprite = merchantSprite; // 상점 방 이미지 할당
                imageComponent.color = Color.green;
                break;
            case Types.RoomType.Treasure:
                imageComponent.sprite = treasureSprite; // 보물 방 이미지 할당
                imageComponent.color = Color.magenta;
                break;
            case Types.RoomType.Rest:
                imageComponent.sprite = restSprite; // 휴식 방 이미지 할당
                imageComponent.color = Color.yellow;
                break;
            default:
                break;
        }
    }

}
