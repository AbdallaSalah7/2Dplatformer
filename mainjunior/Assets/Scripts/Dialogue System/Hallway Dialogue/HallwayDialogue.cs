using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HallwayDialogue
{
    public string name;
    [TextArea(3, 15)]
    public string[] sentences;
}
