using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movetoward : MonoBehaviour
{
    Vector3 dir;

    void Start()
    {
        dir = new Vector3(transform.position.x + 10, transform.position.y, transform.position.z);
    }
    
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, dir, Time.deltaTime);
    }
}
