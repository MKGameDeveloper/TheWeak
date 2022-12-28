using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour
{
    RectTransform tr;
    public Rect rc;

    public InventorySlot slot;

    //사용할 정보
    public itemInfo info;
    public Image item;
    public int itemcount = 0;
    public Text count;

    void Awake()
    {
        tr = gameObject.transform.GetChild(1).GetComponent<RectTransform>();

        rc.x = tr.position.x - tr.rect.width / 2;
        rc.y = tr.position.y + tr.rect.height / 2;

        rc.xMax = tr.rect.width;
        rc.yMax = tr.rect.height;
        rc.width = tr.rect.width;
        rc.height = tr.rect.height;

        item = gameObject.transform.GetChild(1).GetComponent<Image>();
        count = gameObject.transform.GetChild(2).GetComponent<Text>();
        
        item.sprite = null;
    }

    void Start()
    {
        
    }

    void QuickSlotUpdate()
    {
        if (itemcount == 0)
        {
            ResourceManager.instance.ImageAlphaChange(item, 0.5f);
            count.text = "<color=#ff0000>" + itemcount.ToString() + "</color>";

        }
        else if (itemcount != 0)
        {
            ResourceManager.instance.ImageAlphaChange(item, 1f);
            count.text = itemcount.ToString();

        }
    }
    
    void Update()
    {
        QuickSlotUpdate();

        if (item.sprite == null)
        {
            item.gameObject.SetActive(false);
            count.gameObject.SetActive(false);
        }
        else if(item.sprite != null)
        {
            item.gameObject.SetActive(true);
            count.gameObject.SetActive(true);
        }
    }
}
