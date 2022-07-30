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
    [SerializeField] private ResourceRange[] resourceRanges = new ResourceRange[0];
    [SerializeField] private ResourceRange[] popProductionRanges = new ResourceRange[0];

    [System.Serializable]
    private struct ResourceRange
    {
        public ResourceType resourceType;
        public int lowest;
        public int highest;

        private ResourceRange(ResourceType resourceType, int lowest, int highest)
        {
            this.resourceType = resourceType;
            this.lowest = lowest;
            this.highest = highest;
        }

    }

    public int[] GetRandomProductionAmounts()
    {
        int[] production;
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        production = new int[length];

        for (int i = 0; i < popProductionRanges.Length; i++)
        {
            production[(int)popProductionRanges[i].resourceType] = Random.Range(popProductionRanges[i].lowest, popProductionRanges[i].highest + 1);
        }

        return production;
    }

    public int[] GetRandomResourceAmounts()
    {
        int[] resources;
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        resources = new int[length];

        for (int i = 0; i < resourceRanges.Length; i++)
        {
            resources[(int)resourceRanges[i].resourceType] = Random.Range(resourceRanges[i].lowest, resourceRanges[i].highest + 1);
        }
        return resources;
    }
}
