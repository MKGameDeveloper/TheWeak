using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Vector3 Teleport;

    public string NoticeArea;

    public float distance;

    void Start()
    {
        
    }

    void Update()
    {
        distance = Vector3.Distance(transform.position, Character.instance.transform.position);
        
    }
}
