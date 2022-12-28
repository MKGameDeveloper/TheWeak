using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossSkillNotice : MonoBehaviour
{
    public static BossSkillNotice instance = null;

    public Text Notice;
    public bool noticeActive = false;

    string[] skillnotice = new string[6];

    public int NoticeNum = 0;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        skillnotice[0] = "드래곤이 리치를 소환하였습니다.";
        skillnotice[1] = "드래곤이 마나를 모두 소진하여 그로기 상태가 됩니다.";
        skillnotice[2] = "드래곤이 스켈레톤을 소환합니다.";
        skillnotice[3] = "드래곤이 디펜스 모드에 돌입합니다.";
        skillnotice[4] = "메테오가 소환됩니다. 10초간 생존하십시오.";
        skillnotice[5] = "드래곤을 처치하였습니다!";
    }

    // Update is called once per frame
    void Update()
    {
        if(noticeActive == true)
        {
            Notice.gameObject.SetActive(true);
        }
        else if(noticeActive == false)
        {
            Notice.gameObject.SetActive(false);
        }

        Notice.text = skillnotice[NoticeNum];
    }
}
