using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public struct MobStatus
{
    public int index;
    public float speed;
    public float Exp;
    public float Atk;
    public float Def;
    public float MaxHp;
    public float Hp;
    public float CurHp;
    public string drop0;
    public string drop1;
    public string drop2;
    public string drop3;
    public string drop4;
    public string drop5;
    public string Name;
}

public class MonsterManager : MonoBehaviour
{
    public Transform east;
    public Transform north;


    //자식트랜스폼.setparent(부모트랜스폼)

    //몬스터의 소환위치를 저장할 구조체
    public struct monster_01
    {
        public float x;
        public float y;
        public float z;
        public int MonsterNum;
        
    }

   


    //몬스터의 소환위치 정보를 담은 구조체를 저장할 리스트
    List<monster_01> positionlist = new List<monster_01>();

    //몬스터의 상태정보를 담은 구조체리스트
    public List<MobStatus> mobstatlist = new List<MobStatus>();

    //소환된 몬스터들을 저장할 리스트
    public List<Monster> monsterlist = new List<Monster>();

    //몬스터 종류들을 저장할 배열
    public GameObject[] monster_;


    public static MonsterManager instance = null;

    

    GameObject targeting;
    public GameObject target;



    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        monster_ = Resources.LoadAll<GameObject>("Monster/");

