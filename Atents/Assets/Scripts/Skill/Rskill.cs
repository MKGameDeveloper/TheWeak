using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rskill : MonoBehaviour
{
    public GameObject skillEffect;

    //스킬슬롯의 쿨타임 및 액티브 이미지
    public Image cool;
    public Image ActiveSkill;

    //시전시간 표시 이미지 및 카운트
    public Image ActiveImage;
    public Text ActiveCount;

    float ActiveCounting;

    bool isActive = false;
    float ActiveOn = 1f;

    float SkillTime = 0f;

    float ActiveTime = 10f;
    float CoolSpeed = 30f;

    private void Awake()
    {
        skillEffect.gameObject.SetActive(false);
    }
    
    void Start()
    {
        ActiveImage.fillAmount = 1f;
        ActiveCount.gameObject.SetActive(false);
    }
    
    

    void UseRSkill()
    {
        if (Input.GetKeyDown(KeyCode.R)&& cool.fillAmount == 0f && isActive != true) 
        {
            ActiveSet();
            skillEffect.gameObject.SetActive(true);
            isActive = true;
        }
    }
    
    void ActiveSet()
    {
        ActiveCount.gameObject.SetActive(true);
        ActiveImage.fillAmount = 0f;
        ActiveCounting = 10.99f;
    }

    void SKillActive()
    {
        if(isActive == true)
        {
            Status.instance.BurfAtk = (int)(Status.instance.Atk * 0.5f);
            ActiveSkill.fillAmount += ActiveOn*Time.deltaTime;

            //시전시간 Setting
            ActiveCount.text = ((int)ActiveCounting).ToString();
            ActiveCounting -= Time.deltaTime;
            ActiveImage.fillAmount += Time.deltaTime / ActiveTime;

            if(ActiveSkill.fillAmount >= 1f)
            {
                ActiveOn *= -1f;
                ActiveSkill.fillClockwise = false;
            }
            else if(ActiveSkill.fillAmount <= 0f)
            {
                ActiveOn *= -1f;
                ActiveSkill.fillClockwise = true;
            }
        }
    }

    void Cool()
    {
        cool.fillAmount -= Time.deltaTime / CoolSpeed;
    }

    void Update()
    {
        if (isActive == true)
        {
            SkillTime += Time.deltaTime;
        }

        if(SkillTime > ActiveTime && isActive==true)
        {
            ActiveCount.gameObject.SetActive(false);
            Status.instance.BurfAtk -= (int)(Status.instance.Atk * 0.5f);
            ActiveSkill.fillAmount = 0f;
            cool.fillAmount = 1f;
            skillEffect.gameObject.SetActive(false);
            SkillTime = 0f;
            isActive = false;
        }

        //Debug.Log(Status.instance.curatk);

        Cool();
        SKillActive();
        UseRSkill();
    }
}
