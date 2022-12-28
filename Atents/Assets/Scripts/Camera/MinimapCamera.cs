using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    Vector3 CameraPos;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        CameraPos = new Vector3(Character.instance.transform.position.x, 50f, Character.instance.transform.position.z);
        transform.position = CameraPos;
    }
}
