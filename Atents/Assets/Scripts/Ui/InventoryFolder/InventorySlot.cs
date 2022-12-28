using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
 //   InventorySlot slot;

    RectTransform tr;
    public Rect rc;

    public Text Count;
    public int ItemCount = 0;
    public int OnQuick = -1;

    public Image Item;

    //슬롯은 아이템 정보를 저장하고 있다
    public itemInfo ItemInfo;

    private void Awake()
    {
        

        //slot 오브젝트의 자식 오브젝트인 이미지와 카운트를 저장
        Item = gameObject.transform.GetChild(1).GetComponent<Image>();
        Count = gameObject.transform.GetChild(2).GetComponent<Text>();

        tr = gameObject.transform.GetChild(1).GetComponent<RectTransform>();
        //rc의 정보 : 아이템 rect의 좌측 상단점이 시작점임,(rc는 아이템이미지 rect의 정보)
        rc.x = tr.position.x - tr.rect.width / 2;
        rc.y = tr.position.y + tr.rect.height / 2;

        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;

        Item.sprite = null;
        OnQuick = -1;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        if (Item != null)
        {
            Debug.Log(ItemInfo.index);
        }
        Count.text = ItemCount.ToString();

        if (Input.GetKeyDown(KeyCode.H))
        {
            ItemCount -= 1;
        }

        if(ItemCount == 0)
        {
            Item.sprite = null;
        }

        //아이템 이미지가 비어있다면 카운트와 이미지를 비활성화
        if (Item.sprite == null)
        {
            Item.gameObject.SetActive(false);
            Count.gameObject.SetActive(false);
        }
        else if (Item.sprite != null) 
        {
            Item.gameObject.SetActive(true);
            Count.gameObject.SetActive(true);
        }
    }
}
