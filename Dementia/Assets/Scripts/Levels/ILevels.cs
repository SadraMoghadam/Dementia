using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevels
{
    public void Setup();
    public void Process();
    public void EndOfLevel();
}
