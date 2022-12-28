using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour
{

    public static PortalManager instance = null;

    public PortalChange TownportalNpc;

    public GameObject eventcamera;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    public List<Portal> portallist = new List<Portal>();

    public float TelCount = 0f;
    float MaxCount = 3f;

    void Start()
    {
        portallist[0].Teleport = portallist[1].transform.position;
        portallist[1].Teleport = portallist[0].transform.position;
        portallist[4].Teleport = portallist[0].transform.position;
        portallist[5].Teleport = portallist[6].transform.position;
        portallist[6].Teleport = portallist[0].transform.position;

        StartCoroutine(portalon());

    }

    IEnumerator portalon()
    {
        yield return new WaitForSeconds(0.5f);
        Portal_east();
    }

    public void Portal_east()
    {

        MapManager.instance.MapOff();
        MapManager.instance.maplist[0].gameObject.SetActive(true);
        MapManager.instance.maplist[0].MonsterOn();
        portallist[0].Teleport = portallist[1].transform.position;
        TownportalNpc.gameObject.SetActive(false);
    }
    public void Portal_west()
    {
        portallist[0].Teleport = portallist[2].transform.position;
        TownportalNpc.gameObject.SetActive(false);
    }
    public void Portal_south()
    {
        portallist[0].Teleport = portallist[3].transform.position;
        TownportalNpc.gameObject.SetActive(false);
    }
    public void Portal_north()
    {
        MapManager.instance.MapOff();
        MapManager.instance.maplist[1].gameObject.SetActive(true);
        MapManager.instance.maplist[1].MonsterOn();
        portallist[0].Teleport = portallist[4].transform.position;
        TownportalNpc.gameObject.SetActive(false);
    }


    public void TeleportCharacter()
    {
        PortalTimeControl.instance.time = TelCount;
        PortalTimeControl.instance.MaxCount = MaxCount;

        Portal portal = portallist.Find(o => (o.distance <= 2.1f) && o.gameObject.activeSelf == true && o.distance != 0f);

        if(portal != null)
        {
            PortalTimeControl.instance.gameObject.SetActive(true);
            TelCount += Time.deltaTime;

            if (TelCount > MaxCount)
            {
                if(portal.name.Equals("North to Boss"))
                {
                    eventcamera.gameObject.SetActive(true);
                }
                Character.instance.transform.position = portal.Teleport;
                TelCount = 0f;
            }

        }

        else if(portal == null)
        {
            PortalTimeControl.instance.gameObject.SetActive(false);
            TelCount = 0f;
        }

        
    }

    void Update()
    {
        TeleportCharacter();
    }
}
