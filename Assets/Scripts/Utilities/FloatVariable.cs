using UnityEngine;

[CreateAssetMenu (fileName = "New Float Variable", menuName = "Variables/Float")]
public class FloatVariable : ScriptableObject
{
    public float Value;

    public void SetValue(float value)
    {
        Value = value;
    }

    public void ChangeValue(float amount)
    {
        Value += amount;
    }
}
