using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public GameObject Ui;

    //restart 창을 가지고 있는 오브젝트
    public Restart Die;
    //restart를 실행하는 버튼
    public Button restart;

    //미니맵 껐다켰다를 해줄 오브젝트와 버튼
    public GameObject minimap;
    public RectTransform minimap_BT;

    public Inventory inven;

    public PortalTimeControl portalgage;

    bool resurrection = false;
    bool appear = false;

    //restart 창 자식오브젝트를 저장할 변수들
    Image back;
    Text gameover;
    Image town;
    Text town_t;
    Image menu;
    Text menu_t;

    bool resurPassive = false;
    float resurrectionCount = 0;

    //퀘스트창을 관리해줄 오브젝트
    public GameObject QuestWindow;

    //포탈 연결창을 관리해줄 오브젝트
    public GameObject PortalChangeWindow;
    

    void Awake()
    {
        //Restart 창 알파블렌딩을 위한 자식오브젝트 호출
        back = Die.GetComponent<Image>();
        gameover = Die.transform.GetChild(0).GetComponent<Text>();
        town = Die.transform.GetChild(1).GetComponent<Image>();
        town_t = Die.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        menu = Die.transform.GetChild(2).GetComponent<Image>();
        menu_t = Die.transform.GetChild(2).GetChild(0).GetComponent<Text>();

        //
    }

    void Start()
    {
        if (inven.gameObject.activeSelf == false)
        {
            inven.gameObject.SetActive(true);
        }
        if (portalgage.gameObject.activeSelf == false)
        {
            portalgage.gameObject.SetActive(true);
        }
    }

    //포탈관리창을 꺼주는 버튼
    public void OffPortalChange()
    {
        PortalChangeWindow.gameObject.SetActive(false);
    }

    //인벤토리 꺼주는 버튼
    public void OninventoryOut()
    {
        inven.gameObject.SetActive(false);
    }

    //Tab 누르면 인벤토리를 호출하는 함수
    public void InventoryKeycode()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inven.gameObject.activeSelf == false)
            {
                inven.gameObject.SetActive(true);
            }
            else
            {
                inven.gameObject.SetActive(false);
            }
        }
    }

    //화면상의 인벤토리를 불러올수있는 버튼
    public void OninventoryButton()
    {
        if (inven.gameObject.activeSelf == false)
        {
            inven.gameObject.SetActive(true);
        }
        else
        {
            inven.gameObject.SetActive(false);
        }
    }

    //미니맵 OnOff 버튼
    public void OnMinimapClick()
    {
        if (minimap.gameObject.activeSelf == false)
        {

            
            minimap.gameObject.SetActive(true);
            Quaternion rotation = Quaternion.Euler(0f, 0f, 0f);
            minimap_BT.rotation = rotation;
            

        }
        else
        {
            Quaternion rotation = Quaternion.Euler(0f, 0f, 180f);
            minimap_BT.rotation = rotation;
            minimap.gameObject.SetActive(false);
        }
    }

    //마을에서 부활 버튼
    public void OnRestartClick()
    {
        Character.instance.transform.position = Character.instance.savePos;
        Character.instance.hp = (int)(Status.instance.curhp * 0.7f);
        Character.instance.stamina.fillAmount = 1f;
        Character.instance.Die = false;
        Character.instance.state = Character.STATE.IDLE;
        Die.gameObject.SetActive(false);
        resurrection = true;
        appear = false;


    }

    //부활버튼을 눌렀을시 게임오버 화면이 사라짐
    public void RestartActive()
    {
        if (Character.instance.Die == true)
        {
            Die.gameObject.SetActive(true);

            if (appear == false)
            {
                ResourceManager.instance.ImageAlphaChange(back, 0f);
                ResourceManager.instance.TextAlphaChange(gameover, 0f);
                ResourceManager.instance.ImageAlphaChange(town, 0f);
                ResourceManager.instance.TextAlphaChange(town_t, 0f);
                ResourceManager.instance.ImageAlphaChange(menu, 0f);
                ResourceManager.instance.TextAlphaChange(menu_t, 0f);

                appear = true;
            }
        }
    }

    //캐릭터가 사망했을시 호출되는 화면
    void reAppear()
    {
        if (appear == true && back.color.a <= 0.88)
        {
            ResourceManager.instance.ImageAlphaBlending_Plus(back);
            ResourceManager.instance.TextAlphaBlending_Plus(gameover);
            ResourceManager.instance.ImageAlphaBlending_Plus(town);
            ResourceManager.instance.TextAlphaBlending_Plus(town_t);
            ResourceManager.instance.ImageAlphaBlending_Plus(menu);
            ResourceManager.instance.TextAlphaBlending_Plus(menu_t);
        }
    }

    //ESC를 누르면 UI가 꺼짐
    public void canvascontrol()
    {
        if (Ui.gameObject.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Ui.gameObject.SetActive(false);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Ui.gameObject.SetActive(true);
            }
        }
    }

    //V를 누르면 퀘스트창을 OnOff 할 수 있음
    public void QuestWindowOnOff()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (QuestWindow.gameObject.activeSelf == false)
            {
                QuestWindow.gameObject.SetActive(true);
            }
            else if(QuestWindow.gameObject.activeSelf == true)
            {
                QuestWindow.gameObject.SetActive(false);
            }
        }
    }

    //클릭시 퀘스트창 Off
    public void QuestWindowXbutton()
    {
        QuestWindow.gameObject.SetActive(false);
    }

    //캐릭터 부활시 애니메이션 호출
    public void Onresurrection()
    {
        if (resurrection == false)
            return;

        Character.instance.state = Character.STATE.ATTACK;
        Character.instance.ani.SetInteger("iAniIndex", 9);
        resurrection = false;

        //resurPassive = false;
        //Character.instance.CanMove = false;
    }
    
    void Update()
    {

        //if (resurPassive == false)
        //{
        //    resurrectionCount += 1f * Time.deltaTime;
        //    if (resurrectionCount >= 1.5f)
        //    {
        //        resurPassive = true;
        //        Character.instance.CanMove = true;
        //        resurrectionCount = 0f;
        //    }
        //}

        reAppear();
        canvascontrol();
        RestartActive();
        Onresurrection();
        InventoryKeycode();
        QuestWindowOnOff();


    }
}
