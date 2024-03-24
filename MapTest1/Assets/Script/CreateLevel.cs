using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField]
    public GameObject _levelPanel; // 레벨 프리팹
    [SerializeField]
    public GameObject _room; // 방 프리팹
    [SerializeField]
    public int _maxLevel; // 최대 레벨 갯수 
    [SerializeField]
    public int _maxRoom; // 최대 방 갯수

    GameObject nowLevel;
    int count_room = 0;
    void Start()
    {
        // 레벨을 만들어줍니다.
        buildLevel(); 
        // 만들어진 다음 프레임부터 그리드의 position이 할당되기 때문에 코루틴에 그리드의 position을 사용하는
        // 기능들을 넣어줍니다.
        StartCoroutine(buildForGrid()); 
    }

    void buildLevel()
    {
        // 랜덤값, 시드는 현재시간입니다.
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        for (int currentLevel = 0; currentLevel < _maxLevel; currentLevel++)
        {
            // 레벨을 만듭니다
            nowLevel = Instantiate(_levelPanel, gameObject.transform) as GameObject;
            nowLevel.name = "LevelPanel" + currentLevel;

            int depth = 0;
            int currentRoomNumber = 0;
            while (depth < _maxRoom)
            {
                // 방을 만들고 변수들을 넘겨줍니다.
                GameObject room = Instantiate(_room, nowLevel.transform) as GameObject;
                room.name = "Room_" + count_room + "_" + currentLevel + currentRoomNumber + " (" + depth + ")";
                room.GetComponent<Room>().setEnvironment(currentRoomNumber, currentLevel, _maxRoom);
                count_room++;
                currentRoomNumber++;

                if (currentLevel == _maxLevel - 1) // 마지막 레벨은 방이 항상 1개
                {
                    depth += _maxRoom;
                }
                else if (currentLevel == 0) // 최초 레벨은 방이 항상 1개
                {
                    depth += _maxRoom;
                }
                else if (currentLevel == 1 || currentLevel == _maxLevel - 2) // 전개 레벨 및 휴식 레벨
                {
                    depth += (int)Mathf.Ceil(_maxRoom / 3);
                }
                else // 그 외 레벨
                {
                    depth += random.Next(1, 4); // 1~ 4추가
                }
            }
        }
    }
    void buildBridge()
    {
        // 레벨 0부터 마지막레벨 직전까지 반복 ( nextLevel도 가져오기에 _maxLevel -1 )
        for (int currentLevel = 0; currentLevel < _maxLevel - 1; currentLevel++)
        {
            GameObject nowLevel = gameObject.transform.GetChild(currentLevel).gameObject;
            GameObject nextLevel = gameObject.transform.GetChild(currentLevel + 1).gameObject;

            // 현재 레벨의 방 갯수만큼 반복 ( 각 방마다 다음 방이랑 연결 )
            for (int i = 0; i < nowLevel.transform.childCount; i++)
            {
                GameObject currentRoom = nowLevel.transform.GetChild(i).gameObject;
                GameObject[] nextRooms = getRandomChild(nowLevel, nextLevel);
                // 다음방과 논리적 연결
                for ( int j =0; j < nextRooms.Length; j++)
                {
                    currentRoom.GetComponent<Room>().setNextRoom(nextRooms[j]);
                }
                // 다음방에 향하는 길을 만듦
                currentRoom.GetComponent<Room>().startDraw();
            }
        }
    }
    GameObject[] getRandomChild(GameObject currentLevel, GameObject nextLevel)
    {
        // 다음 방을 담을 배열
        List<GameObject> objectiveRooms = new List<GameObject>();

        int nextChildCount = nextLevel.transform.childCount;
        int currentChildCount = currentLevel.transform.childCount;
        GameObject selectedRoom;
        List<int> indexes = new List<int>(); // 현재 방에서 길을 연결할 대상 방들을 나타내는 인덱스
        
        if (currentChildCount != 1)
        {
            if (currentChildCount >= nextChildCount) // currentChildCount == n, nextChildCount == 1, m (n > m)
            {
                // 첫 번째 조건: 무조건 하나는 선택
                // 빈 방 찾기 알고리즘
                FindUnconnectedRoom(nextLevel, nextChildCount, indexes);

                // 두 번째 조건: 자식 갯수가 여러개이면, 확률로 추가적으로 하나를 선택
                RandomizeConnectionRoom(nextLevel, objectiveRooms, nextChildCount, indexes);
            }
            else // currentChildCount < nextChildCount
            {
                // 첫 번째 조건: 무조건 하나는 선택
                // 빈 방 찾기 알고리즘
                float minimumRoomCount = MathF.Ceiling((float)nextChildCount / (float)currentChildCount);
                while (minimumRoomCount > indexes.Count)
                {
                    FindUnconnectedRoom(nextLevel, nextChildCount, indexes);
                    indexes = indexes.Distinct().ToList();
                }
                string forDebug = "";
                for(int i=0; i < indexes.Count; i++)
                {
                    forDebug += indexes[i];
                }
                // 두 번째 조건: 자식 갯수가 여러개이면, 확률로 추가적으로 하나를 선택
                RandomizeConnectionRoom(nextLevel, objectiveRooms, nextChildCount, indexes);

            }
        } 
        else // currentChildCount == 1 , nextChildCount == 1, m
        {
            for (int i = 0; i < nextChildCount; i++)
            {
                selectedRoom = nextLevel.transform.GetChild(i).gameObject;
                objectiveRooms.Add(selectedRoom);
            }
        }
        return objectiveRooms.ToArray();
    }

    private static void RandomizeConnectionRoom(GameObject nextLevel, List<GameObject> objectiveRooms, int nextChildCount, List<int> indexes, float randomRatio = 0.01f)
    {
        while (indexes.Count < nextChildCount && UnityEngine.Random.Range(0f, 1f) < randomRatio)
        {
            int additionalRandomIndex = UnityEngine.Random.Range(0, nextChildCount);
            // 이미 선택한 자식과 중복되지 않도록 한다
            while (indexes.Contains(additionalRandomIndex))
            {
                additionalRandomIndex = UnityEngine.Random.Range(0, nextChildCount);
            }
            indexes.Add(additionalRandomIndex);
        }

        indexes.Sort();
        for (int i = 0; i < indexes.Count; i++)
        {
            objectiveRooms.Add(nextLevel.transform.GetChild(indexes[i]).gameObject);
        }
    }

    private static void FindUnconnectedRoom(GameObject nextLevel, int nextChildCount, List<int> indexes)
    {
        int randomIndex;
        List<int> checkedIndexes = new List<int>();
        while (true)
        {
            randomIndex = UnityEngine.Random.Range(0, nextChildCount);
            if (checkedIndexes.Contains(randomIndex)) // true: randomIndex의 방은 차 있음, false : 확인안함
            {
                continue;
            }
            // 방 차있는지 확인하기
            if (nextLevel.transform.GetChild(randomIndex).gameObject.GetComponent<Room>().BeforeRoom[0] == null && !indexes.Contains(randomIndex)) // 목표방에 길이 없으면 탈출
            {
                indexes.Add(randomIndex); // 길을 연결하라고 인덱스 추가
                break;
            }
            // 점유된 방이므로 빈방이 아니라고 표시
            checkedIndexes.Add(randomIndex);
            if (checkedIndexes.Count == nextChildCount) // 모든 방이 점유 되었다면
            {
                indexes.Add(randomIndex); // 걍 아무거나 골라서 가지고 나감
                break;
            }
        }
    }


    // 프레임 이후 작업
    IEnumerator buildForGrid()
    {
        yield return new WaitForSeconds(1f);
        buildBridge();
        gameObject.GetComponent<RectTransform>().localScale = new Vector3(4f, 2f, 1f);
    }

    public void CustomDestroy()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            
            Destroy(transform.GetChild(i).gameObject);
        }
        _maxLevel = 17;
        _maxRoom = 6;
        nowLevel = null;
        count_room = 0;
        Start();
    }

}
