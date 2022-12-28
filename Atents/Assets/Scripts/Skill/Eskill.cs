using UnityEngine;
using UnityEngine.UI;

public class Eskill : MonoBehaviour
{
    //늑대 오브젝트

    public GameObject wolf;

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

    float ActiveTime = 120f;
    float CoolSpeed = 240f;

    private void Awake()
    {
        wolf.gameObject.SetActive(false);
    }

    void Start()
    {
        ActiveImage.fillAmount = 1f;
        ActiveCount.gameObject.SetActive(false);
    }



    void UseESkill()
    {
        if (Input.GetKeyDown(KeyCode.E) && cool.fillAmount == 0f && isActive != true)
        {
            wolf.gameObject.SetActive(true);
            Vector3 wolfPos;
            Vector3 charPos = Character.instance.transform.position;
            wolfPos = new Vector3(Random.Range(charPos.x - 1f, charPos.x + 1f), 0f, Random.Range(charPos.z - 1f, charPos.z + 1f));
            wolf.transform.position = wolfPos;
            ActiveSet();
            isActive = true;
        }
    }

    void ActiveSet()
    {
        ActiveCount.gameObject.SetActive(true);
        ActiveImage.fillAmount = 0f;
        ActiveCounting = 120.99f;
    }

    void SKillActive()
    {
        if (isActive == true)
        {
            Status.instance.BurfAgi = (int)(Status.instance.Agi * 0.2f);
            ActiveSkill.fillAmount += ActiveOn * Time.deltaTime;

            //시전시간 Setting
            ActiveCount.text = ((int)ActiveCounting).ToString();
            ActiveCounting -= Time.deltaTime;
            ActiveImage.fillAmount += Time.deltaTime / ActiveTime;

            if (ActiveSkill.fillAmount >= 1f)
            {
                ActiveOn *= -1f;
                ActiveSkill.fillClockwise = false;
            }
            else if (ActiveSkill.fillAmount <= 0f)
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

        if (SkillTime > ActiveTime && isActive == true)
        {
            ActiveCount.gameObject.SetActive(false);
            Status.instance.BurfAgi -= (int)(Status.instance.Agi * 0.2f);
            ActiveSkill.fillAmount = 0f;
            cool.fillAmount = 1f;
            SkillTime = 0f;
            Eskill_wolf.instance.ani.SetInteger("WolfAniIndex", 10);
            Eskill_wolf.instance.die = true;
            isActive = false;
        }



        Cool();
        SKillActive();
        UseESkill();
    }
}
