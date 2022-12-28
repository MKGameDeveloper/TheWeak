using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public string MapArea;

    public List<Monster> monsterinmaplist = new List<Monster>();

    private void Awake()
    {
        
    }

    void Start()
    {
      
    }
    
    public void MonsterOn()
    {
        for(int i = 0; i<monsterinmaplist.Count; i++)
        {
            monsterinmaplist[i].gameObject.SetActive(true);
        }
    }

    void Update()
    {
        
    }
}
