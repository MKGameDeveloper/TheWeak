using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eventCamera : MonoBehaviour
{
    public Transform bossPos;
    public GameObject ui;

    public static eventCamera instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("EventCamera"), 
            "easeType", "easeOutCubic", 
            "time", 20f, 
            "looktarget",bossPos,
            "oncomplete","EndEvent",
            "movetopath",true));

        Boss.instance.Event = 1;
    }

    void Start()
    {
        
    }

    void EndEvent()
    {
        ui.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }
}
