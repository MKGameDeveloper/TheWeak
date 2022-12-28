using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{

    public RectTransform rcContent;
    public ScrollRect scRect;

    public Image chaMini;
    RectTransform rtChaMini;

    public RectTransform rtItem;

    public RectTransform RTCHAMINI
    {
        get
        {
            if (rtChaMini == null)
            {
                rtChaMini = chaMini.GetComponent<RectTransform>();

            }
            return rtChaMini;
        }
    }


    //시작점
    Vector2 normalPos = new Vector2(0.0f, 0.5f);


    void Start()
    {
        scRect.normalizedPosition = normalPos;
        //rtItem.localPosition = new Vector2(0f,0f);

        //게임공간상의 좌표
        //실제 맵과 미니맵의 비율 계산
        //float xRatio = rcContent.sizeDelta.x / Character.instance.vMapSize.x;
        //맵의 아이템을 -7.39이 아닌 0을 기준으로 한 좌표
        //Vector3 tmp = ItemManager.instance.ITEM + new Vector3(7.5f, 0, 0);
        //맵의 크기에서의 좌표를 비율로 계산
        //float xRatio = tmp.x / Character.instance.vMapSize.x;

        //rtItem.localPosition = new Vector3(rcContent.sizeDelta.x * xRatio, 0, 0);

    }


    void Update()
    {
        if (Character.instance != null && Character.instance.h == 0f)
            return;

        //비율계산
        float fDeltax = Character.instance.transform.position.x - Character.instance.vStart.x;
        //이동한 거리에 대한 비율
        float ratiox = fDeltax / Character.instance.vMapSize.x;
        Debug.Log(ratiox);

        //미니맵에게 이동한 거리를 가르쳐 준다.
        normalPos.x = ratiox;
        scRect.normalizedPosition = normalPos;


    }
}
