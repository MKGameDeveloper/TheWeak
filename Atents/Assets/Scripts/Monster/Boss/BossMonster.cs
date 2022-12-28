using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class BossMonster : MonoBehaviour
{
    public static BossMonster instance = null;

    public Image HPbar;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        ReadBossStat("BossStatus.csv");
    }

    public GameObject target;

    //보스 스테이터스
    public string Name;
    public float Atk;
    public float Def;
    public float MaxHp;
    public float Hp;
    public float CurHp;

    void Start()
    {
        
    }

    //몬스터 상태정보를 불러오는 함수
    public void ReadBossStat(string FileName)
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
            
            Name = strData[0];
            Atk = float.Parse(strData[1]);
            Def = float.Parse(strData[2]);
            MaxHp = float.Parse(strData[3]);
            Hp = float.Parse(strData[4]);
            CurHp = float.Parse(strData[5]);
            
        }
        reader.Close();

    }

   
    

    void Update()
    {
        CurHp = Hp;
        HPbar.fillAmount = CurHp/MaxHp;
    }
}
