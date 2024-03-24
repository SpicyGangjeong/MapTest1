using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField]
    public GameObject _levelPanel; // ���� ������
    [SerializeField]
    public GameObject _room; // �� ������
    [SerializeField]
    public int _maxLevel; // �ִ� ���� ���� 
    [SerializeField]
    public int _maxRoom; // �ִ� �� ����

    GameObject nowLevel;
    int count_room = 0;
    void Start()
    {
        // ������ ������ݴϴ�.
        buildLevel(); 
        // ������� ���� �����Ӻ��� �׸����� position�� �Ҵ�Ǳ� ������ �ڷ�ƾ�� �׸����� position�� ����ϴ�
        // ��ɵ��� �־��ݴϴ�.
        StartCoroutine(buildForGrid()); 
    }

    void buildLevel()
    {
        // ������, �õ�� ����ð��Դϴ�.
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        for (int currentLevel = 0; currentLevel < _maxLevel; currentLevel++)
        {
            // ������ ����ϴ�
            nowLevel = Instantiate(_levelPanel, gameObject.transform) as GameObject;
            nowLevel.name = "LevelPanel" + currentLevel;

            int depth = 0;
            int currentRoomNumber = 0;
            while (depth < _maxRoom)
            {
                // ���� ����� �������� �Ѱ��ݴϴ�.
                GameObject room = Instantiate(_room, nowLevel.transform) as GameObject;
                room.name = "Room_" + count_room + "_" + currentLevel + currentRoomNumber + " (" + depth + ")";
                room.GetComponent<Room>().setEnvironment(currentRoomNumber, currentLevel, _maxRoom);
                count_room++;
                currentRoomNumber++;

                if (currentLevel == _maxLevel - 1) // ������ ������ ���� �׻� 1��
                {
                    depth += _maxRoom;
                }
                else if (currentLevel == 0) // ���� ������ ���� �׻� 1��
                {
                    depth += _maxRoom;
                }
                else if (currentLevel == 1 || currentLevel == _maxLevel - 2) // ���� ���� �� �޽� ����
                {
                    depth += (int)Mathf.Ceil(_maxRoom / 3);
                }
                else // �� �� ����
                {
                    depth += random.Next(1, 4); // 1~ 4�߰�
                }
            }
        }
    }
    void buildBridge()
    {
        // ���� 0���� ���������� �������� �ݺ� ( nextLevel�� �������⿡ _maxLevel -1 )
        for (int currentLevel = 0; currentLevel < _maxLevel - 1; currentLevel++)
        {
            GameObject nowLevel = gameObject.transform.GetChild(currentLevel).gameObject;
            GameObject nextLevel = gameObject.transform.GetChild(currentLevel + 1).gameObject;

            // ���� ������ �� ������ŭ �ݺ� ( �� �渶�� ���� ���̶� ���� )
            for (int i = 0; i < nowLevel.transform.childCount; i++)
            {
                GameObject currentRoom = nowLevel.transform.GetChild(i).gameObject;
                GameObject[] nextRooms = getRandomChild(nowLevel, nextLevel);
                // ������� ���� ����
                for ( int j =0; j < nextRooms.Length; j++)
                {
                    currentRoom.GetComponent<Room>().setNextRoom(nextRooms[j]);
                }
                // �����濡 ���ϴ� ���� ����
                currentRoom.GetComponent<Room>().startDraw();
            }
        }
    }
    GameObject[] getRandomChild(GameObject currentLevel, GameObject nextLevel)
    {
        // ���� ���� ���� �迭
        List<GameObject> objectiveRooms = new List<GameObject>();

        int nextChildCount = nextLevel.transform.childCount;
        int currentChildCount = currentLevel.transform.childCount;
        GameObject selectedRoom;
        List<int> indexes = new List<int>(); // ���� �濡�� ���� ������ ��� ����� ��Ÿ���� �ε���
        
        if (currentChildCount != 1)
        {
            if (currentChildCount >= nextChildCount) // currentChildCount == n, nextChildCount == 1, m (n > m)
            {
                // ù ��° ����: ������ �ϳ��� ����
                // �� �� ã�� �˰���
                FindUnconnectedRoom(nextLevel, nextChildCount, indexes);

                // �� ��° ����: �ڽ� ������ �������̸�, Ȯ���� �߰������� �ϳ��� ����
                RandomizeConnectionRoom(nextLevel, objectiveRooms, nextChildCount, indexes);
            }
            else // currentChildCount < nextChildCount
            {
                // ù ��° ����: ������ �ϳ��� ����
                // �� �� ã�� �˰���
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
                // �� ��° ����: �ڽ� ������ �������̸�, Ȯ���� �߰������� �ϳ��� ����
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
            // �̹� ������ �ڽİ� �ߺ����� �ʵ��� �Ѵ�
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
            if (checkedIndexes.Contains(randomIndex)) // true: randomIndex�� ���� �� ����, false : Ȯ�ξ���
            {
                continue;
            }
            // �� ���ִ��� Ȯ���ϱ�
            if (nextLevel.transform.GetChild(randomIndex).gameObject.GetComponent<Room>().BeforeRoom[0] == null && !indexes.Contains(randomIndex)) // ��ǥ�濡 ���� ������ Ż��
            {
                indexes.Add(randomIndex); // ���� �����϶�� �ε��� �߰�
                break;
            }
            // ������ ���̹Ƿ� ����� �ƴ϶�� ǥ��
            checkedIndexes.Add(randomIndex);
            if (checkedIndexes.Count == nextChildCount) // ��� ���� ���� �Ǿ��ٸ�
            {
                indexes.Add(randomIndex); // �� �ƹ��ų� ��� ������ ����
                break;
            }
        }
    }


    // ������ ���� �۾�
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
