using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using System.IO;
using System.Linq;

public struct Item_s
{
    //아이템
    //ItemKinds : 1=소모품, 2=장비, 3=기타 아이템, 4=퀘스트 아이템
    //ItemNumber : 1=공격력, 2=방어력, 3=주문력, 4=민첩, 5=체력, 6=마나, 7=스테미나

    public int ItemKinds;
    public int ItemNumber;
    public string ItemIndex;
    public string ItemName;
    public int ItemInfo;
    public string ItemExp;
    public int Item_B_Cost;
    public int Item_S_Cost;
    public int index;

}

public class ItemManager : MonoBehaviour
{

    public static ItemManager instance = null;

    
    //몬스터가 드롭할 아이템 또는 상점에서 사용할 아이템 정보를 저장할 구조체
   
    //아이템의 정보를 담은 구조체를 저장할 리스트
    public List<Item_s> ItemInfolist = new List<Item_s>();

    //드랍된 아이템 저장할 리스트
    List<ItemData> DropItemlist = new List<ItemData>();

    ItemData findobj;

    //아이템들을 저장할 배열
    GameObject[] Items;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        Items = Resources.LoadAll<GameObject>("ItemObject/");

    }


    void Start()
    {
        ReadItem("ItemData.csv");
    }


    public void ReadItem(string FileName)
    {
        string strPath = Application.dataPath + "/Resources/" + FileName;
        FileStream monster1 = new FileStream(strPath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(monster1, System.Text.Encoding.UTF8);

        string line = string.Empty;

        //헤드라인을 미리 읽어놓음.
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {

            string[] strData = line.Split(',');

            Item_s Info = new Item_s();
            Info.ItemKinds = int.Parse(strData[0]);
            Info.ItemNumber = int.Parse(strData[1]);
            Info.ItemIndex = strData[2];
            Info.ItemName = strData[3];
            Info.ItemInfo = int.Parse(strData[4]);
            Info.ItemExp = strData[5];
            Info.Item_B_Cost = int.Parse(strData[6]);
            Info.Item_S_Cost = int.Parse(strData[7]);
            Info.index = int.Parse(strData[9]);


            ItemInfolist.Add(Info);


        }
        reader.Close();

    }

    public void MoveDrop(Vector3 Cur, Vector3 move)
    {

    }

    public void MakeItem(string dropitem, Vector3 Pos)
    {
        //Itemx : 아이템 오브젝트 정보를 저장한 배열
        //ItemInfolist : 아이템 csv정보를 저장한 리스트
        //DropItemlist : 아이템 오브젝트에 csv 정보를 저장하고 ItemData 클래스를 붙여놓은 오브젝트를 저장하는 리스트

        findobj = DropItemlist.Find(o => (o.info.ItemIndex == dropitem) && o.gameObject.activeSelf == false);
        

        if (findobj != null)
        {
            findobj.transform.position = Pos;
            findobj.gameObject.SetActive(true);
            findobj = null;

        }
        else if (findobj == null)
        {

            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].name == dropitem)
                {


                    for (int j = 0; j < ItemInfolist.Count; j++)
                    {
                        if (Items[i].name == ItemInfolist[j].ItemIndex)
                        {
                            GameObject itemobj = Instantiate<GameObject>(Items[i]);
                            itemobj.transform.position = Pos;
                            ItemData InItem = itemobj.AddComponent<ItemData>();
                            InItem.info.ItemKinds = ItemInfolist[j].ItemKinds;
                            InItem.info.ItemNumber = ItemInfolist[j].ItemNumber;
                            InItem.info.ItemIndex = ItemInfolist[j].ItemIndex;
                            InItem.info.ItemName = ItemInfolist[j].ItemName;
                            InItem.info.ItemInfo = ItemInfolist[j].ItemInfo;
                            InItem.info.ItemExp = ItemInfolist[j].ItemExp;
                            InItem.info.Item_B_Cost = ItemInfolist[j].Item_B_Cost;
                            InItem.info.Item_S_Cost = ItemInfolist[j].Item_S_Cost;
                            InItem.info.index = ItemInfolist[j].index;
                            InItem.name = "Item_" + j;
                            


                            DropItemlist.Add(InItem);
                        }
                    }

                }
            }
        }
        
    }

   

    public int RandomInteger(int MaxNum)
    {
        int num = Random.Range(0, MaxNum);

        return num;
    }

    public void DropItem(string drop1, string drop2, string drop3 , string drop4, string drop5, Vector3 dropPos)
    {
        int num = ItemManager.instance.RandomInteger(100);

        if (num < 30)//30퍼센트
        {
            
        }
        else if (30 <= num && num < 60)//30퍼센트
        {
            
        }
        else if (60 <= num && num < 80)//20퍼센트
        {
            
        }
        else if (80 <= num && num < 90)//10퍼센트
        {
            MakeItem(drop1, dropPos);
            //꽝
        }
        else if (90 <= num && num < 95)//5퍼센트
        {
            MakeItem(drop2, dropPos);
            //꽝
        }
        else if (95 <= num && num < 97)//2퍼센트
        {
            
            //꽝
        }
        else if (97 <= num && num < 98)//1퍼센트
        {
            MakeItem(drop3, dropPos);
            //꽝
        }
        else if (98 <= num && num < 99)//1퍼센트
        {
            MakeItem(drop4, dropPos);
            //꽝
        }
        else if (99 <= num && num < 100)//1퍼센트
        {
            MakeItem(drop5, dropPos);
            //꽝
        }

    }

    void Update()
    {
        
    }
}
