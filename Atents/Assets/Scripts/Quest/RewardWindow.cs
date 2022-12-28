using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardWindow : MonoBehaviour
{
    public static RewardWindow instance = null;

    public Text QName;
    public Text Qcontents;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        QName = gameObject.transform.GetChild(1).GetComponent<Text>();
        Qcontents = gameObject.transform.GetChild(2).GetComponent<Text>();
    }

    void Start()
    {
    }
    

    void Update()
    {
       
    }
}
