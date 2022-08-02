using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Empire
{
    public static Empire player = null;

    //cosmetic
    public string name;
    public Color flagColor;

    public Territory territory;
    public Economy economy;
    public Research research;
    public Military military;

    public Empire(bool isPlayer, Color flagColor, string factionName)
    {
        this.name = factionName;
        this.flagColor = flagColor;
       
        territory = new Territory();
        economy = isPlayer ? new PlayerEconomy() : new Economy();
        research = new Research(this);
        military = new Military(this);

        territory.Init(this);
        economy.Init(territory);
    }


    public bool IsEnemyTo(Empire empire)
    {
        return empire != this; //&& enemies.Contains(empire);
    }

    public virtual void Start()
    {
        
    }

    public virtual void Update(float deltaTime)
    {
        research.Update();
    }
   
}
