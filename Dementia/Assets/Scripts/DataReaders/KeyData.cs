using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyData
{
    public int Id;
    public string Name;
    public string Description;

    public KeyData(int id, string name, string description)
    {
        Id = id;
        Name = name;
        Description = description;
    }

    public override string ToString()
    {
        return "name: " + Name;
    }
}
