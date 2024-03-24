using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    public GameObject Content;
    public GameObject scrollViewViewPort;
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
        if (scrollViewViewPort.transform.Find("Content") != null)
        {
            instanceMainPanel.transform.GetComponent<CreateLevel>().CustomDestroy();
            return;
        }
        else Debug.Log("MainPanel is not exist");
    }
    void CreatePanel()
    {
        if (scrollViewViewPort.transform.Find("Content") == null)
        {
            instanceMainPanel = Instantiate(Content, scrollViewViewPort.transform) as GameObject;
            instanceMainPanel.name = "Content";
            return;
        }
        else
        {
            Debug.Log("MainPanel is Already exist");
            instanceMainPanel = scrollViewViewPort.transform.Find("Content").gameObject;
        }
    }
}
