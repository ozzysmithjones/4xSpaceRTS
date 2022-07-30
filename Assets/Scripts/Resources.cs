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

    public Resources Clone()
    {
        Resources clone = new Resources();
        this.amounts.CopyTo(clone.amounts, 0);
        return clone;
    }

    public static Resources operator -(Resources a)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = a.amounts[i] * -1;

        return result;
    }

    public static Resources operator+(Resources a, Resources b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = a.amounts[i] + b.amounts[i];

        return result;
    }
    public static Resources operator-(Resources a, Resources b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = a.amounts[i] - b.amounts[i];

        return result;
    }

    public static Resources operator *(Resources a, Resources b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = a.amounts[i] * b.amounts[i];

        return result;
    }

    public static Resources operator /(Resources a, Resources b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = a.amounts[i] / b.amounts[i];

        return result;
    }

    public static Resources operator*(Resources a, float b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = UnityEngine.Mathf.RoundToInt(a.amounts[i] * b);

        return result;
    }


    public static Resources operator /(Resources a, float b)
    {
        Resources result = new Resources();

        for (int i = 0; i < result.amounts.Length; i++)
            result.amounts[i] = UnityEngine.Mathf.RoundToInt(a.amounts[i] / b);

        return result;
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
