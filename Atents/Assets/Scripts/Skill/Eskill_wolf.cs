using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eskill_wolf : MonoBehaviour
{
    public static Eskill_wolf instance = null;

    bool attack = false;
    public bool die = false;

    float distance;
    Vector3 direction;

    float wolfTocharSpeed;
    float wolfToMonsterSpeed;
    float wolfTurnSpeed = 500f;

    public Animator ani;
    float aniWalkSpeed = 2f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        ani = GetComponent<Animator>();
    }

    

    void Start()
    {
        
    }

    public void Attack()
    {
        if (Character.instance.Target != null)
        {
            Character.instance.Target.Hp -= (10f +(((float)Status.instance.Int) * 0.5f));
        }
    }

    public void WolfDie()
    {
        gameObject.SetActive(false);
        die = false;
    }

    public void AttackEnd()
    {
        attack = false;
    }

    void AttackMonster()
    {
        
        Vector3 CharPos = Character.instance.transform.position;

        //캐릭터의 타겟이 활성화 되있을 시
        if (Character.instance.Target != null && die != true)
        {
            Vector3 targetPos = Character.instance.Target.transform.position;

            //distance = 늑대와 타겟의 거리
            distance = Vector3.Distance(targetPos, transform.position);


            //만약 늑대의 위치와 타겟 위치의 거리가 1.5 이상일시 늑대가 타겟을 추격
            if (2f <= distance && attack != true)
            {
                ani.SetInteger("WolfAniIndex", 2);
                transform.position = Vector3.MoveTowards(transform.position, targetPos, wolfToMonsterSpeed * Time.deltaTime);
                direction = targetPos - transform.position;
                direction = new Vector3(direction.x, 0f, direction.z);
            }

            //늑대와 타겟 사이의 거리가 1.5이하라면 늑대가 타겟을 공격함
            else if (distance < 2f)
            {
                attack = true;
                ani.SetInteger("WolfAniIndex", 3);
                direction = targetPos - transform.position;
                direction = new Vector3(direction.x, 0f, direction.z);
            }
        }

        //캐릭터의 타겟이 비활성화일시
        else if (Character.instance.Target == null && die != true)
        {
            distance = Vector3.Distance(Character.instance.transform.position, transform.position);

            if (distance >= 12f)
            {
                Vector3 wolfPos;
                Vector3 charPos = Character.instance.transform.position;
                wolfPos = new Vector3(Random.Range(charPos.x - 1f, charPos.x + 1f), 0f, Random.Range(charPos.z - 1f, charPos.z + 1f));
                transform.position = wolfPos;
            }

            else if (distance >= 6f && distance < 12f)
            {
                ani.SetInteger("WolfAniIndex", 2);
                transform.position = Vector3.MoveTowards(transform.position, CharPos, wolfTocharSpeed * 1.7f * Time.deltaTime);
                direction = CharPos - transform.position;
                direction = new Vector3(direction.x, 0f, direction.z);
            }
            else if (distance < 6f && distance >= 3f)
            {
                ani.SetInteger("WolfAniIndex", 2);
                transform.position = Vector3.MoveTowards(transform.position, CharPos, wolfTocharSpeed * Time.deltaTime);
                direction = CharPos - transform.position;
                direction = new Vector3(direction.x, 0f, direction.z);
            }
            else if (distance < 3f)
            {
                ani.SetInteger("WolfAniIndex", 0);
            }

            //늑대가 이동중이 아니라면 몬스터는 대기 상태를 유지
            if (direction.x == 0 && direction.z == 0)
            {
                ani.SetInteger("WolfAniIndex", 0);
            }
        }
    }

    void wolfTurn()
    {
        if ((direction.x == 0 && direction.z == 0))
            return;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, wolfTurnSpeed * Time.deltaTime);
        transform.rotation = turn;
    }

    void Update()
    {
        wolfTocharSpeed = Character.instance.f_MoveSpeed*0.8f;
        wolfToMonsterSpeed = Character.instance.Run_Speed * 1.2f;

        aniWalkSpeed = 2f + ((Status.instance.agiall - 10) * 0.04f);
        ani.SetFloat("WalkSpeed", aniWalkSpeed);

        wolfTurn();
        AttackMonster();
    }
}
