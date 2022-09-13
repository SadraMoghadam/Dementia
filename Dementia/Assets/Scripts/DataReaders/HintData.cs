using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintData
{
    public int Id;
    public string Hint;

    public HintData(int id, string hint)
    {
        Id = id;
        Hint = hint;
    }

    public override string ToString()
    {
        return "name: " + Hint;
    }
}
