using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ItemNotice : MonoBehaviour
{
    public static ItemNotice instance = null;

    //습득 아이템 알림창을 관리할 리스트
    List<GetItem> noticelist = new List<GetItem>();

    public ScrollRect sr;
    public RectTransform tr;

    GameObject Itemobj;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        Itemobj = Resources.Load("GetItem") as GameObject;
    }

    void Start()
    {
        
    }

    public void itemnotice(itemInfo info)
    {
        GetItem findobj = noticelist.Find(o => (o.gameObject.activeSelf == false));

        if (findobj != null)
        {
            findobj.gameObject.SetActive(true);

            GameObject obj = findobj.transform.GetChild(1).gameObject;
            Image itemSprite = obj.GetComponent<Image>();
            obj = findobj.transform.GetChild(2).gameObject;
            Text itemName = obj.GetComponent<Text>();

            itemSprite.sprite = ResourceManager.instance.GetIcon(info.ItemIndex);
            itemName.text = info.ItemName;


            findobj.transform.SetAsFirstSibling();


        }
        if (findobj == null)
        {
            GameObject instance = GameObject.Instantiate(Itemobj) as GameObject;

            GameObject obj = instance.transform.GetChild(1).gameObject;
            Image itemSprite = obj.GetComponent<Image>();
            obj = instance.transform.GetChild(2).gameObject;
            Text itemName = obj.GetComponent<Text>();

            itemSprite.sprite = ResourceManager.instance.GetIcon(info.ItemIndex);
            itemName.text = info.ItemName;

            instance.transform.SetParent(sr.content.transform);
            instance.transform.SetAsFirstSibling();

            GetItem getitem = instance.AddComponent<GetItem>();

            noticelist.Add(getitem);
        }

    }
    void Update()
    {

    }
}
