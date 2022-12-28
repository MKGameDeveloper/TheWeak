using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class NPC : MonoBehaviour
{
    public float distance;

    public int index;

    public string NpcName;
    public string str_1;
    public string str_2;

    float OffSetY = 100f;

    GameObject Namebar;
    GameObject nameobj;
    Text name;

    void Awake()
    {
        Namebar = Resources.Load<GameObject>("NpcNameBar") as GameObject;
        
    }

    void Start()
    {
        NpcNameSet();
    }

    void NpcNameSet()
    {
        nameobj = PrefabUtility.InstantiatePrefab(Namebar) as GameObject;
        nameobj.transform.SetParent(CanvasScript.instance.transform.GetChild(0).transform.GetChild(1).transform);
        name = nameobj.transform.GetChild(0).GetComponent<Text>();

    }

    void NamebarPos()
    {
        name.text = NpcName;
        nameobj.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        nameobj.transform.position += new Vector3(0, OffSetY, 0);
    }

    void Update()
    {
        
        NamebarPos();

        distance = Vector3.Distance(transform.position, Character.instance.transform.position);
    }
}
