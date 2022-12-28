using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance = null;

    public GameObject roundring;

    //UI창
    public GameObject ui;
    //대화창
    public GameObject comunity;
    public Text strName;
    public Text script;
    public Button EndComunity;

    public GameObject Shop;
    public GameObject weapon;
    public GameObject heal;
    public GameObject portal;

    public GameObject PortalWindow;

    //퀘스트를 진행하는지를 판별할 논리 변수
    public bool isQuest = false;
    NPC npc;


    //NPC 렌더링 카메라
    public Transform RenderCamera;
    float rpos;

    NPC active = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        strName = comunity.transform.GetChild(1).GetComponent<Text>();
        script = comunity.transform.GetChild(2).GetComponent<Text>();

    }

    public struct NPCinfo
    {
        public string name;
        public string str_1;
        public string str_2;
        public int index;
    }

    //npc 대사, 이름 등을 저장한 정보리스트
    public List<NPCinfo> npcinfolist = new List<NPCinfo>();

    //npc들을 저장한 리스트
    public List<NPC> npclist = new List<NPC>();

    //대화창에 사용될 버튼들을 저장한 리스트
    public List<ButtonOff> buttonlist = new List<ButtonOff>();

    void Start()
    {
        ReadNPCInfo("NPC.csv");
        NPCSetting();

        rpos = RenderCamera.position.x;
    }

    public void ReadNPCInfo(string FileName)
    {
        string strPath = Application.dataPath + "/Resources/" + FileName;
        FileStream npcfile = new FileStream(strPath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(npcfile, System.Text.Encoding.UTF8);

        string line = string.Empty;

        //헤드라인을 미리 읽어놓음.
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {

            string[] strData = line.Split(',');

            NPCinfo info = new NPCinfo();
            info.name = strData[0];
            info.str_1 = strData[1];
            info.str_2 = strData[2];
            info.index = int.Parse(strData[3]);
            npcinfolist.Add(info);


        }
        reader.Close();

    }

    //NPC 대사 및 이름, index 저장
    public void NPCSetting()
    {
        for (int i = 0; i < npclist.Count; i++)
        {
            npclist[i].NpcName = npcinfolist[i].name;
            npclist[i].str_1 = npcinfolist[i].str_1;
            npclist[i].str_2 = npcinfolist[i].str_2;
            npclist[i].index = npcinfolist[i].index;
        }
    }

    public void ButtonDown()
    {
        for (int i = 0; i < buttonlist.Count; i++)
        {
            buttonlist[i].gameObject.SetActive(false);
        }
    }

    public void NpcActive()
    {
        if(active == null)
        {
            npc = npclist.Find(o => (o.distance <= 2f));

            if(npc != null)
            {
                roundring.transform.position = new Vector3(npc.transform.position.x, 0.1f, npc.transform.position.z);
                roundring.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    ButtonDown();
                    Vector3 cameraMove = new Vector3(rpos + (npc.index * 3), RenderCamera.position.y, RenderCamera.position.z);
                    RenderCamera.position = cameraMove;
                    int num = ResourceManager.instance.RandomInteger(2);

                    if (QuestManager.instance.Quest.NpcNum != npc.index || QuestManager.instance.Accept == true)
                    {
                        EndComunity.gameObject.SetActive(true);

                        if (npc.index == 0)
                        {
                            Shop.gameObject.SetActive(true);
                        }

                        else if (npc.index == 3)
                        {
                            portal.gameObject.SetActive(true);
                        }
                        else if (npc.index == 4)
                        {
                            heal.gameObject.SetActive(true);
                        }
                        else if (npc.index == 5)
                        {
                            weapon.gameObject.SetActive(true);
                        }

                        ui.gameObject.SetActive(false);
                        comunity.gameObject.SetActive(true);
                        strName.text = npc.NpcName;
                        if (num == 0)
                        {
                            script.text = npc.str_1;
                        }
                        else if (num == 1)
                        {
                            script.text = npc.str_2;
                        }

                        if (npc.distance > 2f)
                        {
                            npc = null;
                        }
                    }
                    else if (QuestManager.instance.Quest.NpcNum == npc.index && QuestManager.instance.Accept == false) 
                    {
                        isQuest = true;
                    }
                }
            }
            else if(npc == null)
            {
                ButtonDown();
                roundring.gameObject.SetActive(false);
                comunity.gameObject.SetActive(false);
                ui.gameObject.SetActive(true);
            }
            
        }
    }

    
    public void QuestIng()
    {
        if ( isQuest == true )
        {
            strName.text = npc.NpcName;
            if (QuestManager.instance.ProgressNum <= (QuestManager.instance.maxStr - 1))
            {
                script.text = QuestManager.instance.ment[QuestManager.instance.ProgressNum];
            }

            ui.gameObject.SetActive(false);
            comunity.gameObject.SetActive(true);


            if (npc.distance > 2f)
            {
                QuestManager.instance.ProgressNum = 0;
                npc = null;
                ComunityEnd();
            }
            if (QuestManager.instance.ProgressNum < (QuestManager.instance.maxStr - 1))
            {
                QuestManager.instance.QuestProgress.gameObject.SetActive(true);
                EndComunity.gameObject.SetActive(true);
            }
            else if (QuestManager.instance.ProgressNum == (QuestManager.instance.maxStr - 1) && QuestManager.instance.Success == false)
            {
                QuestManager.instance.QuestAccept.gameObject.SetActive(true);
                QuestManager.instance.QuestReject.gameObject.SetActive(true);
            }
            else if(QuestManager.instance.ProgressNum == (QuestManager.instance.maxStr - 1) && QuestManager.instance.Success == true)
            {
                QuestManager.instance.QuestComplete.gameObject.SetActive(true);
            }
        }
    }

    public void ComunityEnd()
    {
        ButtonDown();
        isQuest = false;
        QuestManager.instance.ProgressNum = 0;
        comunity.gameObject.SetActive(false);
        ui.gameObject.SetActive(true);
    }

    public void ShopUse()
    {

    }

    public void WeaponUse()
    {

    }

    public void PortalUse()
    {
        PortalWindow.gameObject.SetActive(true);
        ButtonDown();
        comunity.gameObject.SetActive(false);
        ui.gameObject.SetActive(true);
    }
    
    void Update()
    {
        QuestIng();
        NpcActive();
    }
}
