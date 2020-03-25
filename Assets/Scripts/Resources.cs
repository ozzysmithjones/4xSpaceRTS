public enum ResourceType
{
    FOOD,
    STABILITY,
    MATERIALS,
    SCIENCE
}

[System.Serializable]
public struct Resources
{
    public int[] amounts;

    public void Initialise()
    {
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        amounts = new int[length];
    }
    public void Clear()
    {
        for (int i = 0; i < amounts.Length; i++)
            amounts[i] = 0;
    }
}
