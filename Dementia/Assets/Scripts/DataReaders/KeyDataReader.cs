using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDataReader : MonoBehaviour
{
    private string fileName = "Data/KeyData";
    public List<KeyData> keysData;
    
    private void Awake()
    {
        keysData = ReadKeysData(fileName);
    }

    private List<KeyData> ReadKeysData(string filename)
    {
        int currentLineNumber = 0;
        int columnCount = 0;
        TextAsset txt = Resources.Load<TextAsset>(filename);
        string[] lines = txt.text.Split('\n');
        keysData = new List<KeyData>();
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
            string name = lineSplitted[1];
            string description = lineSplitted[3];

            KeyData data = new KeyData(id, name, description);
            keysData.Add(data);
        }

        return keysData;
    }

    public KeyData GetKeyData(int id)
    {
        return keysData[id];
    }
    
}
