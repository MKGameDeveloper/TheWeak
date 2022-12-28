using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance = null;

    public struct QuestInfo
    {
        public int QuestNum;
        public string ment1;
        public string ment2;
        public string ment3;
        public string ment4;
        public string ment5;
        public int MaxMentCount;
        public int NpcNum;
        public string Reward;
        public string RequireStr;
        public string Accept;
        public string Reject;
        public string QuestName;
        public int CompleteNpcNum;
        public string Cment1;
        public string Cment2;
        public string Cment3;
        public string Cment4;
        public string Cment5;
        public int MaxCmentCount;
        public int Type;
        public int Require;
        public int Count;
        public string Contents;
    }

    //첫번째 보상 포탈 개방
    public GameObject Portal;
    public GameObject Button;

    //저장한 퀘스트들의 정보
    public List<QuestInfo> questlist = new List<QuestInfo>();


    //퀘스트 멘트 내용
    public string[] ment = new string[5];
    public int ProgressNum = 0;
    //진행 퀘스트 멘트갯수
    public int maxStr;


    //진행중인 퀘스트 번호
    public int QuestIndex= 0;

    //퀘스트 수락여부
    public bool Accept = false;

    //진행중인 퀘스트 상황
    public QuestInfo Quest;
    public bool Success = false;

    //퀘스트 진행 여부에 따른 마크
    public Image CanQuest;
    public Image NotComplete;
    public Image Complete;
    float OffSetY = 170f;

    //퀘스트 수락,진행,완료 버튼
    public GameObject QuestAccept;
    public GameObject QuestReject;
    public GameObject QuestProgress;
    public GameObject QuestComplete;

    //퀘스트 보상창
    public GameObject QuestRewardWindow;

    //퀘스트 진행상황
    public int Count = 0;

    //퀘스트창에 등록될 퀘스트 오브젝트
    GameObject QuestObj;
    public ScrollRect sr;

    //퀘스트창에 등록될 퀘스트 리스트
    public List<Quest> getquestlist = new List<Quest>();
    Quest getquest;
    public Text Contents;
    public Text RequireStr;
    public Text RequireCount;
    public string requirename = null;

    //완료창


    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        QuestObj = Resources.Load("Quest") as GameObject;
        
    }

    void Start()
    {
        if (QuestRewardWindow.gameObject.activeSelf == false)
        {
            QuestRewardWindow.gameObject.SetActive(true);
            QuestRewardWindow.gameObject.SetActive(false);
        }
        ReadQuestInfo("QuestData.csv");
    }

    void MarkPos()
    {

        NPC Pos = NPCManager.instance.npclist.Find(o => o.index == Quest.NpcNum);
        Vector3 Position = Camera.main.WorldToScreenPoint(Pos.transform.position);
        Vector3 Mark = new Vector3(Position.x, Position.y + OffSetY, Position.z);

        if (Accept != true && Success != true)
        {
            CanQuest.gameObject.SetActive(true);
            NotComplete.gameObject.SetActive(false);
            Complete.gameObject.SetActive(false);

            CanQuest.transform.position = Mark;
            
        }
        else if(Accept == true && Success != true)
        {
            CanQuest.gameObject.SetActive(false);
            NotComplete.gameObject.SetActive(true);
            Complete.gameObject.SetActive(false);

            NotComplete.transform.position = Mark;
        }
        else if (Accept == false && Success == true)
        {
            CanQuest.gameObject.SetActive(false);
            NotComplete.gameObject.SetActive(false);
            Complete.gameObject.SetActive(true);

            Complete.transform.position = Mark;
        }
    }

    void GetQuest()
    {
        GameObject instance = GameObject.Instantiate(QuestObj) as GameObject;

        Text Qname = instance.transform.GetChild(0).GetComponent<Text>();
        Qname.text = Quest.QuestName;

        Quest quest = instance.AddComponent<Quest>();

        quest.QuestNum = Quest.QuestNum;
        quest.QuestContents = Quest.Contents;
        quest.RequireStr = Quest.RequireStr;
        if(Quest.Type == 0)
        {
            NPC hi = NPCManager.instance.npclist.Find(o => o.index == Quest.Require);
            requirename = hi.NpcName;
        }
        else if(Quest.Type == 1)
        {
            MobStatus hi = MonsterManager.instance.mobstatlist.Find(o => o.index == Quest.Require);
            requirename = hi.Name;
           
        }
        else if(Quest.Type == 2)
        {
            Item_s hi = ItemManager.instance.ItemInfolist.Find(o => o.index == Quest.Require);
            requirename = hi.ItemName;
        }
        quest.RequireCount = requirename + " " + Count + "/" + Quest.Count;


        instance.transform.SetParent(sr.content.transform);
        instance.transform.SetAsFirstSibling();

        

        getquestlist.Add(quest);

    }

    //퀘스트 정보를 읽어오는 함수
    public void ReadQuestInfo(string FileName)
    {
        string strPath = Application.dataPath + "/Resources/" + FileName;
        FileStream Questfile = new FileStream(strPath, FileMode.Open, FileAccess.Read);
    StreamReader reader = new StreamReader(Questfile, System.Text.Encoding.UTF8);      
                                                                                       
        string line = string.Empty;                                                    
                                                                                       
        //헤드라인을 미리 읽어놓음.                                                      
        reader.ReadLine();                                                             
                                                                                       
        while ((line = reader.ReadLine()) != null)                                     
        {                                                                              
                                                                                       
            string[] strData = line.Split(',');                                        
                                                                                       
            QuestInfo info = new QuestInfo();                                          
            info.QuestNum = int.Parse(strData[0]);                                     
            info.ment1 = strData[1];                                                   
            info.ment2 = strData[2];                                                   
            info.ment3 = strData[3];                                            
            info.ment4 = strData[4];                                                   
            info.ment5 = strData[5];                                                   
            info.MaxMentCount = int.Parse(strData[6]);
            info.NpcNum = int.Parse(strData[7]);
            info.Reward = strData[8];
            info.RequireStr = strData[9];
            info.Accept = strData[10];
            info.Reject = strData[11];
            info.QuestName = strData[12];
            info.CompleteNpcNum = int.Parse(strData[13]);
            info.Cment1 = strData[14];
            info.Cment2 = strData[15];
            info.Cment3 = strData[16];
            info.Cment4 = strData[17];
            info.Cment5 = strData[18];
            info.MaxCmentCount = int.Parse(strData[19]);
            info.Type = int.Parse(strData[20]);
            info.Require = int.Parse(strData[21]);
            info.Count = int.Parse(strData[22]);
            info.Contents = strData[23];


            questlist.Add(info);


        }
        reader.Close();

    }

    //진행 버튼을 누를시 다음 대사가 호출됨.
    public void NextMent_BT()
    {
        NPCManager.instance.ButtonDown();
        ProgressNum += 1;
    }

    //수락 버튼을 눌렀을시 Type에 따른 퀘스트가 진행됨.
    public void AcceptQuest_BT()
    {
        NPCManager.instance.ButtonDown();
        NPCManager.instance.EndComunity.gameObject.SetActive(true);
        ProgressNum += 1;
        NPCManager.instance.script.text = Quest.Accept;
        Accept = true;
        Count = 0;
        GetQuest();
        GetReward();
        if (Quest.Type == 0)
        {
            Count += 1;
            Accept = false;
            Success = true;
        }
    }
    public void RejectQuest_BT()
    {
        NPCManager.instance.ButtonDown();
        NPCManager.instance.EndComunity.gameObject.SetActive(true);
        ProgressNum += 1;
        NPCManager.instance.script.text = Quest.Reject;
    }

    //퀘스트 요구조건에 따른 완료방법.(대화형은 수락단계에서 바로 완료됨)
    public void QuestRequire()
    {
        //사냥형
        if (Quest.Type == 1)
        {
            if (Count >= Quest.Count)
            {
                Count = Quest.Count;
                if (Accept == true)
                {
                    Accept = false;
                    Success = true;
                }
            }
        }
        //수집형
        else if (Quest.Type == 2 && Accept == true)
        {
            InventorySlot pick = Inventory.instance.slotlist.Find(o => (o.ItemInfo.index.Equals(Quest.Require)));
            if (pick != null)
            {
                Count = pick.ItemCount;
                if (Count >= Quest.Count)
                {
                    Count = Quest.Count;
                }
            }            
            if (Count >= Quest.Count)
            {
                Accept = false;
                Success = true;
            }
        }
        //보스형
        else if (Quest.Type == 3)
        {
            if (Count >= 1) 
            {
                Accept = false;
                Success = true;
            }
        }
    }

    //완료시 조건 초기화 후 보상 지급, 퀘스트Number는 1씩 증가
    public void QuestComplete_BT()
    {

        
        if (Quest.QuestNum == 0)
        {
            Portal.GetComponent<Portal>().enabled = true;
            Portal.transform.GetChild(0).gameObject.SetActive(true);
            Button.gameObject.SetActive(true);
        }
        if(Quest.Type == 2)
        {
            InventorySlot pick = Inventory.instance.slotlist.Find(o => (o.ItemInfo.index.Equals(Quest.Require)));
            if (pick != null)
            {
                pick.ItemCount -= Quest.Count;
                
            }
            
        }
        getquest.Complete = 1;
        Count = 0;
        NPCManager.instance.ComunityEnd();
        QuestRewardWindow.gameObject.SetActive(true);
        Success = false;
        QuestIndex += 1;
    }

    public void QuestReward_BT()
    {

        QuestRewardWindow.gameObject.SetActive(false);
    }


    public void GetReward()
    {
        RewardWindow.instance.QName.text = Quest.QuestName;
        RewardWindow.instance.Qcontents.text = Quest.RequireStr;

    }


    public void QuestNotChoice()
    {
        for(int i = 0; i< getquestlist.Count; i++)
        {
            getquestlist[i].select = 0;
        }
    }

    //퀘스트창 등록퀘스트 찾는함수
    public void FindGetQuest()
    {
        getquest = getquestlist.Find(o => o.QuestNum == QuestIndex);
        //if (getquest.Complete == 0 && getquest != null)
        //{
        //    getquest.RequireCount = Quest.Require + " " + Count + "/" + Quest.Count;
        //}
    }

    public void FindQuest()
    {

        Quest = questlist.Find(o => o.QuestNum == QuestIndex);
        if (Success == false)
        {
            maxStr = Quest.MaxMentCount;
            ment[0] = Quest.ment1;
            ment[1] = Quest.ment2;
            ment[2] = Quest.ment3;
            ment[3] = Quest.ment4;
            ment[4] = Quest.ment5;
        }
        else if (Success == true)
        {
            Quest.NpcNum = Quest.CompleteNpcNum;
            maxStr = Quest.MaxCmentCount;
            ment[0] = Quest.Cment1;
            ment[1] = Quest.Cment2;
            ment[2] = Quest.Cment3;
            ment[3] = Quest.Cment4;
            ment[4] = Quest.Cment5;
        }
        
    }
   
    
    void Update()
    {
        MarkPos();
        FindGetQuest();
        QuestRequire();
        FindQuest();
    }
}
