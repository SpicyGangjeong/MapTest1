using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    public GameObject MainPanel;
    public GameObject instanceMainPanel;
    public void Start()
    {
        CreatePanel();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DestroyPanel();
            CreatePanel();
        }
    }
    void DestroyPanel()
    {
        if (gameObject.transform.Find("MainPanel") != null)
        {
            Destroy(instanceMainPanel);
            return;
        }
        Debug.Log("MainPanel is not exist");
    }
    void CreatePanel()
    {
        if (gameObject.transform.Find("MainPanel") == null)
        {
            instanceMainPanel = Instantiate(MainPanel, gameObject.transform) as GameObject;
            instanceMainPanel.name = "MainPanel";
            return;
        }
        Debug.Log("MainPanel is Already exist");
    }
}
