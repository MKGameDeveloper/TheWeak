using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    public GameObject ball;

    public enum STATE
    {
        IDLE = 0,
        WALK,
        RUN,
        ATTACK,
        DIE,
        SKILL
    };
    public STATE state;

    public static Character instance = null;

    //미니맵////////////////////////////////////

    public Vector3 vStart = Vector3.zero;
    public Vector2 vMapSize = new Vector2(200f, 200f);

    //게이지 변수 선언//////////////////////////

    public Image stamina;
    public Text staminaCount;

    public Image Hp;
    public Text HpCount;

    public Image Exp;
    public Text ExpCount;

    public Text Level;

    public float hp;

    //몬스터 공격////////////////////////////

    public Monster Target;

    public BossMonster BossAttack;


    /// ////////////////////////////////////


    bool Running = true;
    public bool CanMove = true;

    public bool Die = false;


    //애니메이션
    public Animator ani;

    //이동값 변수
    public float h;
    public float v;

    float StaminaTimeCount = 0f;

    //회전시 방향 벡터
    Vector3 dir;

    //부활시 세이브포인트
    public Vector3 savePos;

    //캐릭터 현재 위치
    public Vector3 curPos;

    public float f_MoveSpeed;
    public float Walk_Speed;
    public float Run_Speed;
    public float f_TurnSpeed = 500f;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        ani = GetComponent<Animator>();


    }

    void Start()
    {
        state = STATE.IDLE;

        savePos = new Vector3(-954.94f, 0.1f, 44.6f);

        hp = Status.instance.curhp;

    }

    void Animator()
    {
        

        if (h == 0 && v == 0)
        {
            ani.SetInteger("iAniIndex", 0);
        }

        if (h != 0 || v != 0)
        {
            ani.SetInteger("iAniIndex", 1);
        }
        if (Input.GetKey(KeyCode.LeftShift) && Running == true && CanMove == true)
        {
            if (h == 0 && v == 0)
                return;
            ani.SetInteger("iAniIndex", 2);
        }
        if (hp <= 0)
        {
            state = STATE.DIE;
            ani.SetInteger("iAniIndex", 10);
        }


    }

    void Turn()
    {
        if (h == 0 && v == 0)
            return;
        if (state == STATE.ATTACK)
            return;

        Quaternion rotation = Quaternion.LookRotation(dir.normalized);

        Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, f_TurnSpeed * Time.deltaTime);
        transform.rotation = turn;
    }

    void Moving()
    {

        dir = new Vector3 (h, 0, v);

        Walk_Speed = 2f + Status.instance.curagi;
        Run_Speed = Walk_Speed * 2f;

        transform.position += dir.normalized * Time.deltaTime * f_MoveSpeed;

        GameObject raycube = gameObject.transform.GetChild(2).gameObject;
        

        RaycastHit hitInfo;
        if (Physics.Raycast(raycube.transform.position, transform.TransformDirection(Vector3.down), out hitInfo, Mathf.Infinity))
        {
            //Debug.Log(hitInfo.collider.name);
            if (hitInfo.collider.tag == "Terrain")
            {
                Vector3 position = transform.position;
                position.y = hitInfo.point.y;
                transform.position = position;
            }
        }
    }

    void Run()
    {

        if (Die == true)
            return;




        if (Input.GetKey(KeyCode.LeftShift) && Running == true)
        {
            if (h == 0 && v == 0)
                return;
            f_MoveSpeed = Run_Speed;
            stamina.fillAmount -= 0.2f * Time.deltaTime;
        }

        else
        {
            f_MoveSpeed = Walk_Speed;
            stamina.fillAmount += 0.5f * Time.deltaTime;
        }

        if (stamina.fillAmount == 0f)
        {
            Running = false;

        }
        if (Running == false)
        {
            StaminaTimeCount += 1 * Time.deltaTime;
            if (StaminaTimeCount >= 1f)
            {
                Running = true;
                StaminaTimeCount = 0f;
            }
                
        }
        
        
        //staminaCount.text = (int)(stamina.fillAmount * 100) + "/100";



    }

    void CharacterState()
    {
        //Hp 관리상태

        Hp.fillAmount = (hp / (float)Status.instance.curhp);
        //HpCount.text = (int)hp + "/" + Status.instance.curhp;
        if (hp >= Status.instance.curhp)
        {
            hp = Status.instance.curhp;
        }

        //그외

        Exp.fillAmount = (float)Status.instance.Exp / (float)Status.instance.MaxExp;
        int curExp = (int)(Exp.fillAmount * 100);
        //ExpCount.text = curExp.ToString() + "%";

        Level.text = Status.instance.Level.ToString();


    }

    void Damaged()
    {
        if (Die == false && hp <= 0)
        {
            hp = 0;
            Die = true;
            h = 0;
            v = 0;
            Status.instance.Exp -= Status.instance.MaxExp / 2;
        }
    }

    void Attack()
    {

        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;



            for (int i = 0; i < MonsterManager.instance.monsterlist.Count; i++)
            {

                if (Physics.Raycast(ray, out hitInfo))
                {
                    Debug.Log(hitInfo.collider.gameObject.name);
                    if (hitInfo.collider.gameObject.name == MonsterManager.instance.monsterlist[i].name)
                    {

                        Target = MonsterManager.instance.monsterlist[i];
                    }

                }

            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            

            h = 0;
            v = 0;

            //화면상의 마우스 위치(Input.mousePosition)로 부터 
            //카메라 기준으로 게임공간상으로 향하는 광선(Ray) 생성.
            //Camera.main은 메인 카메라를 의미.
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                //if (Target == null)
                //{
                //    Vector3 direction = hitInfo.point - transform.position;
                //    Quaternion rotation = Quaternion.LookRotation(direction);
                //    rotation.x = 0f;
                //    rotation.z = 0f;
                //    Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, 50000f * Time.deltaTime);
                //    Character.instance.transform.rotation = turn;

                //    ani.SetInteger("iAniIndex", 4);
                //}
                if (Target != null)
                {
                    state = STATE.ATTACK;
                    Vector3 direction = Target.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, 50000f * Time.deltaTime);
                    Character.instance.transform.rotation = turn;

                    ani.SetInteger("iAniIndex", 4);
                }
                if(BossAttack != null)
                {
                    state = STATE.ATTACK;
                    Vector3 direction = BossAttack.transform.position - transform.position;
                    Quaternion rotation = Quaternion.LookRotation(direction);
                    Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, 50000f * Time.deltaTime);
                    Character.instance.transform.rotation = turn;

                    ani.SetInteger("iAniIndex", 4);
                }
                
                
            }
            
        }
    }
    



    public void Attack_enemy()
    {
        if (Target != null)
        {
            if (Vector3.Distance(transform.position, Target.transform.position) < 3f)
            {
                float realAtk = Status.instance.curatk - Target.defense;
                if(realAtk <= 0)
                {
                    realAtk = 1;
                }
                Target.Hp -= realAtk;
            }
        }
        if (BossAttack != null)
        {
            if (Vector3.Distance(transform.position, BossAttack.transform.position) < 5f)
            {
                float realAtk = Status.instance.curatk - BossAttack.Def;
                if (realAtk <= 0)
                {
                    realAtk = 1;
                }
                BossAttack.Hp -= realAtk;
            }
        }
    }
    
    public void Attack_end()
    {
        state = STATE.IDLE;
    }

    void Skill()
    {

    }

    void Update()
    {

        curPos = transform.position;

        if (Die == false && CanMove == true)
        {
            if (state == STATE.ATTACK)
                return;
            state = STATE.WALK;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        ball.transform.position = new Vector3(transform.position.x, 35f, transform.position.z);

        Animator();
        Moving();
        Turn();
        Run();
        CharacterState();
        Damaged();

        Attack();

        
    }
}
