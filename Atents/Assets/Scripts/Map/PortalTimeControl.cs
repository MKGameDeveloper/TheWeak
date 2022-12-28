using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortalTimeControl : MonoBehaviour
{
    public static PortalTimeControl instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public float MaxCount;
    public float time;

    public Image Gage;
    public Text Count;

    void Start()
    {
        
    }

    public void GageControl()
    {
        Gage.fillAmount = time / MaxCount;
        Count.text = ((int)time).ToString()+"s";
    }

    void Update()
    {
        GageControl();
    }
}
