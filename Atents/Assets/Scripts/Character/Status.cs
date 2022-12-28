using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour
{
    public static Status instance = null;

    public int Level;
    public int Atk;
    public int PlusAtk = 0;
    public int BurfAtk = 0;
    public int Def;
    public int PlusDef = 0;
    public int BurfDef = 0;
    public int Int;
    public int PlusInt = 0;
    public int BurfInt = 0;
    public int Agi;
    public int PlusAgi = 0;
    public int BurfAgi = 0;
    public int agiall;
    public int Hp;
    public int PlusHp;
    public int BurfHp;
    public int Mp;
    public int PlusMp;
    public int BurfMp;
    public float Exp;
    public float MaxExp;

    public float curatk;
    public float curdef;
    public float curint;
    public float curagi;
    public int curhp;
    public int curmp;

    public int BonusStatus;

    float time = 0f;
    public GameObject Effect;
    public Text LevelUp;
    float OffSetY = 130f;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Level = 1;
        Atk = 1000;
        Def = 10;
        Int = 1000;
        Agi = 100;
        Hp = 10;
        PlusHp = 0;
        Mp = 10;
        PlusMp = 0;
        Exp = 0;
        MaxExp = 200;
        BonusStatus = 10;
        SetStat();
    }

    void Start()
    {

    }

    void SetStat()
    {
        curatk = (Atk + PlusAtk + BurfAtk) * 4;
        curdef = 500 * (Def + PlusDef + BurfDef) / (100 + Def + PlusDef + BurfDef);
        curint = (Int + PlusInt + BurfInt) * 3;
        curagi = (Agi + PlusAgi + BurfAgi) * 0.03f;
        agiall = Agi + PlusAgi + BurfAgi;
        curhp = (Hp * 100) + PlusHp + BurfHp;
        curmp = (Mp * 100) + PlusMp + BurfMp;
    }


    void NextLevel()
    {
        if (Exp >= MaxExp)
        {

            Exp -= MaxExp;
            MaxExp = MaxExp * 1.4f;
            Level += 1;
            Effect.gameObject.SetActive(false);
            Effect.gameObject.SetActive(true);
            LevelUp.gameObject.SetActive(false);
            LevelUp.gameObject.SetActive(true);
            ResourceManager.instance.TextAlphaChange(LevelUp, 0f);
            BonusStatus += 10;
            time = 0f;
        }
    }

    public void LevelUpPos()
    {
        LevelUp.transform.position = Camera.main.WorldToScreenPoint(Character.instance.transform.position);
        LevelUp.transform.position += new Vector3(0, OffSetY, 0);

    }


    void Update()
    {
        if (Exp < 0)
        {
            Exp = 0;
        }



        time += Time.deltaTime;
        ResourceManager.instance.TextAlphaBlending_Plus(LevelUp);

        if (time > 2f)
        {
            Effect.gameObject.SetActive(false);
            LevelUp.gameObject.SetActive(false);
            time = 0f;
        }

        LevelUpPos();
        SetStat();
        NextLevel();
    }
}
