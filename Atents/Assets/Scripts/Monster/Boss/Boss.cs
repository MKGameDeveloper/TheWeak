using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public static Boss instance = null;

    public Portal Totown;

    public GameObject DieCamera;

    public BossMonster point;

    public Transform BossArea;

    public GameObject Dragon;
    Renderer Material;

    public GameObject PointLight;

    //보스 애니메이션 "BossAniIndex"
    Animator ani;

    //보스 Y 위치값 -6.7f;

    //브레스 이펙트
    public GameObject flame;

    //보스 HP바
    public GameObject HpBar;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        ani = GetComponent<Animator>();

        Material = Dragon.GetComponent<Renderer>();
    }

    //보스지역 입장시 이벤트 등장 유무
    //public bool isEvent = false;
    public int Event = 0;

    //보스 페이즈
    int Phase = 1;

    //보스 상태
    public enum MODE
    {
        //Mode가 NONE 또는 TRACE 상태일때 공격 타겟팅이 잡힘.
        NONE = 0,

        //Mode가 NONE 또는 TRACE 상태일때 공격 타겟팅이 잡힘.
        //Mode가 Trace 상태일때 플레이어를 추격.
        TRACE,

        //Mode가 ATTACK 이면 이동멈춤,바라봄 멈춤.
        ATTACK,

        //Mode가 SKELETON 상태일때 성벽으로 이동하는 이벤트 발생.
        //SKELETON을 잡을동안 천천히 HP 회복.
        SKELETON,

        //Mode가 METEOR 상태일때 Defense 모션을 취하고 메테오 스킬 사용.
        METEOR,

        //Mode가 DOWN 상태일때 움직이지 못하고, 보스의 머리가 타격포인트로 설정된다.
        DOWN
    };

    public MODE Mode = MODE.NONE;

    public enum STATE
    {
        IDLE = 0,    //NONE
        WALK,        //TRACE
        RUN,         //TRACE  
        BITE,        //ATTACK
        CLAW,        //ATTACK
        FLAME,       //SKELETON
        END_SUMMON,  //
        DEFENSE,     //METEOR
        DAMAGE,      //
        DIE,         //
        DOWN         //

    };

    public int state = 0;

    //행동간 딜레이
    float ActDelay = 3f;
    float delayMax = 3f;

    //행동중인지를 체크
    int ActOn = 0;
    int RandomAct = 0;

    //드래곤이 이동할 위치
    public Vector3 Goal;

    //딜레이 시간동안 무작위로 이동할 위치
    public Vector3 RandomPos;

    //리치 남은 수 Count
    public int Lich_Count = 0;
    int Lich_Max_Count = 5;
    //소환상태 체크
    bool LichSum = false;
    //보스 다운 타임 
    float DownTime = 0f;

    //소환되는 리치를 관리할 리스트
    List<Lich> lichsummonlist = new List<Lich>();


    //해골병사 남은 수 Count
    public int Skeleton_Count = 0;
    int Skeleton_Max_Count = 5;
    //소환상태 체크
    bool SkeletonSum = false;

    //소환되는 스켈레톤을 관리할 리스트
    List<Skeleton> skeletonsummonlist = new List<Skeleton>();

    //메테오 상태 시간 Count
    //메테오 MaxTime = 10f;
    float MeteorTime = 0f;
    float MaxMeteorTime = 10f;

    //드래곤 시야 방향
    public Vector3 direction;

    //보스 스테이터스
    float DragonSpeed;
    float DragonTurnSpeed = 500f;

    //보스 공격 위치
    //캐릭터가 가까이 왔을시 공격을 한다. 캐릭터가 벗어나도 인식한 공격 장소를 공격함.
    public Vector3 AttackPos;

    //캐릭터와의 거리를 계산하기 위한 float 변수
    public float distance;


    //브레스 애니메이션 진행시 브레스 이펙트 OnOff 기능
    public void FlameStart()
    {
        flame.gameObject.SetActive(true);
    }
    public void FlameEnd()
    {
        flame.gameObject.SetActive(false);
    }

    //보스 이벤트 등장시 사용되는 Fly함수
    public void Fly()
    {
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath("BossEvent"),
            "easeType", "linear",
            "time", 6f,
            "movetopath", true));
    }

    //구현할 내용

    //페이즈 공통
    //공격시 Move 멈춤, 공격시 Turn 멈춤
    //렛맨들을 소환, 모두 잡을시 드래곤 Knock Down, 머리공격 가능

    //1페이즈 : 이동시 Run 애니메이션 사용, 속도 빠름, 손톱공격,물어뜯기 공격만 사용할 예정

    //2페이즈 : 이동시 Walk 애니메이션 사용, 속도 느림, 손톱공격, 물어뜯기 공격 사용하고 브레스 사용할 예정, 브레스 사용 후 성으로 올라간뒤 Idle2 상태 & 해골병사 소환, 
    //해골병사가 모두 죽으면 다시 내려온다.

    //3페이즈 : 이동시 Walk 애니메이션 사용, 속도 느림, 손톱공격, 물어뜯기 공격 사용하고 브레스 사용할 예정, 브레스 사용 후 Defense 상태 & 메테오 시전
    //10초간 버티면 Defense를 풀고 다시 공격 시작.

    //데미지 주는법 : 드래곤의 4개의 발을 몬스터로 인식하여 4개의 발 Hp를 깎으면 드래곤의 Hp Down 
    //4개의 발 모두 Hp 소진시 드래곤이 넘어지고, 머리를 몬스터로 인식. 데미지를 줄 수 있다. 단 2페이즈와 3페이즈 때는 발 1개가 HP를 모두 소모할때마다
    //각각 해골병사 소환, 메테오 시전 상태로 돌입한다.


    void Start()
    {
        RandomPos = NonePos();

        Mode_Attack();
        
    }


    public void EndEvent()
    {

        Mode_None();

        SummonManager();

        NoticeChange(0);

        StartCoroutine("NoticeManager");

    }

    public void NoticeChange(int Num)
    {
        BossSkillNotice.instance.NoticeNum = Num;
    }

    public IEnumerator NoticeManager()
    {
        BossSkillNotice.instance.noticeActive = true;
        yield return new WaitForSeconds(10f);
        while (BossSkillNotice.instance.Notice.color.a > 0) 
        {
            ResourceManager.instance.TextAlphaBlending_Minus(BossSkillNotice.instance.Notice); 
        }
        BossSkillNotice.instance.noticeActive = false;
    }

    

    //public void PhaseChange()
    //{
    //    //1페이즈
    //    if (hpimage.fillAmount >= 0.5)
    //    {
    //        Phase = 1;
    //    }
    //    //2페이즈
    //    else if (hpimage.fillAmount < 0.5 && hpimage.fillAmount >= 0.2)
    //    {
    //        Phase = 2;
    //    }
    //    //3페이즈
    //    else if (hpimage.fillAmount < 0.2)
    //    {
    //        Phase = 3;
    //    }

    //} 

    public void BossAi()
    {
        if (Mode == MODE.DOWN)
            return;

        //Phase 공용 코드
        distance = Vector3.Distance(Character.instance.transform.position, transform.position);
        
        ani.SetInteger("BossAniIndex", state);

        

        //행동마다 딜레이 3초
        if (ActDelay >= delayMax)
        {
            ActOn = 0;

            if(Mode != MODE.ATTACK)
            {
                AttackPos = Character.instance.transform.position;
            }
            
            //바라볼 방향과 이동위치
            direction = Goal - transform.position;
            direction = new Vector3(direction.x, 0f, direction.z);

            //거리가 15이상일시 캐릭터를 타겟으로 잡는다.
            if (distance >= 15)
            {
                Goal = Character.instance.transform.position;
            }
            //거리가 15미만이고, 공격모드가 아닐시 그 당시의 캐릭터 위치를 타겟으로 잡는다.
            else if (distance < 15 && Mode != MODE.ATTACK)
            {
                Goal = Character.instance.transform.position;
            }
            ///////////////////////////////////////////////////////////
            

            if (Phase == 1)
            {
               

                //1페이즈때 드래곤 속도는 6f이고 애니메이션은 "Run"을 재생한다.

                //거리가 35 이상일시 idle
                if (distance >= 35f || DownTime >= 10f)
                {
                    Mode_None();
                    Idle();
                }
                //거리가 15 이상 35 미만일시 캐릭터를 추격
                else if (15f <= distance && distance < 35f && Mode != MODE.ATTACK)
                {
                    Mode_Trace();
                    DragonMove();
                    DragonTurn();
                }
                //거리가 15 미만이면 캐릭터를 공격
                else if(distance < 15f)
                {
                    int num = Random.Range(1, 3);
                    //각각 50% 확률로 손톱 또는 물기 공격
                    //공격 시도를 할때마다 딜레이 0초로 초기화

                    Goal = AttackPos;
                    DragonTurn();
                    
                    if (num == 1)
                    {
                        Mode_Attack();
                        Bite();
                    }
                    else if (num == 2)
                    {
                        Mode_Attack();
                        Claw();
                    }
                }

                
            }
            else if (Phase == 2)
            {
                //2페이즈때 드래곤 속도는 4f이고 애니메이션은 "Walk"을 재생한다.

                //거리가 35 이상일시 idle
                if (distance >= 35f)
                {
                    Mode_None();
                    Idle();
                }
                //거리가 15 이상 35 미만일시 캐릭터를 추격
                else if (15f <= distance && distance < 35f && Mode != MODE.ATTACK)
                {
                    Mode_Trace();
                    DragonMove();
                    DragonTurn();
                }
                //거리가 15 미만이면 캐릭터를 공격
                else if (distance < 15f)
                {
                    int num = Random.Range(1, 3);
                    //각각 50% 확률로 손톱 또는 물기 공격
                    //공격 시도를 할때마다 딜레이 0초로 초기화

                    Goal = AttackPos;
                    DragonTurn();

                    if (num == 1)
                    {
                        Mode_Attack();
                        Bite();
                    }
                    else if (num == 2)
                    {
                        Mode_Attack();
                        Claw();
                    }
                }
            }
            else if (Phase == 3)
            {

            }
        }

        //딜레이 3초간 랜덤위치로 이동
        else if (ActDelay < delayMax)
        {
            if (ActOn == 0)
            {
                RandomAct = Random.Range(1, 10);
                ActOn = 1;
            }
            if (RandomAct < 4)
            {

                //바라볼 방향과 이동위치
                direction = RandomPos - transform.position;
                direction = new Vector3(direction.x, 0f, direction.z);

                Goal = RandomPos;
                //////////////////////////////////////////////////////
                ///
                if (Vector3.Distance(transform.position, Goal) > 1f)
                {
                    DragonMove();
                    DragonTurn();
                }
                else if (Vector3.Distance(transform.position, Goal) < 1f)
                {
                    Idle();
                }

            }
            else if (RandomAct >= 4)
            {
                Idle();
            }
        }
    }

    public void Delay0()
    {
        ActDelay = 0f;
    }

    public Vector3 NonePos()
    {
        Vector3 pos;
        pos.x = Random.Range(-1520f, -1470f);
        pos.y = transform.position.y;
        pos.z = Random.Range(130f, 145f);

        return pos;
    }

    public void RandomMove()
    {
        if (ActDelay >= delayMax)
        {
            RandomPos = NonePos();
            if(transform.position == RandomPos)
            {
                state = (int)STATE.IDLE;
            }
        }

        ////행동간 딜레이 시간 2초가 지나면 새롭게 위치를 지정.
        //if ()
        //{
        //    RandomPos.x = Random.Range(-13.3f, 38.9f);
        //    RandomPos.y = transform.position.y;
        //    RandomPos.z = Random.Range(-12.1f, 7.6f);
        //}

    }

    //public void CanMove()
    //{
    //    if (Mode == MODE.NONE || Mode == MODE.TRACE)
    //    {
    //        DragonMove();
    //        DragonTurn();
    //    }
    //}

    public void DragonMove()
    {
        if(Phase == 1)
        {
            DragonSpeed = 5f;
            state = (int)STATE.RUN;
        }
        else if(Phase != 1)
        {
            DragonSpeed =3f;
            state = (int)STATE.WALK;
        }
        transform.position = Vector3.MoveTowards(transform.position, Goal, DragonSpeed * Time.deltaTime);
    }

    public void DragonTurn()
    {
        if ((direction.x == 0 && direction.z == 0))
            return;
        Quaternion rotation = Quaternion.LookRotation(direction);
        Quaternion turn = Quaternion.RotateTowards(transform.rotation, rotation, DragonTurnSpeed * Time.deltaTime);
        transform.rotation = turn;
    }

    //행동시 애니메이션 변경 함수////////////////////////////////////
    public void Idle()
    {
        state = (int)STATE.IDLE;
    }

    public void Bite()
    {
        state = (int)STATE.BITE;
    }

    public void Claw()
    {
        state = (int)STATE.CLAW;
    }

    public void Flame()
    {
        state = (int)STATE.FLAME;
    }

    public void End_Summon()
    {
        state = (int)STATE.END_SUMMON;
    }

    public void Defense()
    {
        state = (int)STATE.DEFENSE;
    }

    public void Damage()
    {
        state = (int)STATE.DAMAGE;
    }

    public void Die()
    {
        state = (int)STATE.DIE;
    }

    public void Down()
    {
        state = (int)STATE.DOWN;
        Mode = MODE.DOWN;
    }
    //////////////////////////////////////////////////////////////


    //모드변형/////////////////////////////////////////////////////
    public void Mode_None()
    {
        Mode = MODE.NONE;
    }
    public void Mode_Trace()
    {
        Mode = MODE.TRACE;
    }
    public void Mode_Attack()
    {
        Mode = MODE.ATTACK;
    }
    public void Mode_Skeleton()
    {
        Mode = MODE.SKELETON;
    }
    public void Mode_Meteor()
    {
        Mode = MODE.METEOR;
    }
    public void Mode_DOWN()
    {
        Mode = MODE.DOWN;
    }
    /////////////////////////////////////////////////////////////

    void LichSummon()
    {
        List<MobStatus> mobData = MonsterManager.instance.mobstatlist;
        
        for (int i = 0; i < mobData.Count; i++)
        {
            if (mobData[i].index == 2)
            {
                while (Lich_Count < Lich_Max_Count)
                {
                    MobStatus Info = mobData[i];
                    Vector3 LichPos;

                    LichPos = NonePos();

                    LichPos.y = 0.1f;

                    float angle = UnityEngine.Random.Range(-180f, 180f);
                    Quaternion rotation = Quaternion.Euler(0f, angle, 0f);

                    GameObject obj = GameObject.Instantiate(MonsterManager.instance.monster_[mobData[i].index], LichPos, rotation);

                    obj.name = "Lich_0" + i.ToString();

                    Lich monster = obj.AddComponent<Lich>();

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
                    monster.Summon = true;


                    monster.transform.SetParent(BossArea);

                    MonsterManager.instance.monsterlist.Add(monster);
                    lichsummonlist.Add(monster);

                    Lich_Count += 1;
                }
            }
        }
        for(int i = 0; i<lichsummonlist.Count; i++)
        {
            if(lichsummonlist[i].Hp <= 0 )
            {
                Lich_Count -= 1;
                //lichsummonlist[i].Summon = false;
            }
        }
    }
    void SummonManager()
    {
        if (LichSum == false)
        {
            LichSummon();
            if (Lich_Count == Lich_Max_Count)
                LichSum = true;
        }
    }

    void BossDown()
    {
        if(LichSum == true && Lich_Count == 0)
        {
            Down();
            ani.SetInteger("BossAniIndex", state);
            DownTime += Time.deltaTime;
            point.gameObject.SetActive(true);
            

            if (DownTime >= 10f && DownTime < 11f)
            {
                
                Idle();
                ani.SetInteger("BossAniIndex", state);
                NoticeChange(1);
                StartCoroutine("NoticeManager");
                
            }
            else if( DownTime >= 11f)
            {
                point.gameObject.SetActive(false);
                Mode_None();
                DownTime = 0f;
                LichSum = false;
            }
        }
    }

    public void BossDie()
    {
        if (state != 10)
            return;
        if (BossMonster.instance.Hp < 1)
        {

            Die();
            Totown.gameObject.SetActive(true);
            if (QuestManager.instance.Quest.Type == 3)
            {
                QuestManager.instance.Count = 1;
            }
            DieCamera.gameObject.SetActive(true);
            ani.SetInteger("BossAniIndex", state);
            NoticeChange(5);
            BossMonster.instance.Hp = 1;
        }
    }

    void diecameraOff()
    {
        DieCamera.gameObject.SetActive(false);
    }

    public void Targeting()
    {
        if (Character.instance.BossAttack == null)
        {

            if (Vector3.Distance(point.gameObject.transform.position, Character.instance.transform.position) < 15f
               && point.gameObject.activeSelf == true)
            {
                Character.instance.BossAttack = point;
            }


        }
        if (Character.instance.BossAttack != null)
        {
            BossMonster.instance.target.gameObject.SetActive(true);

            if (Vector3.Distance(Character.instance.BossAttack.transform.position, Character.instance.transform.position) > 8f || point.gameObject.activeSelf == false)
            {
                Character.instance.BossAttack = null;

                BossMonster.instance.target.gameObject.SetActive(false);
            }
        }
    }

    void TimeControl()
    {
        ActDelay += Time.deltaTime;
    }

    void Update()
    {
        Targeting();
        if (distance < 100f)
        {
            HpBar.gameObject.SetActive(true);
        }
        else if (distance >= 100f)
        {
            HpBar.gameObject.SetActive(false);
        }

        BossDie();
        if (state == 9)
            return;

        TimeControl();
        BossAi();
        RandomMove();
        BossDown();
        

        if (Input.GetKeyDown(KeyCode.M))
        {
            point.gameObject.SetActive(true);
        }

        //Material.material.SetTexture =
        

        //등장이벤트 관리할 코드(나중에 수정예정)
        
        if (Event == 1)
        {
            ani.SetBool("Event", true);
            Event = 0;
        }
        else if (Event == 0)
        {
            ani.SetBool("Event", false);
        }
    }

    
}

