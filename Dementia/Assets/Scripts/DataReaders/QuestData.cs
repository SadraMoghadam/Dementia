using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestData
{
    public int Id;
    public string Quest;

    public QuestData(int id, string quest)
    {
        Id = id;
        Quest = quest;
    }

    public override string ToString()
    {
        return "name: " + Quest;
    }
}