        targeting = Resources.Load<GameObject>("targeting") as GameObject;

    }

    void Start()
    {
        ReadPosition("Monster_position.csv");
        ReadMobStat("MonsterStatus.csv");

        target = Instantiate(targeting);
        target.gameObject.SetActive(false);

        MakeMonster();


    }

    //몬스터 상태정보를 불러오는 함수
    public void ReadMobStat(string FileName)
    {
        string strPath = Application.dataPath + "/Resources/" + FileName;
        FileStream monsterStatus = new FileStream(strPath, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(monsterStatus, System.Text.Encoding.UTF8);

        string line = string.Empty;

        //헤드라인을 미리 읽어놓음.
        reader.ReadLine();

        while ((line = reader.ReadLine()) != null)
        {

            string[] strData = line.Split(',');

            MobStatus Info = new MobStatus();

            Info.index = int.Parse(strData[0]);
            Info.speed = float.Parse(strData[1]);
            Info.Exp = float.Parse(strData[2]);
            Info.Atk = float.Parse(strData[3]);
            Info.Def = float.Parse(strData[4]);
            Info.MaxHp = float.Parse(strData[5]);
            Info.Hp = float.Parse(strData[6]);
            Info.CurHp = float.Parse(strData[7]);
            Info.drop0 = strData[8];
            Info.drop1 = strData[9];
            Info.drop2 = strData[10];
            Info.drop3 = strData[11];
            Info.drop4 = strData[12];
            Info.drop5 = strData[13];
            Info.Name = strData[14];

            mobstatlist.Add(Info);


        }
        reader.Close();

    }


    //csv 파일로 몬스터 위치정보를 불러오는 함수
    public void ReadPosition(string FileName)
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

            monster_01 position = new monster_01();
            position.x = float.Parse(strData[0]);
            position.y = float.Parse(strData[1]);
            position.z = float.Parse(strData[2]);
            position.MonsterNum = int.Parse(strData[3]);

            positionlist.Add(position);


        }
        reader.Close();

    }
    
    //몬스터를 인스턴싱하는 함수
    void MakeMonster()
    {
        for (int i = 0; i < positionlist.Count; i++)
        {
            if (positionlist[i].MonsterNum == 0)
            {
                Vector3 msPosition;
                msPosition.x = positionlist[i].x;
                msPosition.y = positionlist[i].y;
                msPosition.z = positionlist[i].z;

                float angle = UnityEngine.Random.Range(-180f, 180f);
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

                GameObject obj = GameObject.Instantiate(monster_[positionlist[i].MonsterNum], msPosition, rotation);

                obj.name = "RatMan_0_" + i.ToString();
                Ratman monster = obj.AddComponent<Ratman>();
                monster.Index = positionlist[i].MonsterNum;
                MobStatus Info = mobstatlist.Find(o => o.index == monster.Index);
                monster.Index = Info.index;
                monster.monsterSpeed = Info.speed;
                monster.MonsterExp = Info.Exp;
                monster.Atk = Info.Atk;
                monster.defense = Info.Def;
                monster.MaxHp = Info.MaxHp;
                monster.Hp = Info.Hp;
                monster.curhp = Info.CurHp;
                monster.drop0 = Info.drop0;
                monster.drop1 = Info.drop1;
                monster.drop2 = Info.drop2;
                monster.drop3 = Info.drop3;
                monster.drop4 = Info.drop4;
                monster.drop5 = Info.drop5;


                monster.transform.SetParent(east);

                monsterlist.Add(monster);
            }

            else if (positionlist[i].MonsterNum == 1)
            {
                Vector3 msPosition;
                msPosition.x = positionlist[i].x;
                msPosition.y = positionlist[i].y;
                msPosition.z = positionlist[i].z;

                float angle = UnityEngine.Random.Range(-180f, 180f);
                Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

                GameObject obj = GameObject.Instantiate(monster_[positionlist[i].MonsterNum], msPosition, rotation);

                obj.name = "Skeleton_0_" + i.ToString();
                Skeleton monster = obj.AddComponent<Skeleton>();
                monster.Index = positionlist[i].MonsterNum;
                MobStatus Info = mobstatlist.Find(o => o.index == monster.Index);
                monster.Index = Info.index;
                monster.monsterSpeed = Info.speed;
                monster.MonsterExp = Info.Exp;
                monster.Atk = Info.Atk;
                monster.defense = Info.Def;
                monster.MaxHp = Info.MaxHp;
                monster.Hp = Info.Hp;
                monster.curhp = Info.CurHp;
                monster.drop0 = Info.drop0;
                monster.drop1 = Info.drop1;
                monster.drop2 = Info.drop2;
                monster.drop3 = Info.drop3;
                monster.drop4 = Info.drop4;
                monster.drop5 = Info.drop5;

                monster.transform.SetParent(north);

                monsterlist.Add(monster);
            }


        }
    }
   


    //몬스터가 죽었을시 다시 소환하는 함수
    void filledMonster()
    {
        for(int i = 0; i<monsterlist.Count; i++)
        {
            if (monsterlist[i].Index == 2)
                return;

            if(monsterlist[i].alive == false && monsterlist[i].reSpawnStart == false)
            {
                monsterlist[i].reSpawnStart = true;
                monsterlist[i].reSpawnTime = 0f;
                
            }
            if(monsterlist[i].reSpawnStart == true)
            {
                monsterlist[i].reSpawnTime += Time.deltaTime;
                
                if(monsterlist[i].reSpawnTime >= 15f)
                {
                    monsterlist[i].transform.position = monsterlist[i].curPos;
                    monsterlist[i].reSpawnStart = false;
                    monsterlist[i].alive = true;
                    monsterlist[i].gameObject.SetActive(true);
                    monsterlist[i].hp.gameObject.SetActive(true);
                    monsterlist[i].Damage = false;
                }
            }
        }
    }

    //캐릭터가 몬스터를 타겟으로 지정하는 함수
    public void Targeting()
    {
        if(Character.instance.Target == null)
        {
            for(int i=0; i<monsterlist.Count; i++)
            {
                if (Vector3.Distance(monsterlist[i].transform.position, Character.instance.transform.position) < 6f  
                    && monsterlist[i].alive == true && monsterlist[i].gameObject.activeSelf == true)
                {
                    Character.instance.Target = monsterlist[i];
                }
                
            }
        }
        if (Character.instance.Target != null)
        {
                target.gameObject.SetActive(true);
                target.transform.position = Character.instance.Target.transform.position;
                target.transform.position += new Vector3(0, 0.01f, 0);

            if (Vector3.Distance(Character.instance.Target.transform.position, Character.instance.transform.position) > 8f 
                || Character.instance.Target.alive == false)
            {
                Character.instance.Target = null;

                target.gameObject.SetActive(false);
            }
        }
    }

    

    
    
    void Update()
    {
        Targeting();
        filledMonster();
    }
}
