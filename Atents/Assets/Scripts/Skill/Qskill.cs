using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Qskill : MonoBehaviour
{
    public GameObject targetingRound;
    public GameObject skillEffect;

    float RoundRadius = 2f;


    bool isRound = false;

    Vector3 mousepos;

    public Image cool;

    float skill;
    public float coolSpeed;

    float Damage;

    void Start()
    {
        cool.fillAmount = 0f;
        skillEffect.gameObject.SetActive(false);
        targetingRound.gameObject.SetActive(false);
    }

    void Damage_Cool()
    {
        Damage = (Status.instance.curint)*1.2f;
        coolSpeed = 12f - (float)Status.instance.Int * 0.003f;
        

        cool.fillAmount -= Time.deltaTime / coolSpeed;
    }

    void UseQSkill()
    {

        if (Input.GetKeyDown(KeyCode.Q) && cool.fillAmount == 0f)
        {
            Character.instance.state = Character.STATE.SKILL;
            isRound = true;
            targetingRound.gameObject.SetActive(true);

        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            targetingRound.gameObject.SetActive(false);
            isRound = false;
        }

        if (isRound == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitinfo;

            if (Physics.Raycast(ray, out hitinfo))
            {
                Vector3 RoundPos = hitinfo.point;
                RoundPos.y = 0.1f;
                targetingRound.transform.position = RoundPos;
                if (Input.GetMouseButtonDown(0))
                {
                    skillEffect.gameObject.SetActive(false);
                    skillEffect.transform.position = RoundPos;
                    for (int i = 0; i < MonsterManager.instance.monsterlist.Count; i++)
                    {
                        if (Vector3.Distance(MonsterManager.instance.monsterlist[i].transform.position, RoundPos) < 2.2f)
                        {
                            float realDamage = Damage - MonsterManager.instance.monsterlist[i].defense;
                            if(realDamage <= 0f)
                            {
                                realDamage = 1;
                            }
                            MonsterManager.instance.monsterlist[i].Hp -= realDamage;
                        }
                    }
                    skillEffect.gameObject.SetActive(true);
                    skill = 0f;
                    cool.fillAmount = 1f;
                    targetingRound.gameObject.SetActive(false);
                    isRound = false;
                }
            }
        }
    }

    void Update()
    {

        skill += Time.deltaTime;

        if (skill >= 2.2f)
        {
            skillEffect.gameObject.SetActive(false);
        }

        UseQSkill();
        Damage_Cool();

    }
}
