using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;

    List<Sprite> InventoryIcon = new List<Sprite>();
    Sprite[] icons;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        icons = Resources.LoadAll<Sprite>("ItemInventory/");

        for (int i = 0; i < icons.Length; i++)
        {
            InventoryIcon.Add(icons[i]);
        }
    }

    public int RandomInteger(int MaxNum)
    {
        int num = Random.Range(0, MaxNum);

        return num;
    }

    public void ImageAlphaChange(Image c, float f)
    {
        Color alpha;
        alpha = c.color;
        alpha.a = f;
        c.color = alpha;
    }
    public void TextAlphaChange(Text c, float f)
    {
        Color alpha;
        alpha = c.color;
        alpha.a = f;
        c.color = alpha;
    }

    public void ImageAlphaBlending_Plus(Image c)
    {
        Color alpha;

        alpha = c.color;

        alpha.a += Time.deltaTime;

        c.color = alpha;
    }

    public void TextAlphaBlending_Plus(Text c)
    {
        Color alpha;

        alpha = c.color;

        alpha.a += Time.deltaTime;

        c.color = alpha;
    }

    public void ImageAlphaBlending_Minus(Image c)
    {
        Color alpha;

        alpha = c.color;

        alpha.a -= Time.deltaTime;

        c.color = alpha;
    }

    public void TextAlphaBlending_Minus(Text c)
    {
        Color alpha;

        alpha = c.color;

        alpha.a -= Time.deltaTime;

        c.color = alpha;
    }

    void Start()
    {
        
    }
    
    public Sprite GetIcon(string SpriteName)
    {
        Sprite icon = InventoryIcon.Find(o => (o.name.Equals(SpriteName)));
        return icon;
    }


    void Update()
    {
        
    }
}
