using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Animator ani;

    public float a;

    //몬스터 논리변수
    public bool attack = false;
    public bool Damage = false;
    public bool alive = true;

    //오브젝트 풀링
    public bool reSpawnStart = false;
    public float reSpawnTime;

    public Vector3 curPos ;
    protected Vector3 direction;

    protected Vector3 charcur;
    protected Vector3 chardirection;

    //몬스터 스텟
    public int Index;
    public float monsterSpeed;
    public float MonsterExp;
    public float Atk;
    public float defense;
    public float MaxHp;
    public float Hp;
    public float curhp;

    protected float FindPlayerRange = 7f;
    protected float monsterTurnSpeed = 500f;

    //몬스터가 드롭할 아이템
    public string drop0;
    public string drop1;
    public string drop2;
    public string drop3;
    public string drop4;
    public string drop5;

    //몬스터가 있는 지형
    public Map Area;

    //캐릭터 논리변수
    protected bool charStop = false;
    protected bool backhome = false;


    protected float moveCount = 0;

    protected float distance;


    protected GameObject Hpbar;
    public GameObject hp;
    protected Image hpimage;
    protected float OffSetY;

    void Awake()
    {
        ani = GetComponent<Animator>();

        MonsterHp();
    }

    void Start()
    {
        curPos = transform.position;
        
    }

    protected void MonsterHp()
    {
        Hpbar = Resources.Load<GameObject>("MonsterHpbar") as GameObject;
        hp = Instantiate(Hpbar);
        hp.transform.SetParent(CanvasScript.instance.transform.GetChild(0).transform.GetChild(0).transform);

        GameObject hpchild = hp.transform.GetChild(0).gameObject;
        hpchild = hpchild.transform.GetChild(0).gameObject;
        hpimage = hpchild.GetComponent<Image>();
    }


    protected void ToPlayer()
    {
        if (alive == false || Damage == true)
            return;
        //distance = 캐릭터의 위치와 현재 몬스터의 위치 사이의 거리
        distance = Vector3.Distance(Character.instance.curPos, transform.position);

        //몬스터 첫 소환 위치와 현재 몬스터 위치 사이의 거리가 15 이상이 되면 제자리로 돌아가는 backhome이 활성화
        if (Vector3.Distance(transform.position, curPos) >= 15f && backhome == false)
        {
            backhome = true;   
        }

        //backhome이 true일때 몬스터는 첫 소환위치로 돌아가가고 backhome은 비활성화
        if(backhome == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, curPos, monsterSpeed * Time.deltaTime);
            direction = curPos - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
            if (transform.position == curPos)
                backhome = false;
        }

        //만약 캐릭터가 죽었을시 Character의 Die가 활성화 되고 몬스터는 첫 소환위치로 돌아감.
        if (Character.instance.Die == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, curPos, monsterSpeed * Time.deltaTime);
            direction = curPos - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
            ani.SetInteger("MonsterAniIndex", 1);
        }

        //만약 backhome이 활성화 되있다면 return
        else if (backhome == true)
            return;

        //만약 캐릭터의 위치와 현재 몬스터 위치의 거리가 1이상 15이하일시 몬스터가 캐릭터를 추격
        else if (1f < distance && distance < FindPlayerRange && attack == false)
        {
            ani.SetInteger("MonsterAniIndex", 1);
            transform.position = Vector3.MoveTowards(transform.position, Character.instance.curPos, monsterSpeed * Time.deltaTime);
            direction = Character.instance.curPos - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
        }

        //캐릭터와 몬스터 사이의 거리가 1이하라면 몬스터가 캐릭터를 공격함
        else if (distance < 1f)
        {
            ani.SetInteger("MonsterAniIndex", 2);
            direction = Character.instance.curPos - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
        }

        //캐릭터와 몬스터 사이의 거리가 15이상이라면 몬스터는 제자리로 돌아감
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, curPos, monsterSpeed * Time.deltaTime);
            direction = curPos - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);
        }

        //몬스터가 이동중이 아니라면 몬스터는 대기 상태를 유지
        if (direction.x == 0 && direction.z == 0)
        {
            ani.SetInteger("MonsterAniIndex", 0);
        }

    }

    protected void monsterTurn()
    {
        if ((direction.x == 0 && direction.z == 0) || alive == false)
            return;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, monsterTurnSpeed * Time.deltaTime);
        transform.rotation = turn;
    }

    protected void Attack()
    {
        Character.instance.hp -= (1 * Atk - (int)Status.instance.curdef);
        Debug.Log((1 * Atk - (int)Status.instance.curdef));

        attack = true;
        charStop = true;
    }

    protected void CharacterAnimator()
    {
        if (attack == true)
        {
            //Character.instance.ani.SetInteger("iAniIndex", 3);
            Character.instance.ani.Play("Damaged", 0, 0.1f);
            //charcur = Character.instance.transform.position;
            //Character.instance.transform.position = Vector3.MoveTowards(charcur, (charcur + direction.normalized), 8f * Time.deltaTime);

            chardirection = transform.position - charcur;
            Quaternion ch_rotation = Quaternion.LookRotation(chardirection);
            Quaternion ch_turn = Quaternion.RotateTowards(Character.instance.transform.rotation, ch_rotation, 50000f * Time.deltaTime);
            Character.instance.transform.rotation = ch_turn;

            attack = false;
            
        }
    }

    protected void CharacterStop()
    {
        if (charStop == false)
            return;
        Character.instance.CanMove = false;
        Character.instance.v = 0;
        Character.instance.h = 0;

        
        moveCount += 1f * Time.deltaTime;
        if (moveCount >= 0.2f)
        {
            moveCount = 0;
            Character.instance.CanMove = true;
            charStop = false;
        }
        
    }



    protected void Damaged()
    {
        if (curhp != Hp)
        {
            if (alive == false)
                return;

            Damage = true;
            ani.Play("Damaged", 0, 0.1f);
            curhp = Hp;

            
        }
    }

    public void NotMove()
    {
        Damage = false;
    }

    protected void MonsterHpPosition()
    {

        hp.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        hp.transform.position += new Vector3(0, OffSetY, 0);

        hpimage.fillAmount = (float)Hp / MaxHp;


    }

    public void MonsterDie()
    {
        if(Hp <= 0f)
        {
            ani.SetInteger("MonsterAniIndex", 10);
            alive = false;
            
        }
    }

    public void GetExp()
    {
        if(QuestManager.instance.Quest.Type == 1 && QuestManager.instance.Quest.Require == Index && QuestManager.instance.Accept == true)
        {
            QuestManager.instance.Count += 1;
        }
        if(Index == 2)
        {
            Boss.instance.Lich_Count -= 1;
        }
        Status.instance.Exp += MonsterExp;
        Vector3 diePos = transform.position;
        ItemManager.instance.MakeItem(drop0, diePos);
        ItemManager.instance.DropItem(drop1, drop2, drop3, drop4, drop5, diePos);
        ItemManager.instance.DropItem(drop1, drop2, drop3, drop4, drop5, diePos);
        ItemManager.instance.DropItem(drop1, drop2, drop3, drop4, drop5, diePos);
    }

    public void Die()
    {
        
        Hp = MaxHp;
        curhp = MaxHp;
        gameObject.SetActive(false);
        hp.gameObject.SetActive(false);
        
    }

    protected void Monster_Map()
    {
        if(Area.gameObject.activeSelf == false)
        {
            gameObject.SetActive(false);
        }
        else if(Area.gameObject.activeSelf == true)
        {
            gameObject.SetActive(true);
        }
    }

    void Update()
    {
        MonsterDie();
        MonsterHpPosition();
        Damaged();
        ToPlayer();
        monsterTurn();
        CharacterAnimator();
        CharacterStop();

        //Debug.Log(Vector3.Distance(Player.position, transform.position));
    }
}
