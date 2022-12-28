using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Monster
{
    public bool Summon = false;

    void Awake()
    {
        ani = GetComponent<Animator>();

        MonsterHp();

        OffSetY = 100f;

        Area = MapManager.instance.maplist.Find(o => o.MapArea.Equals("BossArea"));
    }

    void Start()
    {

        curPos = transform.position;
    }

    void Update()
    {

        Monster_Map();
        MonsterDie();
        MonsterHpPosition();
        Damaged();
        ToPlayer();
        monsterTurn();
        CharacterAnimator();
        CharacterStop();
    }
}
