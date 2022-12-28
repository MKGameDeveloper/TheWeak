using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct itemInfo
{
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

delegate void Do();

public class ItemData : MonoBehaviour
{

    public itemInfo info;

    Vector3 movePos;

    float ItemRomoveCount;

    Do _Move = null;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        ItemRomoveCount = 0f;
        movePos.x = (Random.Range(transform.position.x - 1f, transform.position.x + 1f));
        movePos.z = (Random.Range(transform.position.z - 1f, transform.position.z + 1f));
        movePos.y = 0.5f;
        

        _Move = moveItem;
    }
    

    void moveItem()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePos, 5f * Time.deltaTime);

        if(transform.position == movePos)
        {
            _Move = null;
        }
    }

    void GetItem()
    {
        if (Vector3.Distance(Character.instance.transform.position, transform.position) < 1f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Character.instance.state = Character.STATE.ATTACK;
                Character.instance.ani.Play("Pick", 0, 0.1f);

                Inventory.instance.PickItem(info);
                ItemNotice.instance.itemnotice(info);
                gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        ItemRomoveCount += Time.deltaTime;

        if(ItemRomoveCount >= 25f)
        {
            gameObject.SetActive(false);
        }

        if(_Move != null)
        {
            _Move();
        }

        GetItem();
    }
}
