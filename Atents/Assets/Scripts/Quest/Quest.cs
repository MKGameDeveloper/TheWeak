using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    public int select = 0;
    public int Complete = 0;
    public int QuestNum;

    public string QuestContents;
    public string RequireStr;
    public string RequireCount;

    int Counting = 0;

    Button QuestClickButton;

    void Start()
    {
        QuestClickButton = gameObject.GetComponent<Button>();
        QuestClickButton.onClick.AddListener(OnQuestWindowButton);
    }

    public void OnQuestWindowButton()
    {
        QuestManager.instance.QuestNotChoice();
        select = 1;
    }
    

    void Update()
    {
        if(select == 1)
        {
            QuestManager.instance.Contents.text = QuestContents;
            QuestManager.instance.RequireStr.text = RequireStr;
            QuestManager.instance.RequireCount.text = RequireCount;
        }
        if(Complete == 0)
        {
            Counting = QuestManager.instance.Count;
            RequireCount = QuestManager.instance.requirename + " " + Counting + "/" + QuestManager.instance.Quest.Count;
        }
        
        
    }
}
