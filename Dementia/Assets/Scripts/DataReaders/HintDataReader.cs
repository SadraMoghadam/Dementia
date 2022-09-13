using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintDataReader : MonoBehaviour
{
    private string fileName = "Data/HintData";
    public List<HintData> hintsData;
    
    private void Awake()
    {
        hintsData = ReadHintsData(fileName);
    }

    private List<HintData> ReadHintsData(string filename)
    {
        int currentLineNumber = 0;
        int columnCount = 0;
        TextAsset txt = Resources.Load<TextAsset>(filename);
        string[] lines = txt.text.Split('\n');
        hintsData = new List<HintData>();
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
            string hint = lineSplitted[2];

            HintData data = new HintData(id, hint);
            hintsData.Add(data);
        }

        return hintsData;
    }

    public HintData GetHintData(int id)
    {
        return hintsData[id];
    }
    
}
