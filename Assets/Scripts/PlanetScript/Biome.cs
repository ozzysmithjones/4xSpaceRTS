
using UnityEngine;

public class Biome
{
    //planets produce differing amounts of resources. 

    // (cloud worlds, gas worlds)
    public float energy = 0.0f;
    //energy: it costs energy to fight and warp with fleets. This resource hopefully stops the "deathball" problem
    // beacuse its a huge waste of energy to make a larger fleet battle a smaller fleet. Energy cannot be stored, to reduce the steam rolling effect happening in the late game.
    //(rocky worlds, deserts).
    public float materials = 0.0f;
    //Material:Material is used to expand and build military fleets.
    //The option of improving production vs military power is a key strategy in lots of 4x games.
    //(hot worlds, strange planets).
    public float deathMatter = 0.0f;
    //Death Matter: a very overpowered resource that empires will fight over, inspired from blood diamonds. 
    //Death matter gives a goal to the player with fun consequences. 
    // (goldilocks worlds, jungle worlds)
    public float planetSupplies = 0.0f;
    //Planet supplies is a resource that the player must be carefull to only produce the minimum required. Planet supplies is the upkeep to the players planets in the game.
    //Not having enough planetary supplies creates a long term piracy and rebellion problem, sometimes permanent damage is caused.

}
