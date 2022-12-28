using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetItem : MonoBehaviour
{

    float Count = 0f;

    Color b;
    Color i;
    Color n;

    void Start()
    {
        
    }

    private void OnEnable()
    {
        Count = 0f;
        alphaControll(1f, 1f, 1f);
    }

    void alphaControll(float _b, float _i, float _n)
    {
        GameObject obj = gameObject.transform.GetChild(0).gameObject;
        Image back = obj.GetComponent<Image>();
        obj = gameObject.transform.GetChild(1).gameObject;
        Image item = obj.GetComponent<Image>();
        obj = gameObject.transform.GetChild(2).gameObject;
        Text name = obj.GetComponent<Text>();

        b = back.color;
        i = item.color;
        n = name.color;

        b.a = _b;
        i.a = _i;
        n.a = _n;

        back.color = b;
        item.color = i;
        name.color = n;
    }

    void alphaBlending()
    {
        GameObject obj = gameObject.transform.GetChild(0).gameObject;
        Image back = obj.GetComponent<Image>();
        obj = gameObject.transform.GetChild(1).gameObject;
        Image item = obj.GetComponent<Image>();
        obj = gameObject.transform.GetChild(2).gameObject;
        Text name = obj.GetComponent<Text>();

        b = back.color;
        i = item.color;
        n = name.color;

        b.a -= Time.deltaTime;
        i.a -= Time.deltaTime;
        n.a -= Time.deltaTime;

        back.color = b;
        item.color = i;
        name.color = n;

        if (b.a <= 0 && i.a <= 0 && n.a <= 0) 
        {
            gameObject.SetActive(false);
        }

    }

    void Update()
    {
        Count += Time.deltaTime;

        if(Count > 4f)
        {
            alphaBlending();
        }
    }
}
