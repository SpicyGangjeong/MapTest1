using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMap : MonoBehaviour
{
    [SerializeField]
    public GameObject MainPanel;
    public void Start()
    {
        if ( gameObject.transform.Find("MainPanel") == null)
        {
            GameObject mainPanel = Instantiate(MainPanel, gameObject.transform) as GameObject;
            mainPanel.name = "MainPanel";
            

        }
    }
}
