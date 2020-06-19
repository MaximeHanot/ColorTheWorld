using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Collectable
{
    public string name;

    public int collectableNumber;
    public bool isCollected;

    [TextArea(3, 10)] // Min and Max amount of lines.
    public string[] sentences; // recuperer le texte de fichier text ?
}
