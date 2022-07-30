using UnityEngine;

[CreateAssetMenu(fileName = "Modifier", menuName = "AI/Considerations/Modifier")]
public class Modifier : Consideration
{
    public float minValue;
    public float maxValue;

    public override float Calculate(Option option, Analysis analysis)
    {
        return Random.Range(minValue, maxValue);
    }

    protected override Consideration CreateCopy()
    {
        Modifier copy = ScriptableObject.CreateInstance<Modifier>();
        copy.minValue = this.minValue;
        copy.maxValue = this.maxValue;
        return copy;
    }
}
