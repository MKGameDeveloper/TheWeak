using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class StatWindow : MonoBehaviour
{
    public GameObject StatusWD;

    public GameObject stBT;

    public Text statTxt;
    public Text levelTxt;
    public Text bonusTxt;
    public Text UpstTxt;
    public Text UiStatWindow_1;
    public Text UiStatWindow_2;
    public Text UiStatName;

    int stAtk;
    int stDef;
    int stInt;
    int stAgi;
    int stHp;
    int stMp;

    public int Atk = 0;
    public int Def = 0;
    public int Int = 0;
    public int Agi = 0;
    public int Hp = 0;
    public int Mp = 0;

    string tAtk;
    string tDef;
    string tInt;
    string tAgi;
    string tHp;
    string tMp;

    float notice = 0f;

    void Start()
    {
        stAtk = Status.instance.Atk;
        stDef = Status.instance.Def;
        stInt = Status.instance.Int;
        stAgi = Status.instance.Agi;
        stHp = Status.instance.Hp;
        stMp = Status.instance.Mp;
    }

    void UiStatWindow()
    {
        UiStatWindow_1.text = "ATK  :  " + Status.instance.Atk + "\n" + "DEF  :  " + Status.instance.Def + "\n" + "INT   :  "
            + Status.instance.Int + "\n" + "AGI   :  " + Status.instance.Agi;
        UiStatWindow_2.text = "HP   :  " + Status.instance.Hp + "\n" + "MP   :  " + Status.instance.Mp;
    }

    //x아이콘을 누르면 스텟창이 꺼짐
    public void xIconClick()
    {
        StatusWD.gameObject.SetActive(false);
    }

    //F를 누르면 스텟창이 켜지고 꺼짐
    void StatusKeyDownF()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (StatusWD.gameObject.activeSelf == false)
            {
                StatusWD.gameObject.SetActive(true);
            }
            else if (StatusWD.gameObject.activeSelf == true)
            {
                StatusWD.gameObject.SetActive(false);
            }
        }
    }

    //보너스 스텟이 0보다 작거나 같으면 스텟 조정버튼이 비활성화
    void StatusBTOnOff()
    {
        if(Status.instance.BonusStatus > 0)
        {
            stBT.gameObject.SetActive(true);
            if(notice > 1f)
            {
                notice = 0f;
            }
            if(notice <= 0.5f)
            {
                UiStatName.text = "<color=#ff0000>" + "STATUS" + "</color>";
            }
            else if( notice > 0.5f)
            {
                UiStatName.text = "";
            }
            
            
        }
    }

    //accept 버튼을 누를시 추가 스텟
    public void AcceptOnClick()
    {
        Status.instance.Atk = stAtk;
        Status.instance.Def = stDef;
        Status.instance.Int = stInt;
        Status.instance.Agi = stAgi;
        Status.instance.Hp = stHp;
        Status.instance.Mp = stMp;
        Atk -= Atk;
        Def -= Def;
        Int -= Int;
        Agi -= Agi;
        Hp -= Hp;
        Mp -= Mp;

        stBT.gameObject.SetActive(false);
        UiStatName.text ="STATUS";
    }

    public void CancleOnClick()
    {
        stAtk -= Atk;
        stDef -= Def;
        stInt -= Int;
        stAgi -= Agi;
        stHp -= Hp;
        stMp -= Mp;
        Status.instance.BonusStatus += (Atk + Def + Int + Agi + Hp + Mp);
        Atk -= Atk;
        Def -= Def;
        Int -= Int;
        Agi -= Agi;
        Hp -= Hp;
        Mp -= Mp;

    }

    void StatColor()
    {
        //atk 글자색 변경
        if ((stAtk - Atk) != stAtk)
        {
            tAtk = "<color=#ff0000>" + stAtk + "</color>";
        }
        else if((stAtk - Atk) == stAtk)
        {
            tAtk = stAtk.ToString();
        }

        //def 글자색 변경
        if ((stDef - Def) != stDef)
        {
            tDef = "<color=#ff0000>" + stDef + "</color>";
        }
        else if ((stDef - Def) == stDef)
        {
            tDef = stDef.ToString();
        }

        //int 글자색 변경
        if ((stInt - Int) != stInt)
        {
            tInt = "<color=#ff0000>" + stInt + "</color>";
        }
        else if ((stInt - Int) == stInt)
        {
            tInt = stInt.ToString();
        }

        //agi 글자색 변경
        if ((stAgi - Agi) != stAgi)
        {
            tAgi = "<color=#ff0000>" + stAgi + "</color>";
        }
        else if ((stAgi - Agi) == stAgi)
        {
            tAgi = stAgi.ToString();
        }

        //hp 글자색 변경
        if ((stHp - Hp) != stHp)
        {
            tHp = "<color=#ff0000>" + stHp + "</color>";
        }
        else if ((stHp - Hp) == stHp)
        {
            tHp = stHp.ToString();
        }

        //mp 글자색 변경
        if ((stMp - Mp) != stMp)
        {
            tMp = "<color=#ff0000>" + stMp + "</color>";
        }
        else if ((stMp - Mp) == stMp)
        {
            tMp = stMp.ToString();
        }
    }

    //레벨을 표시해주는 텍스트
    void Levelexp()
    {
        levelTxt.text = "Level : " + Status.instance.Level;
    }

    //스텟을 표시해주는 텍스트
    void Statexp()
    {
        statTxt.text = "Atk : " + tAtk + "\n" + "Def : " + tDef + "\n" +
            "Int : " + tInt + "\n" + "Agi : " + tAgi + "\n" +
            "Hp : " + tHp + "\n" + "Mp : " + tMp + "\n";
    }

    void Upstat()
    {
        UpstTxt.text = "(+" + Atk + ")\n" + "(+" + Def + ")\n" + "(+" + Int + ")\n" + "(+" + Agi + ")\n" + "(+" + Hp + ")\n" + "(+" + Mp + ")\n";
    }

    void Bonusexp()
    {
        if(Status.instance.BonusStatus > 0)
        {
            bonusTxt.text = "Bonus Status : " + "<color=#ff0000>" + Status.instance.BonusStatus + "</color>";
        }
        else
        {
            bonusTxt.text = "Bonus Status : " + Status.instance.BonusStatus;
        }
        
    }


    public void AtkStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stAtk += 1;
            Atk += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    public void DefStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stDef += 1;
            Def += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    public void IntStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stInt += 1;
            Int += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    public void AgiStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stAgi += 1;
            Agi += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    public void HpStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stHp += 1;
            Hp += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    public void MpStatClick()
    {
        if (Status.instance.BonusStatus > 0)
        {
            stMp += 1;
            Mp += 1;
            Status.instance.BonusStatus -= 1;
        }
    }

    void Update()
    {
        notice += Time.deltaTime;

        UiStatWindow();
        StatColor();
        Upstat();
        StatusKeyDownF();
        StatusBTOnOff();
        Bonusexp();
        Statexp();
        Levelexp();
    }
}
