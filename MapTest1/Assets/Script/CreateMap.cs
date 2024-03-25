using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    public GameObject scrollView;
    public GameObject instanceScrollView;
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
        if (transform.Find("Scroll View") != null)
        {
            Destroy(instanceScrollView);
            return;
        }
        else Debug.Log("Scroll View is not exist");
    }
    void CreatePanel()
    {
        if (transform.Find("Scroll View") == null)
        {
            instanceScrollView = Instantiate(scrollView, transform) as GameObject;
            instanceScrollView.name = "Scroll View";
            return;
        }
        else
        {
            Debug.Log("Scroll View is Already exist");
            instanceScrollView = transform.Find("Scroll View").gameObject;
        }
    }
}
