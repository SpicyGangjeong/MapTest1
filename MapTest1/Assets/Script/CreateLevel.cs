using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateLevel : MonoBehaviour
{
    [SerializeField]
    public GameObject _levelPanel;
    [SerializeField]
    public GameObject _room;
    [SerializeField]
    public int _maxLevel;
    [SerializeField]
    public int _maxDepth;
    void Start()
    {
        buildLevel();
        buildBridge();
    }

    void buildLevel()
    {
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        GameObject nowLevel;
        int count_room = 0;
        for(int level = 0; level < _maxLevel ; level++)
        {
            nowLevel = Instantiate(_levelPanel, gameObject.transform) as GameObject;
            nowLevel.name = "LevelPanel" + level;

            int depth = 0;
            while(depth < _maxDepth)
            {
                GameObject room = Instantiate(_room, nowLevel.transform) as GameObject;
                room.name = "Room_" + count_room + "_" + level + depth;
                count_room++;

                if (level == _maxLevel - 1) // 마지막 레벨
                {
                    depth += _maxDepth;
                }
                else if (level == 0) // 최초 레벨
                {
                    depth += _maxDepth;
                }
                else if (level == 1) // 전개 레벨
                {
                    depth += 2;
                }
                else // 그 외 레벨
                {
                    depth += random.Next(1, 6); // 1~ 4추가
                }
            }
        }

    }
    void buildBridge()
    {
        for (int level = 0; level < _maxLevel - 1; level++)
        {
            GameObject currentLevel = gameObject.transform.GetChild(level).gameObject;
            GameObject nextLevel = gameObject.transform.GetChild(level + 1).gameObject;

            for (int i = 0; i < currentLevel.transform.childCount; i++)
            {
                GameObject currentRoom = currentLevel.transform.GetChild(i).gameObject;

            }
        }
    }
}
