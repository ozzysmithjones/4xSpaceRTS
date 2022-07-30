using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Military
{
    public List<Fleet> fleets = new List<Fleet>();
    [System.NonSerialized] public List<Empire> enemies = new List<Empire>();
}
