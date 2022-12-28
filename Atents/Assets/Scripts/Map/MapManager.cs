using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public static MapManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public List<Map> maplist = new List<Map>();

    

    void Start()
    {
        maplist[0].MapArea = "East";
        maplist[1].MapArea = "North";
        maplist[2].MapArea = "BossArea";

        for(int i= 0; i<maplist.Count; i++)
        {
            maplist[i].gameObject.SetActive(true);
        }

        StartCoroutine(mim());
        
        
    }

    IEnumerator mim()
    {
        yield return new WaitForSeconds(0.3f);
        MonsterinMap();
    }

    public void MonsterinMap()
    {
        for (int i = 0; i < maplist.Count; i++)
        {
            for (int j = 0; j < MonsterManager.instance.monsterlist.Count; j++)
            {
                if (MonsterManager.instance.monsterlist[j].Area.MapArea.Equals(maplist[i].MapArea))
                {
                    maplist[i].monsterinmaplist.Add(MonsterManager.instance.monsterlist[j]);
                }
            }
        }

    }

    public void MapOff()
    {
        for (int i = 0; i < /*maplist.Count*/2; i++)
        {
            for (int j = 0; j < maplist[i].monsterinmaplist.Count; j++)
            {
                maplist[i].monsterinmaplist[j].gameObject.SetActive(false);
            }
            maplist[i].gameObject.SetActive(false);
        }
    }
    
    void Update()
    {
        if(maplist[1].gameObject.activeSelf == false)
        {
            NPCManager.instance.npclist[7].gameObject.SetActive(false);
        }
        else
        {
            NPCManager.instance.npclist[7].gameObject.SetActive(true);
        }
    }
}
