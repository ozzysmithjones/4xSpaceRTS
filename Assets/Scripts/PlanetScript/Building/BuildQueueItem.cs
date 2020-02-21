using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuildQueueItem : ScriptableObject {

    public int classIndex = 0;
    public string name = "mine";
    public string description = "gets resources";


    public float buildTime = 10f;
    public int buildCost = 10;
    public enum Category { Economy,Diplomacy,Military};
    public Category category = Category.Economy;

    public virtual void Build (Planet planet)
    {

    }

    public BuildQueueItem (string name, string description) 
    {
        this.name = name;
        this.description = description;
    }

    
}
