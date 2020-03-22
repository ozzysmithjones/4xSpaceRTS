public enum ResourceType
{
    FOOD,
    STABILITY,
    MATERIALS,
    SCIENCE
}

[System.Serializable]
public class Resources
{
    public int[] amounts = new int[4];

    public Resources()
    {
        int length = ResourceType.GetValues(typeof(ResourceType)).Length;
        amounts = new int[length];
    }

    public int Total()
    {
        int amount = 0;
        for (int i = 0; i < amounts.Length; i++)
            amount += amounts[i];
        return amount;
    }

    public void Clear()
    {
        for (int i = 0; i < amounts.Length; i++)
            amounts[i] = 0;
    }
}
