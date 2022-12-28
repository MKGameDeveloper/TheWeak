using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUse : MonoBehaviour
{
    public GameObject heal;
    public static ItemUse instance = null;

    /*구현할 내용
     * 
     * 아이템 착용시 아이템 정보를 GameObject에 저장
     * 저장후 아이템에 저장되있는 정보인 ItemKinds로 장비 착용이나 퀵슬롯 등록 구분
     * 
     * ItemNumber 를 통해 Plus 스탯을 추가하거나 소비아이템을 사용
    */

    //아이템
    //ItemKinds : 1=소모품, 2=장비, 3=기타 아이템, 4=퀘스트 아이템
    //ItemNumber : 1=공격력, 2=방어력, 3=주문력, 4=민첩, 5=체력, 6=마나, 7=스테미나, 10=스크롤    

    //장비 아이템
    public EquipmentSlot objhead;
    public EquipmentSlot objarmor;
    public EquipmentSlot objshoes;
    public EquipmentSlot objweapon;
    public EquipmentSlot objsubweapon;

    //소비아이템
    public QuickSlot qslot1;
    public QuickSlot qslot2;
    public QuickSlot qslot3;

    void Awake()
    {
        if(instance = null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }

    void KeyDown1()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {

            if (qslot1.itemcount > 0)
            {
                if (Character.instance.hp != Status.instance.curhp)
                {
                    if (qslot1.info.ItemNumber == 5)
                    {
                        heal.gameObject.SetActive(false);
                        heal.gameObject.SetActive(true);
                        Character.instance.hp += qslot1.info.ItemInfo;
                        QuickSlotList.instance.Quickslotlist[0].slot.ItemCount -= 1;
                    }
                }
                //if (Character.instance.mp != Status.instance.curmp)
                //{
                //    if (qslot1.info.ItemNumber == 6)
                //    {
                //        //Character.instance.mp += qslot1.ItemInfo;
                //    }
                //}
            }
         
        }
        
    }
    void KeyDown2()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (qslot2.itemcount > 0)
            {
                if (Character.instance.hp != Status.instance.curhp)
                {
                    if (qslot2.info.ItemNumber == 5)
                    {
                        heal.gameObject.SetActive(false);
                        heal.gameObject.SetActive(true);
                        Character.instance.hp += qslot2.info.ItemInfo;
                        QuickSlotList.instance.Quickslotlist[1].slot.ItemCount -= 1;
                    }
                }
                //if (Character.instance.mp != Status.instance.curmp)
                //{
                //    if (qslot2.info.ItemNumber == 6)
                //    {
                //        //Character.instance.mp += qslot1.ItemInfo;
                //    }
                //}
            }
        }
    }
    void KeyDown3()
    {

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (qslot3.itemcount > 0)
            {
                if (Character.instance.hp != Status.instance.curhp)
                {
                    if (qslot3.info.ItemNumber == 5)
                    {
                        heal.gameObject.SetActive(false);
                        heal.gameObject.SetActive(true);
                        Character.instance.hp += qslot3.info.ItemInfo;
                        QuickSlotList.instance.Quickslotlist[2].slot.ItemCount -= 1;
                    }
                }
                //if (Character.instance.mp != Status.instance.curmp)
                //{
                //    if (qslot3.info.ItemNumber == 6)
                //    {
                //        //Character.instance.mp += qslot1.ItemInfo;
                //    }
                //}
            }
        }
    }

    void Update()
    {
        KeyDown1();
        KeyDown2();
        KeyDown3();
    }
}
