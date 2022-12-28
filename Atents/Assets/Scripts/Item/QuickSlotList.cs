using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotList : MonoBehaviour
{
    public static QuickSlotList instance = null;

    public List<QuickSlot> Quickslotlist = new List<QuickSlot>();

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        
    }
    
    void QuickSlotManager()
    {
        for (int i = 0; i < Quickslotlist.Count; i++) 
        {
            InventorySlot quick = Inventory.instance.slotlist.Find(o => (o.OnQuick == i));

            if (quick != null)
            {
                Quickslotlist[i].slot = quick;
                Quickslotlist[i].itemcount = quick.ItemCount;
                Quickslotlist[i].info = quick.ItemInfo;
                
            }
            if(quick == null)
            {
                Quickslotlist[i].item.sprite = null;
            }
        }
    }

    void Update()
    {
        QuickSlotManager();
    }
}
