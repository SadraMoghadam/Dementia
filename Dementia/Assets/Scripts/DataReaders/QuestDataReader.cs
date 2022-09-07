using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDataReader : MonoBehaviour
{
    private string fileName = "Data/QuestData";
    public List<QuestData> questsData;
    
    private void Awake()
    {
        questsData = ReadQuestsData(fileName);
    }

    private List<QuestData> ReadQuestsData(string filename)
    {
        int currentLineNumber = 0;
        int columnCount = 0;
        TextAsset txt = Resources.Load<TextAsset>(filename);
        string[] lines = txt.text.Split('\n');
        questsData = new List<QuestData>();
        foreach (string line in lines)
        {
            if (line == "")
            {
                continue;
            }

            string[] lineSplitted = line.Split(',');
            currentLineNumber++;

            if (currentLineNumber == 1)
            {
                continue;
            }

            int id = int.Parse(lineSplitted[0]);
            string quest = lineSplitted[2];

            QuestData data = new QuestData(id, quest);
            questsData.Add(data);
        }

        return questsData;
    }

    public QuestData GetQuestData(int id)
    {
        return questsData[id];
    }
    
}
