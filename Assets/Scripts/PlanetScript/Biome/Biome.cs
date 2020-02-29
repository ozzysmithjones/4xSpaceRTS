using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BiomeType
{
    BARREN,
    DESERT,
    TOXIC,
    CARBON,
    JUNGLE,
    OCEAN,
    ICE,
    ODD
}

[CreateAssetMenu(fileName = "Basic Biome", menuName = "Biomes/Basic Biome")]
public class Biome : ScriptableObject
{
    public BiomeType biomeType;
    public PlanetTextureData planetTextureData;
    [SerializeField] private  ResourceRange[] resourceRanges;

    [System.Serializable]
    private struct ResourceRange
    {
        public ResourceType resourceType;
        public int lowest;
        public int highest;

    }

    public int[] GetRandomResourceAmounts()
    {
        int[] resources;
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        resources = new int[length];

        for(int i = 0; i < resourceRanges.Length; i++)
        {
            resources[(int)resourceRanges[i].resourceType] = Random.Range(resourceRanges[i].lowest,resourceRanges[i].highest+1);
        }
        return resources;
    }
}
