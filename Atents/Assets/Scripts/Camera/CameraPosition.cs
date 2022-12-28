using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform Player;

    public GameObject Boss;

    public GameObject eventcamera;

    public GameObject ui;

    public float Camera_X = 0f;
    public float Camera_Y = 7f;
    public float Camera_Z = -5.0f;
    Vector3 Camera;

    void Start()
    {
         
    }

    void CameraUpDown()
    {
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if(Camera_Y < 12f)
            {
                Camera_Y += 1f;
                Camera_Z -= 1f;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (Camera_Y > 7f)
            {
                Camera_Y -= 1f;
                Camera_Z += 1f;
            }
        }
    }

    private void LateUpdate()
    {
        CameraUpDown();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Boss.gameObject.SetActive(true);
            ui.gameObject.SetActive(false);
            eventcamera.SetActive(true);
            gameObject.SetActive(false);
        }

        Camera.x = Player.position.x + Camera_X;
        Camera.y = Player.position.y + Camera_Y;
        Camera.z = Player.position.z + Camera_Z;

        transform.position = Camera;

        
    }
}
